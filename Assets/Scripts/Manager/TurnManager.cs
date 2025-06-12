using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public class TurnManager : MonoBehaviourPunCallbacks
{
    public static TurnManager instance;
    
    private int currentPlayerTurnActorNumber;
    private int previousPlayerTurnActorNumber;
    public int curTurn = 0;
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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void NextTurn() // RPC O 
    {
        // 사람이 1명이라면 (나가서 진행이 안됨)
        if (PhotonNetwork.PlayerList.Length < 2)
        {
            return;
        }
        // 현재 플레이어가 몇번째 플레이어인지 확인하고 다음사람의 숫자를 구한다.
        int nextIdx = -1;
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; ++i)
        {
            if(currentPlayerTurnActorNumber == PhotonNetwork.PlayerList[i].ActorNumber)
            {
                nextIdx = ++i % PhotonNetwork.PlayerList.Length;
                break;
            }
        }
        photonView.RPC("RPC_NextTurn", RpcTarget.AllBuffered, nextIdx, PhotonNetwork.LocalPlayer.ActorNumber);
    }
    [PunRPC]
    public void RPC_NextTurn(int _idx,int _actorNumber)
    {
        // 이전 플레이어의 턴 액터넘버를 저장한다.
        previousPlayerTurnActorNumber = _actorNumber;
        // 모든 사람은 위에서 구한 idx번째 사람이 현재 턴으로 인식한다.
        currentPlayerTurnActorNumber = PhotonNetwork.PlayerList[_idx].ActorNumber;
    }
    public void StartFirstTurn() // 첫 턴은 모두가 같은 값을 가지기 때문에 RPC X
    {
        // 게임의 처음 턴
        if (TableManager.instance.GetLastPlayerActorNumber() == -1)
        {
            // 처음은 방장 시작
            previousPlayerTurnActorNumber = PhotonNetwork.MasterClient.ActorNumber;
            currentPlayerTurnActorNumber = PhotonNetwork.MasterClient.ActorNumber;
        }
        else
        {
            // 마지막 플레이어가 죽었을 경우 그 다음 순서로 시작하게 해야함.

            previousPlayerTurnActorNumber = TableManager.instance.GetLastPlayerActorNumber();
            currentPlayerTurnActorNumber = TableManager.instance.GetLastPlayerActorNumber();
        }
    }
}
// curTurn에서 사람이 죽으면 그 죽은것도 넘길 줄도 알아야함.  ActorNumber 
// ex) 1-2-3-4 -> 1-2-4 -> 1-4