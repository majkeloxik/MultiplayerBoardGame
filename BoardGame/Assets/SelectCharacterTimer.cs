using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterTimer : MonoBehaviour
{
    [SerializeField]
    private Text timeValue;

    private int TimeToWait = 10;
    void Start()
    {
        timeValue = gameObject.GetComponent<Text>();
        StartCoroutine(SelectTimer());
    }
    IEnumerator SelectTimer()
    {
        while(TimeToWait >= 0)
        {
            timeValue.text = TimeToWait.ToString();
            yield return new WaitForSeconds(1f);
            TimeToWait--;
        }

    }
}
