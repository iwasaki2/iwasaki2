using UnityEngine;
using UnityEngine.Rendering.PostProcessing; // Post Processingの名前空間


public class PostProcessingController : MonoBehaviour
{
    // Inspectorで設定するPostProcessVolume
    public PostProcessVolume postProcessVolume;
    public Overtone overtone;

    // 取得するエフェクト用の変数
    private Vignette vignette;
    


    private Coroutine timerCoroutine = null; // タイマーコルーチンを管理する変数
    private Coroutine attackCoroutine = null; // 10秒ごとの攻撃用のコルーチン
    private bool isPostProcessingChanged = false;
    public float duration = 2f;                // Intensityを0にするまでの時間

    private float timer = 0f; 

    public bool IsPostProcessingChanged
    {
        get { return isPostProcessingChanged; }
    }

    void Start()
    {
        // PostProcessVolumeからプロファイルのエフェクトを取得する
        if (postProcessVolume != null && postProcessVolume.profile != null)
        {
            // Bloomを取得
            if (postProcessVolume.profile.TryGetSettings(out vignette))
            {
                Debug.Log("vignetteが取得されました");
            }
        }
    }

    void Update()
    {
        if (overtone != null && overtone.IsPlayingForFourSeconds())
        {
            Debug.Log("条件達成");         
            timer += Time.deltaTime;
            if (vignette != null)
            {
                // VignetteのIntensityを動的に変更 (0〜5の範囲で変化)
                float startIntensity = vignette.intensity.value;
                vignette.intensity.value = Mathf.Lerp(startIntensity, 0f, timer / duration);
            }
            Debug.Log("明るくなった");


            
            isPostProcessingChanged = true;
            

            // タイマーが動いていたら停止
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null; // コルーチンをリセット
                Debug.Log("タイマーを停止しました。");
            }
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }

        }
    
    }
}
