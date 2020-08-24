using Project.Neetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class setRoomInLsit : MonoBehaviour
{
    // Start is called before the first frame update

    public Text roomName;
    public Text roomSize;
    public NeetworkClient neetworkClient;
    public string name;

    private void Start()
    {
        neetworkClient = GameObject.Find("CodeNeetworking").GetComponent<NeetworkClient>();
    }
    public void SendRoomName()
    {
        name = roomName.text;
        neetworkClient.OnRoomConnect(name);
    }
}

