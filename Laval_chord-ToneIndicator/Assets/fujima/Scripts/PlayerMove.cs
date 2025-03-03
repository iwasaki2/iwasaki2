using UnityEngine;
using Unity.Netcode;

public class PlayerMove : NetworkBehaviour
{
    private void Update()
    {
        // ローカルプレイヤーのみが動かせるようにする
        if (!IsOwner) return;

        float x = Input.GetAxis("Horizontal") * 5f * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * 5f * Time.deltaTime;

        transform.Translate(x, 0, z);
    }

    public override void OnNetworkSpawn()
    {
        // ローカルプレイヤーの色を赤にする
        if (IsOwner)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
