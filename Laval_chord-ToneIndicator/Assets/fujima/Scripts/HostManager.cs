using Mirror;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    void Start()
    {
        if (!NetworkServer.active)
        {
            Debug.Log("ホストを開始します...");
            NetworkManager.singleton.StartHost();

            // クライアント接続時のイベントを登録
            NetworkServer.OnConnectedEvent += OnClientConnected;
        }
    }

    void OnClientConnected(NetworkConnectionToClient conn)
    {
        Debug.Log($"クライアントが接続しました: ID = {conn.connectionId}");
    }
}
