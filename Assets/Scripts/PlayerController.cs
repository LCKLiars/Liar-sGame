using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public float speed = 5f;

    void Update()
    {
        // 내 플레이어만 조작 가능
        if (!photonView.IsMine) return;

        float h = Input.GetAxis("Horizontal"); // A/D or 좌/우
        float v = Input.GetAxis("Vertical");   // W/S or 상/하
        Vector3 dir = new Vector3(h, 0, v);
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }
}
