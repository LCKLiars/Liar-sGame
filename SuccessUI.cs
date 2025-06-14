using UnityEngine;
using System.Collections;
public class SuccessUI : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(InActiveCoroutine());
    }
    private IEnumerator InActiveCoroutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
