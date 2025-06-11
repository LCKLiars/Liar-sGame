using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DeckManager : MonoBehaviour
{
    [SerializeField] GameObject kingCard;
    [SerializeField] GameObject queenCard;
    [SerializeField] GameObject aceCard;
    [SerializeField] GameObject jokerCard;

    List<CardType> cardSet;

    int maxCardCount = 6;
    int maxJokerCardCount = 2;
    public List<CardType> CreateCardSet()
    {
        CreateCard();
        Shuffles();
        return cardSet;
    }
    private void CreateCard()
    {
        for(int i = 0; i < maxCardCount; ++i)
        {
            cardSet.Add(CardType.KING);
        }
        for(int i = 0; i < maxCardCount; ++i)
        {
            cardSet.Add(CardType.QUEEN);
        }
        for(int i = 0; i < maxCardCount; ++i)
        {
            cardSet.Add(CardType.ACE);
        }
        for(int i = 0; i < maxJokerCardCount; ++i)
        {
            cardSet.Add(CardType.JOKER);
        }
    }
    private void Shuffles()
    {
        for(int i = 0; i < cardSet.Count; ++i)
        {
            int randIdx = Random.Range(i, cardSet.Count);

            CardType temp = cardSet[randIdx];
            cardSet[randIdx] = cardSet[i];
            cardSet[i] = temp;
        }
    }
}
