using UnityEngine;
using Mirror;

public class ObjectSpawner : NetworkBehaviour
{
    public GameObject objectPrefab; // スポーンするオブジェクトのPrefab
    public Transform spawnPoint;    // スポーン位置

    [Server]
    public void SpawnObject()
    {
        if (objectPrefab == null || spawnPoint == null)
        {
            Debug.LogError("SpawnObject() failed: objectPrefab または spawnPoint が設定されていません");
            return;
        }

        GameObject obj = Instantiate(objectPrefab, spawnPoint.position, Quaternion.identity);
        NetworkServer.Spawn(obj);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        SpawnObject();
    }
}
