using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerManager : MonoBehaviourPun
{
    public static GamePlayerManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnrollCard(List<CardType> _cards, int _actorNumber) // 이 행동으로 모든 사람이 테이블에 어떤 카드가 최근에 올라왔고 누가냈는지 알 수 있음. RPC O
    {
        photonView.RPC("EnrollCard_RPC", RpcTarget.AllBuffered, _cards, _actorNumber);
    }
    [PunRPC]
    public void EnrollCard_RPC(List<CardType> _cards, int _actorNumber)
    {
        TableManager.instance.EnrollCard(_cards, _actorNumber);
    }

    public void DeclareLiar() // RPC O 
    {
        int myActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        int liarTargetActorNumber = TurnManager.instance.GetPreviousPlayerActorNumber();

        // 이전 사람이 없을 경우 (첫 시작일 경우) 
        if (liarTargetActorNumber == myActorNumber) return;

        List<CardType> cards = TableManager.instance.GetLastPlayerCard();
        bool isLiar = false;
        for (int i = 0; i < cards.Count; ++i)
        {
            if (cards[i] != TableManager.instance.CurTableCard)
            {
                isLiar = true;
                break;
            }
        }
        int liarActorNumber = isLiar ? myActorNumber : liarTargetActorNumber;
        // 모든사람에게 라이어 RPC
        
        photonView.RPC("RPC_DeclareLiar", RpcTarget.AllBuffered, liarActorNumber);
    }
    [PunRPC]
    public void RPC_DeclareLiar(int _liarActorNumber)
    {
        // liarActorNumber에 해당되는 플레이어만 총으로 자신의 머리를 쏨.
        // 그 후 게임을 새로 시작함. InGameManager.instance.StartGame();
    }
}
