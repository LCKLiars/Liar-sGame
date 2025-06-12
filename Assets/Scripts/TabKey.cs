using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class TabKey : MonoBehaviour
{
    [SerializeField] List<TMP_InputField> inputFields;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for(int i = 0;i<inputFields.Count; ++i)
            {
                if (inputFields[i].isFocused)
                {
                    int next = (i + 1) % inputFields.Count;
                    inputFields[next].ActivateInputField();
                    inputFields[next].Select();
                    break;
                }
            }
        }
    }
}
