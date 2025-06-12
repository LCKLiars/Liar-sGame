using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System;
public class JoinUI : MonoBehaviour
{
    #region
    [SerializeField] TMP_InputField idText = null;
    [SerializeField] TMP_Text idChkText = null;
    [SerializeField] TMP_InputField pwText = null;
    [SerializeField] TMP_InputField pw2Text = null;
    [SerializeField] TMP_Text pwChkText = null;
    [SerializeField] TMP_Text pw2ChkText = null;
    [SerializeField] TMP_InputField nameText = null;
    [SerializeField] TMP_Dropdown birthYear = null;
    [SerializeField] TMP_Dropdown birthMonth = null;
    [SerializeField] TMP_Dropdown birthDay = null;
    [SerializeField] Toggle genderToggle = null;
    [SerializeField] Button pwHideBtn = null;
    [SerializeField] TMP_Text signUpText = null;
    [SerializeField] GameObject SuccessPanel = null;
    #endregion

    string idChkUri = "http://127.0.0.1/idAvailableCheck.php";
    string joinUri = "http://127.0.0.1/join.php";
    bool idChk = false;
    bool pwChk = false;
    bool pw2Chk = false;

    int startYear;
    string lastID = "";

    private void OnEnable()
    {
        BirthSetting();
        InitValue();
    }
    private void InitValue()
    {
        idText.text = "";
        idChkText.text = "";
        pwText.text = "";
        pwChkText.text = "Password must be 8–12 characters, letters, numbers and must include at least one special character.";
        pwChkText.color = Color.white;
        pw2Text.text = "";
        pw2ChkText.text = "";
        nameText.text = "";
        genderToggle.isOn = true;
        signUpText.text = "";
        InitPWMode();
    }
    private void Update()
    {
        if(idText.text != lastID)
        {
            idChkText.text = "";
            idChk = false;
        }
        if (pwText.text == "")
        {
            pwChkText.color = Color.white;
            pwChk = false;
        }
        else
        {
            PasswordPatternChk();
        }

        if(pw2Text.text == "")
        {
            pw2Chk = false;
        }
        else
        {
            PasswordConfirmChk();
        }
    }

    private void PasswordPatternChk()
    {
        string pattern = "^(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,12}$";
        if (!Regex.IsMatch(pwText.text, pattern))
        {
            pwChkText.color = Color.red;
            pwChk = false;
        }
        else
        {
            pwChkText.color = Color.green;
            pwChk = true;
        }
    }
    private void PasswordConfirmChk()
    {
        if(pwText.text == pw2Text.text)
        {
            pw2ChkText.text = "";
            pw2Chk = true;
        }
        else
        {
            pw2ChkText.text = "Password does not match.";
            pw2ChkText.color = Color.red;
            pw2Chk = false;
        }
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

    public void OnClickPWHideBtn()
    {
        PWChangeMode();
    }

    private void PWChangeMode()
    {
        if (pwHideBtn.image.sprite.name == "hideIcon")
        {
            Sprite seekIcon = Resources.Load<Sprite>("Icon/seekIcon");
            pwHideBtn.image.sprite = seekIcon;
            pwText.contentType = TMP_InputField.ContentType.Standard;
            pw2Text.contentType = TMP_InputField.ContentType.Standard;

        }
        else
        {
            InitPWMode();
        }
        pwText.ForceLabelUpdate();
        pw2Text.ForceLabelUpdate();
    }
    private void InitPWMode()
    {
        Sprite hideIcon = Resources.Load<Sprite>("Icon/hideIcon");
        pwText.contentType = TMP_InputField.ContentType.Password;
        pw2Text.contentType = TMP_InputField.ContentType.Password;
        pwHideBtn.image.sprite = hideIcon;
    }

    public void OnClickIDChkBtn()
    {
        IDAvailableCheck();
    }
    private void IDAvailableCheck()
    {
        string id = idText.text;
        lastID = idText.text;
        // 패턴 체크에 통과했다면 중복성 검사
        if (IdPatternChk(id)) StartCoroutine(IdChkCoroutine(id));
    }
    private bool IdPatternChk(string _id)
    {
        string pattern = "^[a-zA-Z0-9]{8,12}$";

        if (!Regex.IsMatch(_id, pattern))
        {
            idChkText.text = "ID must be 8–12 characters, letters and numbers only";
            idChkText.color = Color.red;
            idChk = false;
            return false;
        }
        return true;
    }

    public IEnumerator IdChkCoroutine(string _id)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", _id);
        using (UnityWebRequest www = UnityWebRequest.Post(idChkUri, form))
        {
            yield return www.SendWebRequest();
            string response = www.downloadHandler.text;
            Debug.Log(response);
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error");
            }
            else if (response == "ok")
            {
                idChkText.text = "Available";
                idChkText.color = Color.green;
                idChk = true;
            }
            else if (response == "no")
            {
                idChkText.text = "This ID is already in use.";
                idChkText.color = Color.red;
                idChk = false;
            }
        }
    }

    public void OnClickExitBtn()
    {
        gameObject.SetActive(false);
    }

    public void OnClickSignUpBtn()
    {
        if (!idChk)
        {
            SignUpFailed_IDMessage();
            return;
        }
        if (!pwChk)
        {
            SignUpFailed_PwMessage1();
            return;
        }
        if (!pw2Chk)
        {
            SignUpFailed_PwMessage2();
            return;
        }
        if (!NameChk())
        {
            SignUpFailed_NameMessage();
            return;
        }
        // 모든 조건이 통과됐다면
        StartCoroutine(JoinCoroutine());
    }
    
    public IEnumerator JoinCoroutine()
    {
        string id = idText.text;
        string pw = pwText.text;
        string name = nameText.text;
        string birth = (birthYear.value+startYear) + "-" + (birthMonth.value+1) + "-" + (birthDay.value+1);
        int gender = 1;
        if(genderToggle.isOn == true) gender = 1;
        else gender = 2;
        
        WWWForm form = new WWWForm();
        form.AddField("JoinID", id);
        form.AddField("JoinPw", pw);
        form.AddField("JoinName", name);
        form.AddField("JoinBirth", birth);
        form.AddField("JoinGender", gender);
        using (UnityWebRequest www = UnityWebRequest.Post(joinUri, form))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error");
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                ShowSuccessMessage();
            }
        }
    }

    private void SignUpFailed_IDMessage()
    {
        signUpText.text = "Please check your ID";
        signUpText.color = Color.red;
    }
    private void SignUpFailed_PwMessage1()
    {
        signUpText.text = "Please check your Password";
        signUpText.color = Color.red;
    }
    private void SignUpFailed_PwMessage2()
    {
        signUpText.text = "Please confirm your password";
        signUpText.color = Color.red;
    }
    private bool NameChk()
    {
        if(nameText.text == "")
        {
            return false;
        }
        return true;
    }
    private void SignUpFailed_NameMessage()
    {
        signUpText.text = "Please check your name";
        signUpText.color = Color.red;
    }
    private void ShowSuccessMessage()
    {
        SuccessPanel.SetActive(true);
        gameObject.SetActive(false);
    }

}
