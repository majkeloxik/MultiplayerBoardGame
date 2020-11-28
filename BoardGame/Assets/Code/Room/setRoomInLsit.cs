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

    private void Start()
    {
        networkClient = GameObject.Find("CodeNetworking").GetComponent<NetworkClient>();
    }
    public void SendRoomName()
    {
        networkClient.OnRoomConnect(roomName.text);
    }
}

