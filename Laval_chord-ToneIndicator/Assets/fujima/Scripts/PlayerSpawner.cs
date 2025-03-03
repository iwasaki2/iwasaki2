using UnityEngine;
using Mirror;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject playerPrefab;  // プレイヤーのプレハブ
    public Transform spawnPoint;     // スポーン位置

    void Start()
    {
        // クライアントがゲーム開始時にプレイヤーをスポーンする
        if (isLocalPlayer)
        {
            RequestSpawnPlayer();
        }
    }

    [Command]
    public void CmdSpawnPlayer()
    {
        // `spawnPoint` が `null` ならデフォルト位置を使用
        Vector3 spawnPosition = (spawnPoint != null) ? spawnPoint.position : Vector3.zero;
        Quaternion spawnRotation = (spawnPoint != null) ? spawnPoint.rotation : Quaternion.identity;

        // `playerPrefab` が `null` ならエラーを防ぐ
        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab が設定されていません！");
            return;
        }

        // プレイヤーのインスタンスを生成
        GameObject playerObject = Instantiate(playerPrefab, spawnPosition, spawnRotation);

        // `NetworkManager` の `Spawnable Prefabs` に登録されていることを確認
        if (!NetworkClient.prefabs.ContainsValue(playerPrefab))
        {
            Debug.LogError("プレイヤープレハブが NetworkManager の Spawnable Prefabs に登録されていません！");
            return;
        }

        // サーバーでプレイヤーをスポーン
        NetworkServer.Spawn(playerObject);
    }

    public void RequestSpawnPlayer()
    {
        if (isLocalPlayer)
        {
            CmdSpawnPlayer();
        }
    }
}
