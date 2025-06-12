using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using Photon.Pun;
using System.Linq;
using Photon.Pun.Demo.Cockpit;

public enum CardType
{
    KING,
    QUEEN,
    ACE,
    JOKER
};
public class InGameManager : MonoBehaviourPun
{
    public static InGameManager instance;
    // 카드덱은 개인카드가아닌 처음 6,6,6,2
    private List<CardType> cardDeck;
    private DeckManager deckManager;
    private RuleManager ruleManager;
    bool isReady = false;

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

    // 게임이 시작하면 덱을 먼저 섞는다.
    public void StartGame()
    {

        // 마스터만 카드를 만든 후 섞는다. 
        if (PhotonNetwork.IsMasterClient)
        {
            cardDeck = deckManager.CreateCardSet();
            isReady = true;
            // 마스터를 제외한 사람들의 덱을 똑같이 만든다.
            photonView.RPC("SyncDeck", RpcTarget.OthersBuffered, cardDeck);
        }

        // 카드를 플레이어에게 분배한다.
        // NetworkManager가 플레이어들에게 cardDeck을 분배한다.
        // 첫턴을 시작한다. 
        // 여기서 StartCoroutine으로 시간을 끌어줌.
        StartCoroutine("WaitForCreateDeck");
    }
    [PunRPC]
    public void SyncDeck(List<CardType> syncedDeck)
    {
        cardDeck = syncedDeck;
        isReady = true;
    }
    private IEnumerator WaitForCreateDeck()
    {
        yield return new WaitUntil(() => isReady);
        TurnManager.instance.StartFirstTurn();
    }
    // lastDefeatPlayer 구해야함.


    
}