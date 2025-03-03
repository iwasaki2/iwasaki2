using UnityEngine;
using MathNet.Numerics.IntegralTransforms; // FFTに必要
using System.Linq;
using System.Collections.Generic;
using System.Numerics; 



public class AudioManager : MonoBehaviour
{
    private const int sampleRate = 22050; // サンプルレート
    private const int fftSize = 22050; // FFTサイズ
    private const float targetFreq = 330.0f; // ターゲット周波数 (165Hz)
    private const float bandWidth = 10.0f;  // バンド幅 (10Hz)

    private string micName;
    private AudioClip micInput;
    private float[] samples;
    private Complex[] spectrum; // Complex型の配列
    private Queue<float> recentData = new Queue<float>(); // 最近の周波数データ

    public static float LatestIntensity { get; private set; } 
    public static float LatestZ { get; private set; } // 最新のZ値
    public ToneIndicator toneIndicator; // ToneIndicator スクリプトへの参照

    void Start()
    {
        // マイクの設定
        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0]; // 最初のマイクデバイスを選択
            micInput = Microphone.Start(micName, true, 1, sampleRate); // 録音開始
            samples = new float[fftSize]; // サンプル配列
            spectrum = new Complex[fftSize]; // FFTスペクトル用の配列
            //Debug.Log("Recording started: " + micName);
        }
        else
        {
            //Debug.LogError("No microphone found!");
        }
    }

    void Update()
    {
        if (Microphone.IsRecording(micName))
        {
            // マイクからデータを取得
            micInput.GetData(samples, 0);

            // FFTを実行
            FFT(samples, spectrum);

            // 振幅スペクトルの計算
            float[] amplitude = spectrum.Select(c => (float)c.Magnitude).ToArray();

            // 最大振幅とその周波数を取得
            int maxIndex = amplitude.ToList().IndexOf(amplitude.Max());
            float maxAmp = amplitude[maxIndex];
            float maxFreq = (maxIndex * sampleRate) / (float)fftSize;

            // ターゲット周波数帯域の強度を計算
            LatestIntensity = 0;
            for (int i = 0; i < fftSize / 2; i++)
            {
                float freq = (i * sampleRate) / (float)fftSize;
                if (freq >= targetFreq - bandWidth / 2 && freq <= targetFreq + bandWidth / 2)
                {
                    LatestIntensity += amplitude[i];
                }
            }

            // Z値（最近の最大周波数の平均）
            recentData.Enqueue(maxFreq);
            if (recentData.Count > 3) recentData.Dequeue(); // 最近3回の最大周波数
            LatestZ = recentData.Average(); // Z値はその平均

            // デバッグログ
            //Debug.Log($"Max Frequency: {maxFreq} Hz, Max Amplitude: {maxAmp}, LatestIntensity: {LatestIntensity}, Z-value: {LatestZ}");

            // ToneIndicatorスクリプトに反映
            if (toneIndicator != null)
            {
                toneIndicator.SetCurrentFreq(LatestZ);
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
