using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public class TurnManager : MonoBehaviourPunCallbacks
{
    public static TurnManager instance;
    List<GamePlayer> players;
    private int currentPlayerTurnActorNumber;
    private int previousPlayerTurnActorNumber;
    public int curTurn;
    public int defeatPlayer = 0;
    public bool isMyTurn
    {
        get
        {
            return PhotonNetwork.LocalPlayer.ActorNumber == currentPlayerTurnActorNumber;
        }
    }
    public int GetPreviousPlayerActorNumber()
    {
        return previousPlayerTurnActorNumber;
    }
    public int GetCurrentPlayerTurnActorNumber()
    {
        return currentPlayerTurnActorNumber;
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
        // 이전 플레이어의 턴 액터넘버를 저장한다.
        previousPlayerTurnActorNumber = currentPlayerTurnActorNumber;
        // 턴을 바꾼다.
        currentPlayerTurnActorNumber = PhotonNetwork.PlayerList[curTurn].ActorNumber;
        
    }

    public void StartFirstTurn(int _lastDefeatplayerActorNumber)
    {
        // 게임의 처음 턴
        if(_lastDefeatplayerActorNumber == -1)
        {
            // 1번째 게임 1번째 턴의 경우 null의 값이 들어가 있음. (방장 시작)
            previousPlayerTurnActorNumber = PhotonNetwork.PlayerList[0].ActorNumber;
            currentPlayerTurnActorNumber = PhotonNetwork.PlayerList[0].ActorNumber;
        }
        else
        {
            previousPlayerTurnActorNumber = _lastDefeatplayerActorNumber;
            currentPlayerTurnActorNumber = _lastDefeatplayerActorNumber;
        }
    }
}
