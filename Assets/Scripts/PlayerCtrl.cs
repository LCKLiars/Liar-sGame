using Photon.Pun;
using UnityEngine;

public class PlayerCtrl : MonoBehaviourPun
{
    private void Start()
    {
        // 예시: 내 플레이어만 초록, 상대는 빨강(렌더러 1개 기준)
        var renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.color = photonView.IsMine ? Color.green : Color.red;
        }

        // 내 플레이어만 컨트롤/카메라 등 추가(실전 입력 로직 필요시)
        if (!photonView.IsMine)
        {
            // 입력, 카메라 등 비활성화
        }
    }

    // 예시: 나중에 입력/움직임 구현시 Update에서 photonView.IsMine 체크로 분기
}
