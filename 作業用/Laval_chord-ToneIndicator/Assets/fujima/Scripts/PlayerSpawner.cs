using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpawner : NetworkManager
{
    public Transform spawnPointHost;  // ホストのスポーン位置
    public Transform spawnPointClient; // クライアントのスポーン位置

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform spawnPoint;

        // ホスト（最初のプレイヤー）は spawnPointHost にスポーン
        if (numPlayers == 0)
        {
            spawnPoint = spawnPointHost;
        }
        // クライアント（2人目以降）は spawnPointClient にスポーン
        else
        {
            spawnPoint = spawnPointClient;
        }

        Vector3 spawnPosition = spawnPoint.position;
        Quaternion spawnRotation = spawnPoint.rotation;

        // プレイヤーをスポーン
        GameObject player = Instantiate(playerPrefab, spawnPosition, spawnRotation);
        NetworkServer.AddPlayerForConnection(conn, player);

        // カメラの視点を設定
        SetupCamera(player);
    }

    private void SetupCamera(GameObject player)
    {
        Camera playerCamera = player.GetComponentInChildren<Camera>();

        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);  // 自分のカメラのみ有効化
        }
    }
}
