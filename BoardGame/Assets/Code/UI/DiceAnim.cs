using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceAnim : MonoBehaviour
{
    public DiceSprites diceSprites;
    public Image diceBackground;
    public int diceValue;
    
    public void AnimDiceValue()
    {
        for(int i = 0; i < 10; i++)
        {
            diceBackground.sprite = diceSprites.dice[Random.Range(0, 5)];
        }
    }


}