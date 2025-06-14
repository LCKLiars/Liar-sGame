using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class Player : MonoBehaviourPun
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

    

    private int cardFocused = 0;
    private int cardSelectMaxNum = 3;
    private int BulletCount { get; set; } // Faker의 경우 bullet을 하나 더가짐.
    private void Update()
    {
        //if(마우스 도리도리){ }

        if (IsMyTurn())
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                DeclareLiar();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {

            }
            if (Input.GetKeyDown(KeyCode.D))
            {

            }
            if (Input.GetKeyDown(KeyCode.E))
            {

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {

            }
        }
        else
        {
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
    private bool IsMyTurn()
    {
        // 네트워크에 따라 턴 판별
        return PhotonNetwork.LocalPlayer.ActorNumber == TurnManager.instance.currentPlayerTurnActorNumber;
    }
    
    private void cardHighlights()
    {
        //cardSet[cardFocused]을 하이라이트처리
    }

    private void DeclareLiar()
    {
        //if ()
        {

        }
        photonView.RPC("RPC_DeclareLiar", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RPC_DeclareLiar()
    {
        TurnManager.instance.NextTurn();
    }
}