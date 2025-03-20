using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerController : NetworkBehaviour
{
    public float CorrectVoicePowerThreshold = 1500.0f;
    public float MaxHarmonicAdditionalPowerLevel = 1000.0f;
    public float VoiceLength = 1.0f;
    private Coroutine animationCoroutine;

    public AnimationTest at;


    float[] targetFreqs = { 132, 165 };
    float[] harmonicFreq = { 660, 1320 };

    uint[] clients = { 0, 0 };
    int clientCount;

    // Start is called before the first frame update
    void Start()
    {
        clientCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int playerCount = players.Length;
        if (playerCount == 0) return;

        bool isHarmonic = true;
        float harmonicLevel = 0;
        foreach (var player in players)
        {
            PlayerManager pm = player.GetComponent<PlayerManager>();
            isHarmonic = isHarmonic && (pm.correctVoicePower > CorrectVoicePowerThreshold);
            harmonicLevel += pm.correctVoicePower - CorrectVoicePowerThreshold;
        }
        //ture????????????
        if (isHarmonic)
        {
            if (animationCoroutine == null) // 初回だけ通る
            {
                animationCoroutine = StartCoroutine(TriggerAnimationAfterDelay());
            }
        }
        else
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
                at.SetUtoutoFalse();  // 即座にうとうと解除
            }
        }

        harmonicLevel /= playerCount;
        harmonicLevel /= MaxHarmonicAdditionalPowerLevel;
        if( harmonicLevel > 1.0f ) harmonicLevel = 1.0f; else if(harmonicLevel < 0) harmonicLevel = 0;
        if(!isHarmonic) harmonicLevel =0;

        foreach(var player in players)
        {
            PlayerManager pm = player.GetComponent<PlayerManager>();
            pm.harmonicI = harmonicLevel;
        }

        if(playerCount == 1)
        {
            Debug.Log("here");
            players[0].GetComponent<PlayerManager>().anotherVoiceI = 1.0f;
        }

        bool isHighHarmonic = false;// false???????????
        foreach (var player in players)
        {
            PlayerManager pm = player.GetComponent<PlayerManager>();
            if (pm.isLower && pm.isFemale) isHighHarmonic = true;
        }

        foreach (var player in players)
        {
            PlayerManager pm = player.GetComponent<PlayerManager>();
            pm.harmonicF = isHighHarmonic ? harmonicFreq[1] : harmonicFreq[0];

            float tf = (pm.isLower) ? targetFreqs[0] : targetFreqs[1];
            if (pm.isFemale) tf *= 2;
            pm.targetFreq = tf;

            float af = (!pm.isLower) ? targetFreqs[0] : targetFreqs[1];
            if (pm.isFemale) af *= 2;
            pm.anotherVoiceF = af;
        }

    }

    private IEnumerator TriggerAnimationAfterDelay()
    {
        Debug.Log("ハーモニク検知、コルーチン開始");

        // ここでutoutoアニメーション開始
        at.SetUtoutoTrue();

        // VoiceLengthの間、うとうと状態
        yield return new WaitForSeconds(VoiceLength);

        Debug.Log("コルーチン終了、アニメーション停止");

        // アニメーション停止（utouto解除）
        at.StartAnimation();

        // コルーチン完了
        animationCoroutine = null;
    }

    public void NewClient(PlayerManager pm)
    {
        if (!isServer) return;

        Debug.Log($"Client {pm.netId} connected");
        pm.isLower = (clientCount == 0);

        if (clientCount == 2)
        {
            Debug.Log("Too Many Clients");
            return;
        }
        clients[clientCount] = pm.netId;

        clientCount++;
    }
    


    public void RestartGame()
    {
        Debug.Log("Restart Called");
        at.StopAnimation();
    }
}
