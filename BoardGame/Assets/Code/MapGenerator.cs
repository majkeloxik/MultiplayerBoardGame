using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    public int rows = 5;
    public int cols = 5;
    public int[][] mapArray;

    public GameObject field;
    public GameObject fields;
    public Random random = new Random();
    private void Start()
    {
        mapArray = new int[rows][];
        for (int i = 0; i < rows; i++)
        {
            mapArray[i] = new int[cols];
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int rand = random.Next(0,2);
                mapArray[i][j] = rand;
                Debug.Log(rand);
                if (mapArray[i][j] == 1)
                {
                    spawnField(i,j);
                }
            }
        }
    }
    public void spawnField(int i, int j)
    {
        Instantiate(field, new Vector3(j*(0.40f), i*(0.40f), 0), Quaternion.identity, fields.transform);
    }
}
