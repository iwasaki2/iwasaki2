using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput Systemを使用
using Mirror;

public class AnimationTest : NetworkBehaviour
{
    public Animator m_Animator;

    private bool isMainAnimationStarted = false;  // 本編開始フラグ
    private bool isUtoutoPlaying = false;         // ウトウト再生中フラグ

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // 本編アニメーション開始（優先度最高）
    public void StartAnimation()
    {
        if (!isMainAnimationStarted)
        {
            m_Animator.SetBool("isplaying", true);   // 本編スタート
            m_Animator.SetBool("utouto", false);     // ウトウト強制終了
            isMainAnimationStarted = true;
            isUtoutoPlaying = false;
        }
    }

    // ウトウトアニメーション（本編開始前のみ許可）
    public void utostaAnimation()
    {
        // 本編始まったら、もうウトウトは動かさない
        if (isMainAnimationStarted) return;

        if (!isUtoutoPlaying)
        {
            m_Animator.SetBool("utouto", true);
            isUtoutoPlaying = true;
        }
    }
    public void utostoAnimation()
    {
        if (isUtoutoPlaying)
        {
            m_Animator.SetBool("utouto", false);
            isUtoutoPlaying = false;
        }
    }

    // すべて停止（必要なら）
    public void StopAnimation()
    {
        m_Animator.SetBool("isplaying", false);
        m_Animator.SetBool("utouto", false);
        isMainAnimationStarted = false;
        isUtoutoPlaying = false;
    }
}