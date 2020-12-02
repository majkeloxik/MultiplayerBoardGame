﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
[CreateAssetMenu(fileName ="Action", menuName ="Actions/add actionInfo", order = 1)]
public class ActionInfo : ScriptableObject
{
    public int id;
    public string type;
    public string title;
    public string description;
    //What will be changed / value
    public List <ChangeValue> changeValue = new List<ChangeValue>();
    public List<string> attribute;
    public List<int> value; 
    public List<string> integrateWith;
    public Sprite artBackground;

    public class ChangeValue
    {
        public int value;
        public string attribute;
    }
}
