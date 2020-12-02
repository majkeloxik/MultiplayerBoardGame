using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInfoImage : MonoBehaviour
{
    public int TimeToWait;

    private void OnEnable()
    {
        StartCoroutine(OnEnableCoroutine());
    }
    IEnumerator OnEnableCoroutine()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
