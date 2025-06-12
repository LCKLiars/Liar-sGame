using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("[PhotonManager] Connected to Master ¡æ JoinLobby()");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("[PhotonManager] Joined Lobby");
        RoomManager.Instance.OnLobbyEntered();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("[PhotonManager] Joined Room ¡æ Load RoomScene");
        PhotonNetwork.LoadLevel("RoomScene");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("[PhotonManager] Left Room ¡æ Load LobbyScene");
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public override void OnRoomListUpdate(System.Collections.Generic.List<RoomInfo> roomList)
    {
        RoomManager.Instance.OnRoomListUpdate(roomList);
    }
}
