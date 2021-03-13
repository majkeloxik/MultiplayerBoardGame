using Code.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public PlayerController player;
    public string playerName;
    public Text playerNameText;
    private ObjContainer objContainer;
    public Text mana_text_progres;
    public Text hp_text_progres;
    public Text mana;
    public Text attack;
    public Text defence;
    public Text health;
    public Text level;
    public Text exp;
    public Image expProgres;
    
    private ObjContainer ObjContainer
    {
        get
        {
            return objContainer = (objContainer == null) ? FindObjectOfType<ObjContainer>() : objContainer;
        }
    }

    public void SetPlayerInfo(int playerIndex)
    {
        player = ObjContainer.playersList[playerIndex].GetComponent<PlayerController>();
        mana_text_progres.text = player.playerProperties.character.mana.ToString();
        hp_text_progres.text = player.playerProperties.character.health.ToString();
        mana.text = player.playerProperties.character.mana.ToString();
        attack.text = player.playerProperties.character.mana.ToString();
        defence.text = player.playerProperties.character.defence.ToString();
        health.text = player.playerProperties.character.health.ToString();
        level.text = "Level: " + player.level.ToString();
        exp.text = player.playerProperties.properties.exp.ToString();
        float amountNumber = (float)player.playerProperties.properties.exp / (float)player.maxExp;
        expProgres.fillAmount = amountNumber; 
    }
    private void Start()
    {
        expProgres.fillAmount = 0.5f;
    }
}
