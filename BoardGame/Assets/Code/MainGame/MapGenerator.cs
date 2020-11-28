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
    public int[][] mapArray;

    
    public GameObject field;
    //Parent of fields
    public GameObject fields;
    public List<GameObject> fieldsList;
    public List<GameObject> possibleFields;

    public Random random = new Random();

    public void GenerateMap(int[][] map)
    {
        int value;
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                if(map[i][j] != 0)
                {
                    value = map[i][j];
                    SpawnField(i, j, value);
                }
            }
        }
    }
    public void SpawnField(int i, int j, int value)
    {
        GameObject newField; 
        newField = Instantiate(field, new Vector3(j*(1.1f), 0, i*(1.1f)), Quaternion.identity, fields.transform);
        newField.name = "Field_" + value;
        newField.GetComponent<FieldInfo>().id = value;

        newField.GetComponent<Renderer>().material.color = Color.yellow;
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
                int rand = random.Next(0, 2);


                if (rand == 1)
                {
                    mapArray[i][j] = arrayIndex;
                    arrayIndex++;
                }
            }
        }
        Debug.Log(mapArray.ToString());
        GenerateMap(mapArray);
    }

}