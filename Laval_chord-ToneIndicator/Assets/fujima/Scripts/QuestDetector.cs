using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
using TMPro;

public class QuestDetector : NetworkDiscovery
{
    public NetworkManager nm;
    public NetworkManagerHUD hud;

    private ServerResponse sr;

    public TextMeshProUGUI text;

    bool guard = false;

    private void Update()
    {
        if (guard) return;

        guard = true;
        text.text = "Search Start";
        hud.enabled = false;
        OnServerFound.AddListener(serverResponse => { sr = serverResponse; });
        StartCoroutine("ClientConnect");
    }

    IEnumerator ClientConnect()
    {
        StartDiscovery();
        text.text = "Discovery Start";

        while (!nm.isNetworkActive)
        {
            if (sr.uri != null)
            {
                text.text = "Discovered";
                nm.StartClient(sr.uri);
                StopDiscovery();    
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
