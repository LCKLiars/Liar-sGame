using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerInfoUI : MonoBehaviour
{
    public TMP_Text playerNameText;
    public TMP_Text readyStatusText;

    public void SetUp(Player player)
    {
        playerNameText.text = player.NickName;

        bool isReady = player.CustomProperties.ContainsKey("IsReady") &&
                       (bool)player.CustomProperties["IsReady"];

        readyStatusText.text = isReady ? " Ready" : " Not Ready";
    }
}
