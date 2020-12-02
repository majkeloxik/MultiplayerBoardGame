using Code.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInfo : MonoBehaviour
{
    //class holding info about field, slots on fields, and set clickable field when is possible to move.
    public int id;
    public GameObject[] slots;


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

    //TODO: correct this on touch ? or sth AND after click disabled that option 
    public void OnMouseDown()
    {
        if (Array.Exists(ObjContainer.activeFields, element => element == id))
        {
            NetworkClient.FieldSelected(id);
        }
        else
        {

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //run action
        if(id == ObjContainer.whereMove)
        {
            if (other.name == ObjContainer.actualPlayer && NetworkClient.username == ObjContainer.actualPlayer)
            {
                ObjContainer.playerAction.SetActive(true);
                NetworkClient.PlayerAction();   
            }
            else
            {
                ObjContainer.playerAction.SetActive(true);
            }
        }
    }
}
