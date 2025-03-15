using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    [SyncVar]
    public float targetFreqF;
    [SyncVar]
    public float targetFreqI;
    [SyncVar]
    public float anotherVoiceF;
    [SyncVar]
    public float anotherVoiceI;
    [SyncVar]
    public float harmonicF;
    [SyncVar]
    public float harmonicI;

    void Start()
    {
        if (isServer) // サーバーでのみ初期値を設定
        {
            targetFreqF = 330;
            targetFreqI = 660;
            anotherVoiceF = 264;
            anotherVoiceI = 528;
            harmonicF = 1320;
            harmonicI = 2640;
        }
    }

    // サーバーが特定のクライアントにデータを送る
    [TargetRpc]
    public void TargetUpdateFrequencies(NetworkConnectionToClient target)
    {
        targetFreqF = 660;
        anotherVoiceF = 528;
        harmonicI = 2640;

        Debug.Log($"クライアント {netId} にデータを送信: targetFreqF={targetFreqF}, anotherVoiceF={anotherVoiceF}, harmonicI={harmonicI}");
    }
}
