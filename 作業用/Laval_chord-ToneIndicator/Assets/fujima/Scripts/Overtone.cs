using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class Overtone : NetworkBehaviour
{
    private AudioSource audioSource; // AudioSource型の変数を宣言
    public float frequency = 1320f; // 出力する周波数
    private float duration = 1f; // サイン波の長さ (秒)
    private float sampleRate = 22050f; // サンプルレート (通常44.1kHz)
    public float threshold = 1000f; // 強度の閾値
    public NetworkAnimator networkAnimator;

    private float keyPressDuration = 0f; // Vキーの押下時間
    private bool isPlaying = false;
    private bool isPlayingForFourSeconds = false; // 4秒間再生されたかどうかを記録するフラグ
    private float latestIntensity = 0f; // 最新の合計強度データ
    public Animator metarigAnimator;
    private bool isExtending = false; // 5秒間音を維持するフラグ

    void Start()
    {
        // AudioSourceコンポーネントを取得または追加
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 1f;
        audioSource.loop = true; // ループ再生を設定

        // 初回のサイン波を生成して再生
        UpdateSineWave();
    }


    void Update()
    {
        latestIntensity = AudioManager.LatestIntensity; // Exportudpから最新の合計強度データを取得

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("Female/女性");
            frequency = 1320f;
            UpdateSineWave();
            audioSource.Stop();
        }
        else if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            Debug.Log("Male/男性");
            frequency = 660f;
            UpdateSineWave();
            audioSource.Stop();
        }

        if (Keyboard.current != null)
        {
            if (Keyboard.current.vKey.isPressed)
            {
                keyPressDuration += Time.deltaTime;
                Debug.Log($"Vキーが押されています！ keyPressDuration: {keyPressDuration}");

                if (keyPressDuration >= 5f && !isPlaying)
                {
                    Debug.Log("5秒間押し続けられました！アニメーションを開始します。");
                    StartCoroutine(TriggerAnimation());
                }
            }
            else
            {
                if (keyPressDuration > 0)
                {
                    Debug.Log("Vキーを離しました。keyPressDuration をリセット。");
                }
                keyPressDuration = 0f;
            }
        }
        else
        {
            Debug.LogError("⚠ Keyboard.current が null です！Input System の設定を確認してください。");
        }
    }

    IEnumerator TriggerAnimation()
    {
        isPlaying = true; // アニメーション開始フラグ

        if (networkAnimator != null)
        {
            CmdSetTrigger("PlayAnimation"); // ✅ サーバーにアニメーション開始を指示
            Debug.Log("PlayAnimation トリガーを設定しました。");
        }
        else
        {
            Debug.LogError("NetworkAnimator が設定されていません。");
        }

        yield return new WaitForSeconds(1f);
        isPlaying = false;
    }

    [Command]
    void CmdSetTrigger(string triggerName)
    {
        if (networkAnimator != null)
        {
            networkAnimator.SetTrigger(triggerName);
            RpcSetTrigger(triggerName);
        }
    }

    [ClientRpc]
    void RpcSetTrigger(string triggerName)
    {
        if (networkAnimator != null)
        {
            networkAnimator.SetTrigger(triggerName);
        }
    }

    private AudioClip CreateSineWave(float frequency, float duration, float sampleRate)
    {
        int sampleCount = (int)(duration * sampleRate);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate);
        }

        AudioClip audioClip = AudioClip.Create("SineWave", sampleCount, 1, (int)sampleRate, false);
        audioClip.SetData(samples, 0);

        return audioClip;
    }

    private void UpdateSineWave()
    {
        AudioClip sineWaveClip = CreateSineWave(frequency, duration, sampleRate);
        audioSource.clip = sineWaveClip;
    }

    public bool IsPlayingForFourSeconds()
    {
        return isPlayingForFourSeconds;
    }
}
