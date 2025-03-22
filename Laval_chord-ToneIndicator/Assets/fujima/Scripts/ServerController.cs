using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerController : NetworkBehaviour
{
    public float CorrectVoicePowerThreshold = 1500.0f;
    public float MaxHarmonicAdditionalPowerLevel = 1000.0f;
    public float VoiceLength = 1.0f;
    public bool isHarmonic = true;
    private Coroutine harmonicCoroutine = null;
    private bool isMainAnimationStarted = false;

    public AnimationTest at;

    float[] targetFreqs = { 132, 165 };
    float[] harmonicFreq = { 660, 1320 };
    uint[] clients = { 0, 0 };
    int clientCount;

    void Start()
    {
        clientCount = 0;
    }

    void Update()
    {
        if (!isServer) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        clientCount = players.Length;
        if (clientCount == 0) return;

        isHarmonic = true;
        float harmonicLevel = 0;
        foreach (var player in players)
        {
            PlayerManager pm = player.GetComponent<PlayerManager>();
            isHarmonic = isHarmonic && (pm.correctVoicePower > CorrectVoicePowerThreshold);
            harmonicLevel += pm.correctVoicePower - CorrectVoicePowerThreshold;
        }

        at = FindObjectOfType<AnimationTest>();
        if (at == null) return;

        if (isHarmonic && !isMainAnimationStarted)
        {
            at.utostaAnimation();  // ウトウト再生
            if (harmonicCoroutine == null)
            {
                harmonicCoroutine = StartCoroutine(HarmonicCheckCoroutine());
            }
        }
        else
        {
            if (harmonicCoroutine != null)
            {
                StopCoroutine(harmonicCoroutine);
                harmonicCoroutine = null;
            }
            at.utostoAnimation();  // ウトウト停止
        }

        // harmonicLevel計算処理
        harmonicLevel /= clientCount;
        harmonicLevel /= MaxHarmonicAdditionalPowerLevel;
        harmonicLevel = Mathf.Clamp01(harmonicLevel);
        if (!isHarmonic) harmonicLevel = 0;

        foreach (var player in players)
        {
            PlayerManager pm = player.GetComponent<PlayerManager>();
            pm.harmonicI = harmonicLevel;
        }

        if (clientCount == 1)
        {
            Debug.Log("here");
            players[0].GetComponent<PlayerManager>().anotherVoiceI = 1.0f;
        }
        else
        {
            players[0].GetComponent<PlayerManager>().anotherVoiceI = 0f;
            players[1].GetComponent<PlayerManager>().anotherVoiceI = 0f;
        }


            bool isHighHarmonic = false;
        foreach (var player in players)
        {
            PlayerManager pm = player.GetComponent<PlayerManager>();
            if (pm.isLower && pm.isFemale) isHighHarmonic = true;
        }

        foreach (var player in players)
        {
            PlayerManager pm = player.GetComponent<PlayerManager>();
            pm.harmonicF = isHighHarmonic ? harmonicFreq[1] : harmonicFreq[0];

            float tf = pm.isLower ? targetFreqs[0] : targetFreqs[1];
            if (pm.isFemale) tf *= 2;
            pm.targetFreq = tf;

            float af = !pm.isLower ? targetFreqs[0] : targetFreqs[1];
            if (pm.isFemale) af *= 2;
            pm.anotherVoiceF = af;
        }
    }

    private IEnumerator HarmonicCheckCoroutine()
    {
        Debug.Log("Harmonic Detected, Waiting...");
        yield return new WaitForSeconds(VoiceLength);  // VoiceLength秒待機
        Debug.Log("Voice sustained, starting main animation.");
        if (at != null)
        {
            at.StartAnimation();
            isMainAnimationStarted = true;
        }
        harmonicCoroutine = null;
    }

    public void NewClient(PlayerManager pm)
    {
        if (!isServer) return;

        Debug.Log($"Client {pm.netId} connected");
        pm.isLower = (clientCount == 0);

        if (clientCount == 0)
        {
            pm.isLower = true;
            pm.anotherVoiceI = 1.0f;
        }
        if (clientCount == 1)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            pm.isLower = !player.GetComponent<PlayerManager>().isLower;
            pm.anotherVoiceI = 0.0f;
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restart Called");
        at.StopAnimation();
        isMainAnimationStarted = false;
    }
}
