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
    public List<GameObject> playerBackground;
    public List<GameObject> readyImages;
    public void setPlayerList(string[] players)
    {

        for (int i = 0; i < 4; i++)
        {
            playerList[i].text = "";
            playerList[i].gameObject.SetActive(false);
            playerBackground[i].SetActive(false);
            readyImages[i].SetActive(false);
        }

        for (int i = 0; i < players.Length; i++)
        {
            playerList[i].gameObject.SetActive(true);
            playerList[i].text = players[i];
            playerBackground[i].SetActive(true);
        }
    }
}


