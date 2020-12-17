using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterTimer : MonoBehaviour
{
    [SerializeField]
    private Text timeValue;
    private bool isRunning;
    private int TimeToWait = 10;
    DateTime pausedTime; 
    void Start()
    {
        timeValue = gameObject.GetComponent<Text>();
        isRunning = true;
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
        isRunning = false;

    }
    private void OnApplicationPause(bool pause)
    {
        if(isRunning)
        {
            if(pause)
            {
                pausedTime = DateTime.Now;
            }
            else
            {
                TimeToWait -= (int)(DateTime.Now - pausedTime).TotalSeconds;
            }
        }
    }
}
