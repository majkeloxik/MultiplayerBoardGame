using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
    public GameObject roomObject;
    public GameObject loginError;    
    public GameObject loginUI;
    public GameObject roomCreateUI;
    public GameObject lobbyRoomUI;
    public GameObject lobbyRoomMasterUI;
    public GameObject mainMenuUI;
    public GameObject roomsListUI;
    public GameObject registerSucc;
    public GameObject usernameExist;
    public RectTransform scrollContainer;
    public Canvas canvas;
    public Text roomSize;
    public Text roomName;

    //Scripts

    public PlayerListHandler playerListHandler;
    public PlayerListHandler masterListHandler;
}
