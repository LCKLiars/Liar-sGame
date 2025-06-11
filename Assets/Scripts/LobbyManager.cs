using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{

    public void OnClickLeave()
    {
        // 네트워크적으로 처리할 것 처리 후 메인 씬
        //NetWorkManager.instance.aaaa()
    }
    public void OnClickNext()
    {
        //NetWorkManager.instance.aaaa()
        // 네트워크 처리 후 게임 씬
    }
    public void OnClickReady()
    {
        // 클라이언트만
    }

}
