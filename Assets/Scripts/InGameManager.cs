using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using NUnit.Framework;

public enum CardType
{
    KING,
    QUEEN,
    ACE,
    JOKER
};
public class InGameManager : MonoBehaviour
{
    private List<CardType> cardDeck;

    public DeckManager deckManager;
    public Player lastDefeatPlayer = null; // 패배 플레이어

    // 게임이 시작하면 덱을 먼저 섞는다.
    public void StartGame()
    {
        // 카드를 만든 후 섞는다.
        cardDeck = deckManager.CreateCardSet();
        // 카드를 플레이어에게 분배한다.
        // NetworkManager가 플레이어들에게 cardDeck을 분배한다.
        // 첫턴을 시작한다. 
        TurnManager.instance.StartFirstTurn(lastDefeatPlayer);
    }


}
