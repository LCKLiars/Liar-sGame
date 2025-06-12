using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomPlayerManager : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints;           // Inspector에서 배열로 연결
    public GameObject playerPrefab;           // Player 프리팹 Inspector 연결

    private bool hasSpawned = false;

    private void Start()
    {
        if (PhotonNetwork.InRoom && !hasSpawned)
            SpawnMyPlayer();
    }

    public override void OnJoinedRoom()
    {
        if (!hasSpawned)
            SpawnMyPlayer();
    }

    private void SpawnMyPlayer()
    {
        if (hasSpawned) return;

        int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPoint = spawnPoints[Mathf.Clamp(index, 0, spawnPoints.Length - 1)];
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
        hasSpawned = true;
    }

    // "Back" 또는 "Leave" 버튼에 연결
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
