using Unity.Netcode;
using UnityEngine;

public class ConnectionDebugger : MonoBehaviour
{
    void Update()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log($"🔍 現在の接続クライアント数: {NetworkManager.Singleton.ConnectedClients.Count}");

            foreach (var client in NetworkManager.Singleton.ConnectedClients)
            {
                Debug.Log($"🆔 クライアントID: {client.Key}");
            }
        }
    }
}
