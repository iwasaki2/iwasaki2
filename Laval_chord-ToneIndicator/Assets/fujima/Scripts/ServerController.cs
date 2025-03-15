using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerController : NetworkBehaviour
{
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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(var player in players)
        {
            PlayerManager pm = player.GetComponent<PlayerManager>();
            Debug.Log(player.name);
            Debug.Log(pm.correctVoicePower);
            if (pm.correctVoicePower > 100.0f)
                pm.harmonicI = 1.0f;
            else
                pm.harmonicI = 0.0f;
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
