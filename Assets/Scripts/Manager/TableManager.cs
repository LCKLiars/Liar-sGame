using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
public class TableManager : MonoBehaviour
{
    public static TableManager instance;

    List<CardType> lastPlayCard;
    int lastPlayerActorNumber = -1;
    public CardType CurTableCard;

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
    public List<CardType> GetLastPlayerCard()
    {
        return lastPlayCard;
    }
    public int GetLastPlayerActorNumber()
    {
        return lastPlayerActorNumber;
    }
    
    // 가장 최근에 낸 플레이어의 패를 저장
    public void EnrollCard(List<CardType> cards, int _actorNumber) // GamePlayerManager에서 RPC를 사용해서 호출.
    {
        lastPlayCard.Clear();

        lastPlayerActorNumber = _actorNumber;
        lastPlayCard = cards;
    }


    public void LiarCheck()
    {

    }
}
