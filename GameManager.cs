using UnityEngine;

// 메인로비와, 게임에서 사람을 구하는 로비, 인게임
public enum GameState
{
    MAINMENU,
    LOBBY,
    INGAME
}
public class GameManager : MonoBehaviour
{
    [SerializeField] MainMenuManager mainMenuManager;
    [SerializeField] LobbyManager lobbyManager;
    [SerializeField] InGameManager inGameManager;   

    public static GameManager instance;

    public GameState curState { get; private set; }

    private void Awake()
    {
        // instance는 1개만 존재
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(GameState _newState)
    {
        curState = _newState;
        // Hide와 Show기능을 묶어버리든가 해야함.
        switch (curState)
        {
            case GameState.MAINMENU:
                
                break;

            case GameState.LOBBY:
                
                break;

            case GameState.INGAME:
                
                break;
        }
    }


}
