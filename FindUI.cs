using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
public class FindUI : MonoBehaviour
{
    [SerializeField] GameObject notifyPanel = null;
    [SerializeField] TMP_Text errorText = null;
    TMP_InputField nameText;
    TMP_InputField idText;
    TMP_Dropdown birthYear;
    TMP_Dropdown birthMonth;
    TMP_Dropdown birthDay;

    string FindUri = "http://127.0.0.1/findpw.php";

    int startYear;

    private void OnEnable()
    {
        InitValue();
        BirthSetting();
    }
    private void InitValue()
    {
        nameText = GetComponentsInChildren<TMP_InputField>()[0];
        idText = GetComponentsInChildren<TMP_InputField>()[1];
        birthYear = GetComponentsInChildren<TMP_Dropdown>()[0];
        birthMonth = GetComponentsInChildren<TMP_Dropdown>()[1];
        birthDay = GetComponentsInChildren<TMP_Dropdown>()[2];
        idText.text = "";
        nameText.text = "";
    }
    private void BirthSetting()
    {
        startYear = System.DateTime.Now.Year - 100;
        int endYear = System.DateTime.Now.Year;
        List<string> list = new List<string>();
        for (int i = startYear; i <= endYear; ++i)
        {
            list.Add(i.ToString());
        }
        birthYear.AddOptions(list);
        birthYear.value = endYear - startYear - 20;
        list.Clear();
        for (int i = 1; i <= 12; ++i)
        {
            list.Add(i.ToString());
        }
        birthMonth.AddOptions(list);
        birthMonth.value = 0;
        list.Clear();
        for (int i = 1; i <= 31; ++i)
        {
            list.Add(i.ToString());
        }
        birthDay.AddOptions(list);
        birthDay.value = 0;
        list.Clear();
    }

    public void OnClickFindBtn()
    {
        StartCoroutine(FindCoroutine());
    }
    private IEnumerator FindCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("Name", nameText.text);
        form.AddField("ID", idText.text);
        form.AddField("Birth", (birthYear.value + startYear) + "-" + (birthMonth.value + 1) + "-" + (birthDay.value + 1));
        using (UnityWebRequest www = UnityWebRequest.Post(FindUri, form))
        {
            yield return www.SendWebRequest();
            string response = www.downloadHandler.text;
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error");
            }
            else if(response == "no")
            {
                errorText.text = "The information you entered is incorrect.";
                errorText.color = Color.red;
            }
            else
            {
                Debug.Log(response);
                notifyPanel.GetComponent<NotifyPwUI>().SetPassword(response);
                notifyPanel.SetActive(true);
                gameObject.SetActive(false);
            }
        }

    }
    public void OnClickExitBtn()
    {
        gameObject.SetActive(false);
    }
}
