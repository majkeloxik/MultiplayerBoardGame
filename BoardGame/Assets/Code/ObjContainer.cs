using Code.Network;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjContainer : MonoBehaviour
{
    public string actualPlayer;
    public GameObject actualPlayerObj;


    public GameObject archerCharacter;
    public GameObject warriorCharacter;
    public GameObject mageCharacter;

    public GameObject whoMoveImage;
    public Text whoMoveText;
    public Text diceValue;
    public Button dice;

    public GameObject playerAction;
    //start positions
    public GameObject[] startFields;
    public Material mainFieldMaterial;
    private MapGenerator mapGenerator;
    //fields id where we can move
    public int[] activeFields;

    public List <GameObject> playersList;
    public List<GameObject> fieldsList;
    public int whereMove;
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
        whoMoveText.text = player + " move now";
    }

    public void SetDiceValue(string value)
    {
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
                fieldsList[indexOfPossField].GetComponent<Renderer>().material.color = Color.yellow;
            }

        }
    }
    public void PlayerEndTurn()
    {
        foreach(var field in fieldsList)
        {
            field.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
    public void SelectPossibleMoves(int[] possMoves)
    {

    }
}