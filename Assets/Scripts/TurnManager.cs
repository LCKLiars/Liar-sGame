using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public class TurnManager : MonoBehaviourPunCallbacks
{
    public static TurnManager instance;
    List<Player> players;
    public int currentPlayerTurnActorNumber;
    public int curTurn;
    public int defeatPlayer = 0;
    public bool isMyTurn
    {
        get
        {
            return PhotonNetwork.LocalPlayer.ActorNumber == currentPlayerTurnActorNumber;
        }
    }
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
    public void NextTurn() 
    {
        // 사람이 1명이라면 (나가서 진행이 안됨)
        if (PhotonNetwork.PlayerList.Length < 2)
        {
            return;
        }

        curTurn = ++curTurn % PhotonNetwork.PlayerList.Length;
        currentPlayerTurnActorNumber = PhotonNetwork.PlayerList[curTurn].ActorNumber;
    }

    public void StartFirstTurn(Player _lastDefeatplayer)
    {
        // 죽은 사람부터 시작할 거기 때문에 수정필요
        // 우선 방장(0)부터
        if (_lastDefeatplayer == null)
        {
            defeatPlayer = 0;
        }
        // 만약 defeatPlayer가 죽었을 수도있음.. 문제가 되는지 생각해봐야함.
        // 게임 죽으면 defeatPlayer의 번호를 바꿔줘야함. 어디서?
        currentPlayerTurnActorNumber = PhotonNetwork.PlayerList[defeatPlayer].ActorNumber;
    }
}
