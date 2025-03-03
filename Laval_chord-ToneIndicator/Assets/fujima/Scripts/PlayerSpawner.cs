using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpawner : NetworkManager
{
    public Transform[] spawnPoints; // スポーンポイントの配列

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Error: spawnPoints is not assigned or empty. Please assign spawn points in the Inspector.");
            return;
        }

        int index = conn.connectionId % spawnPoints.Length;
        Vector3 spawnPosition = spawnPoints[index].position;
        Quaternion spawnRotation = spawnPoints[index].rotation;

        // プレイヤーをスポーン（Rigidbodyは不要）
        GameObject player = Instantiate(playerPrefab, spawnPosition, spawnRotation);
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
