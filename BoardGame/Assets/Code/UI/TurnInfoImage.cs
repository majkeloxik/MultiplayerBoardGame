using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInfoImage : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(OnEnableCoroutine());
    }
    IEnumerator OnEnableCoroutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
