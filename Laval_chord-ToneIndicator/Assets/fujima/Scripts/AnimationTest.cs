using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput Systemを使用
using Mirror;

public class AnimationTest : NetworkBehaviour
{
    private Animator m_Animator;
    private float keyPressTime = 0f;
    private const float thresholdTime = 3f;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // "s"キーを押し続けた時間を測定（新しい Input System）
        if (Keyboard.current.sKey.isPressed)
        {
            keyPressTime += Time.deltaTime;
        }
        else
        {
            keyPressTime = 0f;
        }

        // 3秒以上押し続けたら isPlaying を true にする
        if (keyPressTime >= thresholdTime)
        {
            m_Animator.SetBool("isplaying", true);
            Debug.Log("\"s\"キーを3秒間押し続けた");
        }
    }
}
