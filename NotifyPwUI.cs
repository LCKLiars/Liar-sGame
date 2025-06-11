using UnityEngine;
using TMPro;
public class NotifyPwUI : MonoBehaviour
{
    [SerializeField] TMP_Text passwordText;
    public void SetPassword(string password)
    {
        passwordText.text = "Password\n" + password;
    }
    public void OnClickExitBtn()
    {
        gameObject.SetActive(false);
    }
}
