using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ConnectionManager : NetworkBehaviour
{
    ServerController sc;

    void Start()
    {
        Debug.Log("here2");
        if (!isServer) return;
        Debug.Log("here");
        GameObject.FindGameObjectWithTag("Server").GetComponent<ServerController>().NewClient(this.netId);
    }

}
