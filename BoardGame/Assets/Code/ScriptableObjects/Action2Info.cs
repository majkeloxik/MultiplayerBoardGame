using System.Collections;
using System.Collections.Generic;

using UnityEngine;
[CreateAssetMenu(fileName = "Action", menuName = "Actions/add action2 Info", order = 1)]
public class Action2Info : ScriptableObject
{
    public int id;
    public string type;
    public string title;
    [TextArea]
    public string description;
    //What will be changed / value

    public List<string> attribute;
    public List<int> value;
    public List<string> integrateWith;
    public string correctAnswer;
    public string wrongAnswer;
    public Sprite artBackground;


}
