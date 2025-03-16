using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerController : NetworkBehaviour
{
    public float CorrectVoicePowerThreshold = 1500.0f;
    public float MaxHarmonicAdditionalPowerLevel = 1000.0f;

    float[] voiceFreq = { 264, 330 };
    float[] harmFreq = { 1320, 2640 };

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
        if(!isServer) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length == 0) return;

        bool isHarmonic = true;
        float harmonicLevel = 0;
        foreach(var player in players)
        {
            PlayerManager pm = player.GetComponent<PlayerManager>();
            isHarmonic = isHarmonic && (pm.correctVoicePower > CorrectVoicePowerThreshold);
            harmonicLevel += pm.correctVoicePower - CorrectVoicePowerThreshold;
        }

        harmonicLevel /= players.Length;
        harmonicLevel /= MaxHarmonicPowerThreshold;
        if( harmonicLevel > 1.0f ) harmonicLevel = 1.0f; else if(harmonicLevel < 0) harmonicLevel = 0;
        if(!isHarmonic) harmonicLevel =0;

        foreach(var player in players)
        {
            PlayerManager pm = player.GetComponent<PlayerManager>();
            pm.harmonicI = harmonicLevel;
        }
    }

    public void NewClient(uint cid)
    {
        if (!isServer) return;

        Debug.Log($"Client {cid} connected");
        if (clientCount == 2)
        {
            Debug.Log("Too Many Clients");
            return;
        }
        clients[clientCount] = cid;
        clientCount++;
    }
}
