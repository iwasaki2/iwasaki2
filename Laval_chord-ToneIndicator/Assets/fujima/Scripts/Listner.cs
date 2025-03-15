using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Listner : NetworkBehaviour
{
    public AudioListener al;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            al.enabled = false;
    }
}
