using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientConnector : MonoBehaviour
{
    NetworkManager nm;
    public NetworkManagerHUD hud;
    public GameObject temporalCamera;

    bool beingServer;
    // Start is called before the first frame update
    void Start()    // Start() is later than Awake()
    {
        nm = GetComponent<NetworkManager>();
        // nm.networkAddress = "localhost";
        Transport transport = nm.GetComponent<Transport>();
        nm.StartClient();
        beingServer = false;
    }

    void Update()
    {
        Debug.Log(NetworkClient.isConnected);
        if( NetworkClient.isConnected )
        {
            temporalCamera.SetActive(false);
        }
/*
        if (NetworkClient.isConnected && !NetworkClient.ready)
        {
            // client ready
            NetworkClient.Ready();
            if (NetworkClient.localPlayer == null)
                NetworkClient.AddPlayer();
        }
*/
        if (Input.GetKeyDown(KeyCode.S))
        {
            nm.StopClient();
            beingServer = true;
            hud.enabled = true;
        }
    }
/*
    IEnumerator ConnectionTrial()
    {
        while(true)
        {
            if( !NetworkClient.isConnected )
            {
                Debug.Log("Trying to connect");
                GetComponent<NetworkManager>().StartClient();
                yield return new WaitForSeconds(10);
            }
        }
    }
*/
}
