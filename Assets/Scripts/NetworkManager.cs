using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    public TMP_InputField nicknameInput;
    public TMP_InputField roomNameInput;
    public Button createRoomButton;
    public Button quickJoinButton;
    public TMP_Text statusText;
    public TMP_Text playerListText;
    public TMP_Text roomInfoText;


    [Header("Player")]
    public string playerPrefabName = "PlayerPrefab"; // Resources/PlayerPrefab.prefab
    public Transform[] spawnPoints; // 4개의 시작 위치

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        statusText.text = "Connecting...";
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
        else
            PhotonNetwork.JoinLobby();

        if (createRoomButton != null)
            createRoomButton.onClick.AddListener(OnCreateRoomClicked);
        if (quickJoinButton != null)
            quickJoinButton.onClick.AddListener(OnQuickJoinClicked);
    }

    public override void OnConnectedToMaster()
    {
        statusText.text = "Connected! Joining lobby...";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        statusText.text = "Lobby joined. Enter nickname and room name!";
    }

    void OnCreateRoomClicked()
    {
        string nickname = string.IsNullOrWhiteSpace(nicknameInput.text) ? "Player" + Random.Range(1000, 9999) : nicknameInput.text;
        string roomName = string.IsNullOrWhiteSpace(roomNameInput.text) ? "Room" + Random.Range(1000, 9999) : roomNameInput.text;
        PhotonNetwork.NickName = nickname;
        var options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName, options);
        statusText.text = "Creating room...";
    }

    void OnQuickJoinClicked()
    {
        string nickname = string.IsNullOrWhiteSpace(nicknameInput.text) ? "Player" + Random.Range(1000, 9999) : nicknameInput.text;
        PhotonNetwork.NickName = nickname;
        PhotonNetwork.JoinRandomRoom();
        statusText.text = "Joining random room...";
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "Room joined! Players: " + PhotonNetwork.CurrentRoom.PlayerCount + "/4";
        UpdatePlayerList();
        UpdateRoomInfo();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        statusText.text = newPlayer.NickName + " joined.";
        UpdatePlayerList();

        int spawnIdx = PhotonNetwork.LocalPlayer.ActorNumber - 1; // 0~3
        Transform spawn = spawnPoints.Length > spawnIdx ? spawnPoints[spawnIdx] : null;
        Vector3 pos = spawn ? spawn.position : Vector3.zero;
        PhotonNetwork.Instantiate(playerPrefabName, pos, Quaternion.identity);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        statusText.text = otherPlayer.NickName + " left.";
        UpdatePlayerList();
    }

    void UpdatePlayerList()
    {
        if (playerListText == null) return;
        playerListText.text = "";
        foreach (var player in PhotonNetwork.PlayerList)
            playerListText.text += player.NickName + "\n";
    }

    void UpdateRoomInfo()
    {
        if (roomInfoText != null)
            roomInfoText.text = $"Room: {PhotonNetwork.CurrentRoom.Name}\nPlayers: {PhotonNetwork.CurrentRoom.PlayerCount}/4";
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        statusText.text = "Left room. Back to lobby!";
        playerListText.text = "";
        roomInfoText.text = "";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        statusText.text = $"Room create failed: {message}";
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = $"Room join failed: {message}";
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        statusText.text = "No room found. Creating new room...";
        OnCreateRoomClicked();
    }
}
