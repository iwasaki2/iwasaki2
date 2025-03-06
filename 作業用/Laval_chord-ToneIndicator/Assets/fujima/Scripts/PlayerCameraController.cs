using UnityEngine;
using Mirror;

public class PlayerCameraController : NetworkBehaviour
{
    public Camera playerCamera;

    void Start()
    {
        // 自分のプレイヤーならカメラを有効化、他のプレイヤーなら無効化
        if (!isLocalPlayer)
        {
            playerCamera.gameObject.SetActive(false);
        }
        else
        {
            playerCamera.gameObject.SetActive(true);
        }
    }
}
