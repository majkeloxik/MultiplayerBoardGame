using Code.Network;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjContainer : MonoBehaviour
{
    public string username;


    [Header("Characters prefabs")]
    public GameObject archerCharacter;
    public GameObject warriorCharacter;
    public GameObject mageCharacter;
    
    [Header("Actual turn statistics")]
    public string actualPlayer;
    public GameObject actualPlayerObj;
    public GameObject whoMoveImage;
    public Text whoMoveText;
    public Text diceValue;
    public Button dice;
    public DiceSprites diceSprites;
    public Image diceBackground;
    private string diceSpriteValue;
    public int[] activeFields;
    public bool fieldsClickable;

    public GameObject playerAction;
    //start positions
    public GameObject[] startFields;
    public Material mainFieldMaterial;
    private MapGenerator mapGenerator;
    //fields id where we can move
    

    public List <GameObject> playersList;
    public List<GameObject> fieldsList;
    public int whereMove;
    private CameraController cameraController;
    public CameraController CameraController
    {
        get
        {
            return cameraController = (cameraController == null) ? FindObjectOfType<CameraController>() : cameraController;
        }
    }
    public MapGenerator MapGenerator
    {
        get
        {
            return mapGenerator = (mapGenerator == null) ? FindObjectOfType<MapGenerator>() : mapGenerator;
        }
    }
    public ActionController actionController;
    public ActionController ActionController
    {
        get
        {
            return actionController = (actionController == null) ? FindObjectOfType<ActionController>() : actionController;
        }
    }
    private NetworkClient networkClient;

    public NetworkClient NetworkClient
    {
        get
        {
            return networkClient = (networkClient == null) ? FindObjectOfType<NetworkClient>() : networkClient;
        }
    }

    public void SetWhoMove(string player)
    {
        actualPlayer = player;
        actualPlayerObj = playersList.Find(x => x.name == player);
        whoMoveText.text = player + "\n moves now";
    }

    public void SetDiceValue(string value)
    {
        diceBackground.sprite = diceSprites.dice[Int32.Parse(value) - 1];
        diceValue.text = value;
    }

    public void PlayerDraw()
    {
        NetworkClient.DiceValue();
    }
    //Set field color on red, if possiebled move is on that field
    public void PossibleMoves(bool value, int[] possFields)
    {
        if (value)
        {
            activeFields = possFields;
            foreach (var i in possFields)
            {
                int indexOfPossField = fieldsList.FindIndex(t => t.GetComponent<FieldInfo>().id == i);
                fieldsList[indexOfPossField].GetComponent<Renderer>().material.color = Color.red;
            }
            fieldsClickable = true;
        }
    }
    public void PlayerMovedTo(int id)
    {
        foreach (var i in activeFields)
        {
            if(i == id)
            {
                int indexOfPossField = MapGenerator.fieldsList.FindIndex(t => t.GetComponent<FieldInfo>().id == i);
                fieldsList[indexOfPossField].GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                int indexOfPossField = MapGenerator.fieldsList.FindIndex(t => t.GetComponent<FieldInfo>().id == i);
                fieldsList[indexOfPossField].GetComponent<Renderer>().material.color = mainFieldMaterial.color;
            }

        }
    }
    public void PlayerEndTurn()
    {
        foreach(var field in fieldsList)
        {
            field.GetComponent<Renderer>().material.color = mainFieldMaterial.color;
        }
    }
    public void SelectPossibleMoves(int[] possMoves)
    {

    }
    private void Awake()
    {
        username = NetworkClient.username;
    }
    public void SetLevelValue(int playerIndex)
    {
        var playerController = playersList[playerIndex].GetComponent<PlayerController>();

        if(playerController.playerProperties.properties.exp >= playerController.maxExp)
        {
            playerController.level++;
            playerController.maxExp = (playerController.maxExp * 2) + playerController.increaseMaxExp;
        }
    }
}