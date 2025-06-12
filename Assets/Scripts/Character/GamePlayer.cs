using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor.Build;

public class GamePlayer : MonoBehaviourPun
{
    /* 플레이어는 A, D키로 카드의 위치를 변경할 수 있고 
    카드를 선택할 수 있고 가장 왼쪽에 있는 카드가 초기값으로 설정된다.현재 카드 포커스는 카드의 테두리를
    하얀색으로 칠한다.
    선택된 카드에서 스페이스바를 누를 경우 카드가 선택되며 최대 3개의 카드를 선택할 수 있다.
    e키를 누를 경우 카드를 테이블에 내게된다. 
    X키를 누를 경우 이전 플레이어에게 라이어 선언을 한다. 
    마우스를 이용해서 고개를 도리도리할 수 있다. */
    
    // 플레이어의 개인카드 
    List<CardType> cardSet;
    // 총알매니저가 총알을 할당해줄것임.
    // List<Bullet> bullets;
    List<int> cardSelected;
    private int cardFocused = 0;
    private int cardSelectCount = 0;
    private int cardSelectMaxNum = 3;
    private int BulletCount { get; set; } // Faker의 경우 bullet을 하나 더가짐.

    private void Update()
    {
        // 모든 사람은 도리도리가 가능해야 한다.
        //if(마우스 도리도리){ }

        if (IsMyTurn()) // TurnManager에서 구한 액터넘버와 나 자신의 localPlayer의 액터넘버를 비교해서 같다면 자신의 차례
        {
            if (Input.GetKeyDown(KeyCode.X)) // 라이어 선언 키 (RPC O)
            {
                GamePlayerManager.instance.DeclareLiar();
            }
            if (Input.GetKeyDown(KeyCode.A)) // 카드의 왼쪽으로 가는 키 (RPC X)
            {
                if (cardSet.Count == 0) return;
                // 0번째에서 더 왼쪽으로는 못가게.
                if (cardFocused > 0)
                {
                    --cardFocused;
                }
                int idx = cardFocused;
                cardHighlights_outline(); // 포커스 중인 카드를 하이라이트 처리
            }
            if (Input.GetKeyDown(KeyCode.D)) // 카드의 오른쪽으로 가는 키 (RPC X)
            {
                if (cardSet.Count == 0) return;
                // ex) 5장이 있다면 5장 이후로 오른쪽으로 못가게
                if (cardFocused < cardSet.Count - 1)
                {
                    ++cardFocused;
                }
                int idx = cardFocused;
                cardHighlights_outline();
            }
            if (Input.GetKeyDown(KeyCode.E)) // 카드 테이블 등록 키 (RPC O)
            {
                // 선택된 카드가 없었다면 return
                if (cardSelected.Count == 0) return;

                // 내가 선택한 카드리스트를 만듦.
                List<CardType> cards = new List<CardType>();
                for(int i = 0; i < cardSelected.Count; ++i)
                {
                    cards[i] = cardSet[cardSelected[i]];

                }
                int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                GamePlayerManager.instance.EnrollCard(cards, actorNumber); // RPC 처리함.
                TurnManager.instance.NextTurn();  // RPC처리함.
            }
            if (Input.GetKeyDown(KeyCode.Space)) // 카드를 선택 혹은 취소 키 (RPC X)
            {
                bool isSelected = false;
                int selectedIdx = -1;
                // 현재 포커스된 카드가 선택되어 있는 카드인지 검사.
                for (int i = 0; i < cardSelected.Count; ++i)
                {
                    if (cardSelected[i] == cardFocused)
                    {
                        isSelected = true;
                        selectedIdx = i;
                        break;
                    }
                }
                if (isSelected) // 선택되어있는 카드라면 선택을 해제.
                {
                    cardSelected.RemoveAt(selectedIdx);
                    --cardSelectCount;
                    cardSelectHighlights();
                }
                else
                {
                    // 3장까지만 선택가능
                    if (cardSelectCount >= cardSelectMaxNum) return;
                    // 현재 포커스된 카드를 올림
                    cardSelected[cardSelectCount] = cardFocused;
                    ++cardSelectCount;
                    cardSelectHighlights();
                }
            }
        }
        else
        {
            // 밑의 모든 동작 RPC 필요
            // 내 차례가 아니더라도 패를 보기가 가능
            if (Input.GetKeyDown(KeyCode.Space)) 
            {

            }
            // 감정표현 1,2,3
            if (Input.GetKeyDown(KeyCode.Keypad1)) 
            {

            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {

            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {

            }
        }
    }

    

    private bool IsMyTurn() // RPC X
    {
        // 네트워크에 따라 턴 판별
        return PhotonNetwork.LocalPlayer.ActorNumber == TurnManager.instance.GetCurrentPlayerTurnActorNumber();
    }

    private void cardHighlights_outline() // RPC X
    {
        //cardSet[cardFocused]을 테두리만 하이라이트처리
    }

    private void cardSelectHighlights() // RPC X
    {
        // cardSet[cardFocused]를 하이라이트처리
        // 카드의 하이라이트를 끄기도 해야함. (스페이스 2번)
    }

}