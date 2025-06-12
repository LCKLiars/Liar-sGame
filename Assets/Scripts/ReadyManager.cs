using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon;

public class ReadyManager : MonoBehaviourPunCallbacks
{
    public Button readyButton;
    public TextMeshProUGUI readyButtonText;

    private bool isReady = false;

    private void Start()
    {
        readyButton.onClick.AddListener(OnClickReadyButton);
        SetReady(false);
    }

    private void OnClickReadyButton()
    {
        isReady = !isReady;
        SetReady(isReady);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "IsReady", isReady } });
    }

    private void SetReady(bool ready)
    {
        readyButtonText.text = ready ? "준비 취소" : "레디";
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player target, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("IsReady"))
        {
            CheckAllPlayersReady();
        }
    }

    private void CheckAllPlayersReady()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") || !(bool)player.CustomProperties["IsReady"])
                return;
        }
        // 모두 레디면 게임 시작(여기서 GameManager.Instance.GameStart() 등 호출 가능)
        Debug.Log("모든 플레이어 준비 완료 - 게임 시작 가능!");
        readyButton.gameObject.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "IsReady", false } });
        SetReady(false);
        readyButton.gameObject.SetActive(true);
        CheckAllPlayersReady();
    }
}
