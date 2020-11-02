using System.Collections;
using System.Collections.Generic;
using Code.Network;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class setRoomInLsit : MonoBehaviour
{
    // Start is called before the first frame update

    public Text roomName;
    public Text roomSize;
    [FormerlySerializedAs("neetworkClient")] public NetworkClient networkClient;
    public string name;

    private void Start()
    {
        networkClient = GameObject.Find("CodeNeetworking").GetComponent<NetworkClient>();
    }
    public void SendRoomName()
    {
        name = roomName.text;
        networkClient.OnRoomConnect(name);
    }
}

