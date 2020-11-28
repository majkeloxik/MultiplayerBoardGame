using Code.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInfo : MonoBehaviour
{
    public int id;
    public GameObject[] slots;
    public int[] activeFields;


    private NetworkClient networkClient;
    public NetworkClient NetworkClient
    {
        get
        {
            return networkClient = (networkClient == null) ? FindObjectOfType<NetworkClient>() : networkClient;
        }
    }
    private ObjContainer objContainer;
    public ObjContainer ObjContainer
    {
        get
        {
            return objContainer = (objContainer == null) ? FindObjectOfType<ObjContainer>() : objContainer;
        }
    }


    //TODO: correct this on touch ? or sth
    public void OnMouseDown()
    {
        Debug.LogError("ASDASDSAS");
        if (Array.Exists(ObjContainer.activeFields, element => element == id))
        {

            Debug.Log("Jestem aktywny");
            NetworkClient.FieldSelected(id);
        }
        else
        {
            Debug.Log("Nie jestem aktywny");
        }
    }
}
