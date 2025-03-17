using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput Systemを使用
using Mirror;

public class AnimationTest : NetworkBehaviour
{
    public Animator m_Animator;
    private bool isAnimationTriggered = false; // アニメーションを制御するフラグ

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void StartAnimation() // アニメーションを開始するメソッド
    {
        if (!isAnimationTriggered) // すでに実行されていなければ開始
        {
            m_Animator.SetBool("isplaying", true);
            isAnimationTriggered = true; // フラグを立てて二重実行を防ぐ
        }
    }

    public void StopAnimation() // アニメーションを停止するメソッド
    {
        m_Animator.SetBool("isplaying", false);
        isAnimationTriggered = false;
    }
}
