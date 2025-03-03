using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn == null)
        {
            Debug.LogWarning("OnServerDisconnect: conn is null, skipping.");
            return;
        }

        if (conn.identity == null)
        {
            Debug.LogWarning("OnServerDisconnect: conn.identity is null, skipping destroy.");
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopHost()
    {
        Debug.Log("ホストを停止中...");

        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn != null && conn.identity != null)
            {
                NetworkServer.Destroy(conn.identity.gameObject);
            }
        }

        base.OnStopHost();
    }
}
