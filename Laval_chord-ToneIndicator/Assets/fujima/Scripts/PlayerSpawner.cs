using UnityEngine;
using Mirror;

public class PlayerSpawner : NetworkBehaviour
{
    // プレイヤーのプレハブをInspectorから設定
    public GameObject playerPrefab;  // プレイヤーのプレハブ
    public Transform spawnPoint;     // プレイヤーのスポーン位置

    // サーバー側でプレイヤーをスポーンするコマンド
    [Command]
    public void CmdSpawnPlayer()
    {
        // プレイヤーのプレハブをインスタンス化
        GameObject playerObject = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        // サーバーでスポーンさせる
        NetworkServer.Spawn(playerObject);
    }

    // クライアントからスポーンをリクエストするために呼び出される
    public void RequestSpawnPlayer()
    {
        if (isLocalPlayer)
        {
            CmdSpawnPlayer();  // クライアントからサーバーへプレイヤー生成リクエスト
        }
    }
}
