using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject createPanel;
    public GameObject roomListPanel;

    [Header("Room Create")]
    public TMP_InputField roomNameInput;
    public TMP_Dropdown lobbyTypeDropdown;
    public int maxPlayers = 4;

    [Header("Room List")]
    public Transform contentParent; // 방 리스트 Content 오브젝트
    public GameObject roomItemPrefab;
    public Button refreshButton;

    private Dictionary<string, GameObject> roomItems = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ShowMainPanel();
        if (refreshButton != null)
            refreshButton.onClick.AddListener(RequestRoomList);
    }

    public void OnLobbyEntered()
    {
        // 최초 로비 진입시 동작(예: 방 리스트 새로고침)
        RequestRoomList();
    }

    #region UI 패널 제어

    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        createPanel.SetActive(false);
        roomListPanel.SetActive(false);
    }
    public void ShowCreatePanel()
    {
        mainPanel.SetActive(false);
        createPanel.SetActive(true);
        roomListPanel.SetActive(false);
    }
    public void ShowRoomListPanel()
    {
        mainPanel.SetActive(false);
        createPanel.SetActive(false);
        roomListPanel.SetActive(true);
        RequestRoomList();
    }
    #endregion

    #region 방 생성/입장
    public void OnClickCreateRoom()
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;
        string roomName = roomNameInput.text.Trim();
        if (string.IsNullOrEmpty(roomName)) roomName = $"Room_{Random.Range(1000, 9999)}";
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = (byte)maxPlayers,
            IsOpen = true,
            IsVisible = true
        };
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    #endregion

    #region 방 리스트/Refresh
    public void RequestRoomList()
    {
        if (!PhotonNetwork.IsConnectedAndReady) return;
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("[RoomManager] JoinLobby (for refresh)");
            PhotonNetwork.JoinLobby();
        }
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var item in roomItems.Values)
            Destroy(item);
        roomItems.Clear();

        foreach (var info in roomList)
        {
            if (info.RemovedFromList) continue;
            var go = Instantiate(roomItemPrefab, contentParent);
            var item = go.GetComponent<RoomListItem>();
            item.Setup(info.Name, info.PlayerCount, info.MaxPlayers, () => JoinRoom(info.Name));
            roomItems.Add(info.Name, go);
        }
    }
    #endregion
}
