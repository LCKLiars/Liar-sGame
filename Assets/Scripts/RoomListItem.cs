using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    public TMP_Text nameText;
    public Button joinButton;

    private string roomName;

    public void Setup(string name, int playerCount, int maxPlayers, System.Action onJoin)
    {
        roomName = name;
        nameText.text = $"{name} ({playerCount}/{maxPlayers})";
        joinButton.onClick.RemoveAllListeners();
        joinButton.onClick.AddListener(() => onJoin());
    }
}
