using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Overtone : MonoBehaviour
{
    private AudioSource audioSource; // AudioSource型の変数を宣言
    public float frequency = 1320f; // 出力する周波数
    private float duration = 1f; // サイン波の長さ (秒)
    private float sampleRate = 22050f; // サンプルレート (通常44.1kHz)
    public float threshold = 1000f; // 強度の閾値

    private bool isPlaying = false;
    private bool isPlayingForFourSeconds = false; // 4秒間再生されたかどうかを記録するフラグ
    private float latestIntensity = 0f; // 最新の合計強度データ

    void Start()
    {
        // AudioSourceコンポーネントを取得または追加
        audioSource = gameObject.AddComponent<AudioSource>();
        GetComponent<AudioSource>().volume = 1f;
        audioSource.loop = true; // ループ再生を設定
        
        // 初回のサイン波を生成して再生
        UpdateSineWave();
    }

    void Update()
    {
        // 合計強度データを即座にチェック
        latestIntensity = AudioManager.LatestIntensity; // Exportudpから最新の合計強度データを取得

        // ターゲット周波数帯域にあるスペクトルの合計強度が閾値を超えているかチェック
        //Debug.Log(threshold); 
        //Debug.Log($"latestIntensity threshold: {latestIntensity} {threshold} ");

        if(Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("Female/女性");
            frequency = 1320f;
            UpdateSineWave();  // サイン波を更新
            audioSource.Stop(); // 既存の音を止める
            audioSource.Play();
        }
        else if(Keyboard.current.mKey.wasPressedThisFrame)
        {
            Debug.Log("Male/男性");
            frequency = 660f;
            UpdateSineWave();  // サイン波を更新
            audioSource.Stop(); // 既存の音を止める
            audioSource.Play();
        }
        
        if (latestIntensity > threshold)
        {
            Debug.Log("get in");
            if (!isPlaying)
            {
                UpdateSineWave();
                Debug.Log("音が来た");
                audioSource.Play(); // 一致する場合、音を再生
                isPlaying = true;

                StartCoroutine(CheckIfPlayingForFourSeconds());

            }
        }
        else
        {
            //Debug.Log("get down");
            if (isPlaying)
            {
                audioSource.Stop(); // 一致しない場合、音を停止
                isPlaying = false;
                StopCoroutine(CheckIfPlayingForFourSeconds()); // 音が止まったらコルーチンも停止
                isPlayingForFourSeconds = false; // フラグをリセット
            }
        }
    }

    // 6秒間音が再生されたかを確認するコルーチン
    IEnumerator CheckIfPlayingForFourSeconds()
    {
        yield return new WaitForSeconds(6f); // 6秒待機
        if (isPlaying)
        {
            isPlayingForFourSeconds = true; // 6秒間音が再生されたことを記録
            Debug.Log("音が4秒間再生されました。");
        }
    }

    // サイン波を生成する関数
    private AudioClip CreateSineWave(float frequency, float duration, float sampleRate)
    {
        int sampleCount = (int)(duration * sampleRate); // サンプル数を計算
        float[] samples = new float[sampleCount]; // サンプルデータを格納する配列

        // サイン波の各サンプル値を計算して生成
        for (int i = 0; i < sampleCount; i++)
        {
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate);
        }

        // AudioClipを生成し、サイン波データを設定
        AudioClip audioClip = AudioClip.Create("SineWave", sampleCount, 1, (int)sampleRate, false);
        audioClip.SetData(samples, 0);

        return audioClip;
    }

    // サイン波のクリップを更新する関数
    private void UpdateSineWave()
    {
        AudioClip sineWaveClip = CreateSineWave(frequency, duration, sampleRate);
        audioSource.clip = sineWaveClip;
    }

    // 4秒間再生されたかを外部から取得するためのプロパティ
    public bool IsPlayingForFourSeconds()
    {
        return isPlayingForFourSeconds;
    }
}
