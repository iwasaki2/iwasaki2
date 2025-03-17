using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerController : NetworkBehaviour
{
    public float CorrectVoicePowerThreshold = 1500.0f;
    public float MaxHarmonicAdditionalPowerLevel = 1000.0f;
    private Coroutine animationCoroutine;

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
            if (animationCoroutine == null) // ???????????????????
            {
                animationCoroutine = StartCoroutine(TriggerAnimationAfterDelay());
            }
        }
        else
        {
            if (animationCoroutine != null) // isHarmonic ? false ????????????
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null; // ???????
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
        }

    }
    private IEnumerator TriggerAnimationAfterDelay()
    {
        yield return new WaitForSeconds(4.0f); // 4???
        Debug.Log("??????????");

        AnimationTest animTest = FindObjectOfType<AnimationTest>();
        if (animTest != null)
        {
            animTest.StartAnimation(); // ?????????????
        }
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
}
