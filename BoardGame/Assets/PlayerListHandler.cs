﻿using Project.Neetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListHandler : MonoBehaviour
{
    public Text player1;
    public Text player2;
    public Text player3;
    public Text player4;
    public List<Text> playerList;
    public void setPlayerList(string[] players)
    {
        Debug.Log("player: " +players[0] + " length: " + players.Length);
        for (int i = 0; i < 4; i++)
        {
            playerList[i].text = "";
        }

        for (int i = 0; i < players.Length; i++)
        {
            playerList[i].text = players[i];
        }
    }
}


