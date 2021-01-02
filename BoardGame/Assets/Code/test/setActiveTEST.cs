using System;
using Code.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class setActiveTEST : MonoBehaviour
{
    public Image proggressss;
    private void Start()
    {
        proggressss.fillAmount = 0.5f;
        Character character1 = new Character();
        ChangeValues changeValues = new ChangeValues();
        Properties properties = new Properties();
        changeValues.mana = 10;
        changeValues.exp = 50;
        properties.exp = 100;
        properties += changeValues;
        character1 += changeValues;

        Debug.Log(character1.mana);
    }
}