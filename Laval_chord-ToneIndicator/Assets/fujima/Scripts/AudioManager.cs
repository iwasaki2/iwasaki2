using UnityEngine;
using MathNet.Numerics.IntegralTransforms; // FFTに必要
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

using Mirror;

using System.IO;
using UnityEngine.InputSystem;

public class AudioManager : NetworkBehaviour
{
    private const int sampleRate = 22050; // サンプルレート
    private const int fftSize = 22050; // FFTサイズ
    private const float bandWidth = 10.0f;  // バンド幅 (10Hz)

    private string micName;
    private AudioClip micInput;
    private float[] samples;
    private Complex[] spectrum; // Complex型の配列
    private Queue<float> recentData = new Queue<float>(); // 最近の周波数データ

//    public static float LatestIntensity { get; private set; }
//    public static float LatestZ { get; private set; } // 最新のZ値
    public ToneIndicator toneIndicator; // ToneIndicator スクリプトへの参照

    StreamWriter sw;

    public PlayerManager playerManager;

    void Start()
    {
        if(!isLocalPlayer) return;

        // マイクの設定
        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0]; // 最初のマイクデバイスを選択
            micInput = Microphone.Start(micName, true, 1, sampleRate); // 録音開始
            samples = new float[fftSize]; // サンプル配列
            spectrum = new Complex[fftSize]; // FFTスペクトル用の配列
        }
        else
        {
            //Debug.LogError("No microphone found!");
        }
        //sw = new StreamWriter("voice.txt");
    }

    private void OnDestroy()
    {
       // sw.Close();
    }

    void Update()
    {
        if(!isLocalPlayer) return;

        float targetFreq = playerManager.targetFreq;
        toneIndicator.SetTargetFreq(targetFreq);
        toneIndicator.SetFreqRange(targetFreq / 5);

        if (Microphone.IsRecording(micName))
        {
            // マイクからデータを取得
            micInput.GetData(samples, 0);

            // FFTを実行
            FFT(samples, spectrum);

            // 振幅スペクトルの計算
            float[] amplitude = spectrum.Select(c => (float)c.Magnitude).ToArray();

           /* if( Keyboard.current.rKey.wasPressedThisFrame )
            {
                for(int i=0; i<1500; i++)
                {
                    float x = amplitude[i];
                    sw.Write(x);
                    sw.Write(",");
                }
                sw.WriteLine("");
            }

            */
            // 参照用に最大値を持ってくる
            float maxa = 0;
            foreach (var f in amplitude)
                if (maxa < f)
                    maxa = f;

            // 振幅のピークであって、最大値の 1/2 を超えるような山を検出する
            float PEAKRATIO = 0.5f;
            Dictionary<int, float> peaks;
            peaks = new Dictionary<int, float>();
            int maxIndex = 0;
            bool isUp;

            float lsum, rsum;
            lsum = rsum = 0;
            int winsize = 10;
            for (int i = 0; i < winsize; i++)
                lsum += amplitude[i];
            for (int i = 0; i < winsize; i++)
                rsum += amplitude[i + winsize];
            isUp = (lsum < rsum);
            maxIndex = 0;
            for (int i=0; i < 2000; i++)
            {
                if(isUp && (lsum >= rsum) && lsum > maxa * PEAKRATIO && lsum>100)
                {
//                    Debug.Log($"{i} , {lsum} , {rsum}");
                    peaks[i + winsize] = lsum + rsum;
                    maxIndex = i;
                }
                isUp = (lsum < rsum);
                lsum = lsum - amplitude[i] + amplitude[i + winsize];
                rsum = rsum - amplitude[i + winsize] + amplitude[i + winsize * 2];
            }

            // 山のうち、一番音の低い山をとってくる
            foreach (var (i, p) in peaks)
                if (i < maxIndex)
                    maxIndex = i;

            if (peaks.ContainsKey(maxIndex))
            {
                float maxAmp = peaks[maxIndex] / winsize;
                float maxFreq = (maxIndex * sampleRate) / (float)fftSize;

                // ターゲット周波数帯域の強度を計算
                float targetPower = 0;
                for (int i = 0; i < fftSize / 2; i++)
                {
                    float freq = (i * sampleRate) / (float)fftSize;
                    if (freq >= targetFreq - bandWidth / 2 && freq <= targetFreq + bandWidth / 2)
                    {
                        targetPower += amplitude[i];
                    }
                }
                // ToneIndicatorスクリプトに反映
                if (toneIndicator != null)
                {
                    toneIndicator.SetCurrentFreq(maxFreq);
                }

                playerManager.CmdCurrentVoice(targetPower);

                Debug.Log($"Max Frequency: {maxFreq} Hz, Max Amplitude: {maxAmp}, Power: {targetPower}");
            }
            else
            {
                playerManager.CmdCurrentVoice(0);
            }

        }
    }

    void OnApplicationQuit()
    {
        // 終了時にマイクの録音停止
        Microphone.End(micName);
    }

    // FFT計算
    private void FFT(float[] input, Complex[] output)
    {
        for (int i = 0; i < input.Length; i++)
        {
            output[i] = new Complex(input[i], 0); // 実数部分のみ
        }

        Fourier.Forward(output, FourierOptions.Matlab); // FFT実行
    }
}