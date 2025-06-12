using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{
    [SerializeField] TMP_InputField idText = null;
    [SerializeField] TMP_InputField pwText = null;
    [SerializeField] GameObject JoinPanel = null;
    [SerializeField] GameObject FindPanel = null;

    List<TMP_InputField> inputFields;

    string loginUri = "http://127.0.0.1/login.php";

    public void OnClickLoginBtn()
    {
        string id = idText.text;
        string pw = pwText.text;
        StartCoroutine(LoginCoroutine(id, pw));
    }

    public IEnumerator LoginCoroutine(string _id, string _pw)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginId", _id);
        form.AddField("loginPw", _pw);
        using (UnityWebRequest www = UnityWebRequest.Post(loginUri, form))
        {
            yield return www.SendWebRequest();
            string response = www.downloadHandler.text;
            Debug.Log(response);
            if(www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else if(response == "success")
            {
                Debug.Log("Welcome");
                
                
                
                SceneManager.LoadScene("GameScene");
            }
            else if(response == "password")
            {
                Debug.Log("Password Failed");
            }
            else if(response == "id")
            {
                Debug.Log("ID Failed");
            }
        }
    }

    public void OnClickJoinBtn()
    {
        JoinPanel.SetActive(true);
    }

    public void OnClickFindBtn()
    {
        FindPanel.SetActive(true);
    }
    
}
