﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
    [Header("Rooms")]
    public GameObject roomObject;
    public GameObject roomObjectMaster;
    public GameObject roomsListUI;
    public GameObject lobbyRoomUI;
    [Header("Create room")]
    public GameObject roomCreateUI;
    public GameObject lobbyRoomMasterUI;
    public RectTransform scrollContainer;
    public InputField roomSize;
    public InputField roomName;

    [Header("Login/Register")]
    public GameObject loginError;    
    public GameObject loginUI;
    public GameObject registerSucc;
    public GameObject usernameExist;
    [Header("MainMenu")]
    public GameObject mainMenuUI;
    [Header("Lobby buttons")]
    public GameObject startButton;
    public GameObject readyButton;
    [Header("Players list")]
    public PlayerListHandler playerListHandler;
    public PlayerListHandler masterListHandler;
}
