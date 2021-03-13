using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    private readonly int rows = 12;
    private readonly int cols = 12;
    private int maxHight;
    public int[][] mapArray;

    public GameObject LD; // LEFT DOWN , DOWN RIGHT
    public GameObject RU; // UP RIGHT, RIGHT UP
    public GameObject LU; //Up lEFT
    public GameObject RD; //RIGHT DOWN, 
    public GameObject horizontal;
    public GameObject Vertical;

    
    public GameObject field;
    public List<GameObject> randomBoardFields;
    //Parent of fields
    public GameObject fields;
    public List<GameObject> fieldsList;
    public List<GameObject> possibleFields;
    public Random random = new Random();
    [SerializeField]
    private ObjContainer objContainer;
    [Header("Test map generator")]
    public bool testMap = false;
    private ObjContainer ObjContainer
    {
        get
        {
            return objContainer = (objContainer == null) ? FindObjectOfType<ObjContainer>() : objContainer;
        }
    }
    private void Start()
    {
        if(testMap)
        {
            TestGenerator();
        }
    }
    public void GenerateMap(int[][] map)
    {
        int value;
        Dictionary<int, int> fieldsDictionary = new Dictionary<int, int>();
        fieldsDictionary.Add(0, 25);
        fieldsDictionary.Add(1, 25);
        fieldsDictionary.Add(2, 30);
        fieldsDictionary.Add(3, 104);

        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                if(map[i][j] != 0)
                {
                    value = map[i][j];
                    SpawnField(i, j, value);
                }
                else
                {
                    int index = random.Next(fieldsDictionary.Count);
                    var x = fieldsDictionary.ElementAt(index).Key;
                    fieldsDictionary[x]--;
                    SpawnRandomField(i, j, fieldsDictionary.ElementAt(index).Key);
                    if (fieldsDictionary[x] < 1)
                    {
                        fieldsDictionary.Remove(x);
                    }
                }
                
            }
        }
        ObjContainer.fieldsList = fieldsList;
    }
    public void SpawnRandomField(int i, int j, int value)
    {
        GameObject newField;
        newField = Instantiate(randomBoardFields[value], new Vector3(j * (1.0f), 0, i * (1.0f)), Quaternion.Euler(90f, 0, -90f), fields.transform);
    }
    public void SpawnField(int i, int j, int value)
    {
        GameObject newField; 
        newField = Instantiate(field, new Vector3(j*(1.0f),0, i*(1.0f)), Quaternion.Euler(-90f,0,0), fields.transform);
        newField.name = "Field_" + value;
        newField.GetComponent<FieldInfo>().id = value;

        //newField.GetComponent<Renderer>().material.color = Color.yellow;
        fieldsList.Add(newField);
    }
    public void TestGenerator()
    {
        mapArray = new int[rows][];
        int arrayIndex = 1;
        for (int i = 0; i < rows; i++)
        {
            mapArray[i] = new int[cols];
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if( i == 0 || j == 0 || i == rows - 1 || j == cols - 1)
                {
                    mapArray[i][j] = arrayIndex;
                    arrayIndex++;
                }
                /*int rand = random.Next(0, 2);


                if (rand == 1)
                {
                    mapArray[i][j] = arrayIndex;
                    arrayIndex++;
                }
            */
            }
        }
        GenerateMap(mapArray);
    }
}