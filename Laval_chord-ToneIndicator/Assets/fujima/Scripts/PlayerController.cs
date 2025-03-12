using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    public GameObject objectPrefab; // スポーンするオブジェクト

    private void Update()
    {
        if (!isOwned) return; // 自分のプレイヤーオブジェクトのみ処理

        if (Keyboard.current.sKey.wasPressedThisFrame) // Sキーでオブジェクトをスポーン
        {
            CmdSpawnObject();
        }
    }

    [Command]
    private void CmdSpawnObject()
    {
        if (objectPrefab == null)
        {
            Debug.LogError("CmdSpawnObject: objectPrefab が設定されていません！");
            return;
        }

        // すでにシーンにあるオブジェクト（ChordPlayer）を再生成しない
        if (objectPrefab.GetComponent<NetworkIdentity>() != null && objectPrefab.CompareTag("Player"))
        {
            Debug.LogError("プレイヤーオブジェクトを再生成しようとしています！これは許可されていません！");
            return;
        }

        Debug.Log("CmdSpawnObject() がサーバーで呼ばれた");

        // **新しいオブジェクトを生成（ただし、プレイヤーではない）**
        GameObject newObject = Instantiate(objectPrefab, transform.position + Vector3.forward * 2, Quaternion.identity);
        NetworkServer.Spawn(newObject); // ✅ サーバーにスポーン

        Debug.Log("新しいオブジェクトがスポーンされた");
    }
}