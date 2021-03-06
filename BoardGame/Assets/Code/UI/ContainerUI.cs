﻿using Code.Network;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUI : MonoBehaviour
{
    [Header("Neetwork")]
    public GameObject neetworkObject;

    [Header("Rooms")]
    [SerializeField] private GameObject roomObject;

    [SerializeField] private GameObject roomObjectMaster;
    [SerializeField] private GameObject roomsListUI;
    [SerializeField] private GameObject lobbyRoomUI;

    [Header("Create room")]
    [SerializeField] private GameObject roomCreateUI;
    [SerializeField] private GameObject roomNameError;
    [SerializeField] private GameObject lobbyRoomMasterUI;
    [SerializeField] private RectTransform scrollContainer;
    [SerializeField] private InputField roomSize;
    [SerializeField] private InputField roomName;

    [Header("Login/Register")]
    public GameObject usernameLogin;
    public GameObject passwordLogin;
    [SerializeField] private GameObject loginError;
    public GameObject passwordValidError;
    public GameObject registerUI;
    [SerializeField] private GameObject loginUI;
    [SerializeField] private GameObject registerSucc;
    [SerializeField] private GameObject usernameExist;

    [Header("MainMenu")]
    [SerializeField] private GameObject mainMenuUI;

    [Header("Lobby buttons")]
    [SerializeField] private Button startButton;

    [SerializeField] private Button readyButton;

    [Header("Select character")]
    [SerializeField] private GameObject selectCharacter;
    [SerializeField] private GameObject selectCharacterPrefabs;
    [SerializeField] private Button characterSelected;
    [SerializeField] private Button warriorButton;
    [SerializeField] private Button archerButton;
    [SerializeField] private Button mageButton;
    [SerializeField] private GameObject allAccepted;
    [SerializeField] private GameObject notAllAccepted;

    [Header("Players list")]
    [SerializeField] private PlayerListHandler playerListHandler;
    [SerializeField] private PlayerListHandler masterListHandler;
    [SerializeField] private GameObject[] playersInRoomObj;

    public void SetInteractableButton(string buttonName, bool value)
    {
        switch (buttonName)
        {
            case "characterSelected":
                characterSelected.interactable = value;
                warriorButton.interactable = value;
                archerButton.interactable = value;
                mageButton.interactable = value;
                break;

            case "startGameButton":
                startButton.interactable = value;
                break;
        }
    }
    public void SetPlayerReadyInRoom(string playerReady, bool value)
    {
        int index = playerListHandler.playerList.FindIndex(tmp => tmp.GetComponent<Text>().text == playerReady);
        playerListHandler.readyImages[index].SetActive(value);
        masterListHandler.readyImages[index].SetActive(value);
    }
    public void SetActiveUI(string elementUI, bool value)
    {
        switch (elementUI)
        {
            // LOGIN / REGISTER
            case "signed":
                loginUI.SetActive(false);
                mainMenuUI.SetActive(true);
                break;

            case "usernameExist":
                loginError.SetActive(!value);
                usernameExist.SetActive(value);
                registerSucc.SetActive(!value);
                break;

            case "loginError":
                loginError.SetActive(value);
                usernameExist.SetActive(!value);
                registerSucc.SetActive(!value);
                break;

            case "registred":
                loginUI.SetActive(value);
                registerUI.SetActive(!value);
                loginError.SetActive(!value);
                usernameExist.SetActive(!value);
                registerSucc.SetActive(value);
                break;
            // ROOM HANDLING
            case "roomNameToShort":
                roomNameError.GetComponent<Text>().text = "room name is to short!";
                roomNameError.SetActive(true);
                break;
            case "roomNameExist":
                roomNameError.GetComponent<Text>().text = "this room name is busy";
                roomNameError.SetActive(true);
                break;
            case "createdRoom":
                roomNameError.SetActive(false);
                mainMenuUI.SetActive(!value);
                lobbyRoomMasterUI.SetActive(value);
                roomCreateUI.SetActive(!value);
                startButton.interactable = false;
                break;

            case "roomList":
                mainMenuUI.SetActive(!value);
                roomsListUI.SetActive(value);
                break;

            case "joinToRoom":
                lobbyRoomUI.SetActive(value);
                roomsListUI.SetActive(!value);
                break;

            case "kickedFromRoom":
                lobbyRoomUI.SetActive(!value);
                mainMenuUI.SetActive(value);
                break;

            case "deleteRoomMaster":
                lobbyRoomMasterUI.SetActive(false);
                mainMenuUI.SetActive(true);
                break;

            case "deleteRoomPlayer":
                lobbyRoomUI.SetActive(false);
                mainMenuUI.SetActive(true);
                break;

            case "leaveRoom":
                lobbyRoomUI.SetActive(false);
                mainMenuUI.SetActive(true);
                break;
            //SELECT CHARACTER
            case "selectCharacterMaster":
                lobbyRoomMasterUI.SetActive(false);
                selectCharacter.SetActive(true);
                selectCharacterPrefabs.SetActive(true);

                break;

            case "selectCharacterPlayer":
                lobbyRoomUI.SetActive(false);
                selectCharacter.SetActive(true);
                selectCharacter.SetActive(true);
                break;
            //OTHERS
            case "backToMainMenuFromCreator":
                roomCreateUI.SetActive(false);
                mainMenuUI.SetActive(true);
                break;

            //TEST SELECTED CHARACTER:
            case "allSelected":
                if (value)
                {
                    selectCharacter.SetActive(false);
                    allAccepted.SetActive(true);
                }
                else
                {
                    selectCharacter.SetActive(false);
                    notAllAccepted.SetActive(true);
                }
                break;
        }
    }

    public void SetPlayerList(string playerType, PlayerList tab)
    {
        switch (playerType)
        {
            case "master":
                masterListHandler.setPlayerList(tab.players);
                break;

            case "player":
                playerListHandler.setPlayerList(tab.players);
                break;
        }
    }

    public RectTransform GetPlayerContainer()
    {
        return scrollContainer;
    }

    public GameObject GetPlayerInRoomUI()
    {
        return roomObject;
    }

    public GameObject GetLobbyRoom(string playerType)
    {
        if (playerType == "master")
            return lobbyRoomMasterUI;
        else
            return lobbyRoomUI;
    }
}