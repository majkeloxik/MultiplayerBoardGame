using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.UI;
using System.ComponentModel;
using UnityEditor;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

namespace Project.Neetworking
{
    public class NeetworkClient : SocketIOComponent
    {
        private string username;
        private string password;
        private string roomName;
        private int roomSize;

        private bool masterRoom = false;
        private bool isReady = false;
        private ContainerUI containerUI;

        public string character;


        public void StartGame()
        {
            Emit("characterSelected", new JSONObject(JsonUtility.ToJson(new Identity()
            {
                username = username,
                roomName = roomName
            })));
            containerUI.characterSelected.interactable = false;
        }
        public void IsReady()
        {
            isReady = !isReady;
            Emit("playerReady", new JSONObject(JsonUtility.ToJson(new Identity()
            {
                username = username,
                roomName = roomName,
                isReady = isReady
            })));

        }
        public override void  Start()
        {
            base.Start();
            setupEvents();
            containerUI = GameObject.Find("ContainerUI").GetComponent<ContainerUI>(); 
        }
        public void setupEvents()
        {
            On("open", (E) =>
            {
                Debug.Log("connection made to the server");
            });
            On("auth", (E) =>
            {
                var newToken = E.data["token"].ToString();
                Debug.Log(newToken);
                newToken = newToken.Replace("\"", "");
                Token token = new Token();
                token.token = newToken;
                Debug.Log(JsonUtility.ToJson(token));
                Emit("authenticate", new JSONObject(JsonUtility.ToJson(token)));
            });
            On("disconnected", (E) =>
            {
                Debug.Log("disconnected");
                Application.Quit();
            });
            On("accountHandler", (E) =>
            {
                var handler = E.data["handler"].ToString();
                handler = handler.Replace("\"", "");

                if (handler == "loginError")
                {
                    Debug.Log("Login Error!");
                    containerUI.registerSucc.SetActive(false);
                    containerUI.usernameExist.SetActive(false);
                    containerUI.loginError.SetActive(true);
                }
                else if (handler == "signed")
                {
                    Debug.Log("Signed!");
                    containerUI.loginUI.SetActive(false);
                    containerUI.mainMenuUI.SetActive(true);
                }
                else if (handler == "usernameExist")
                {
                    Debug.Log("Username exist!");
                    containerUI.registerSucc.SetActive(false);
                    containerUI.loginError.SetActive(false);
                    containerUI.usernameExist.SetActive(true);
                }
                else if (handler == "registred")
                {
                    containerUI.loginError.SetActive(false);
                    containerUI.usernameExist.SetActive(false);
                    containerUI.registerSucc.SetActive(true);
                }
            });
            On("createdRoom", (E) =>
            {
                PlayerList tablica = JsonUtility.FromJson<PlayerList>(E.data.ToString());
                containerUI.startButton.GetComponent<Button>().interactable = false;
                containerUI.mainMenuUI.SetActive(false);
                containerUI.lobbyRoomMasterUI.SetActive(true);
                containerUI.roomCreateUI.SetActive(false);
                masterRoom = true;
                isReady = true;
                containerUI.masterListHandler.setPlayerList(tablica.players);
            });
            On("roomError", (E) =>
            {
                Debug.Log("room name exist");
            });
            On("roomList", (E) =>
            {
                containerUI.roomsListUI.SetActive(true);
                Rooms rooms = JsonConvert.DeserializeObject<Rooms>(E.data.ToString());

                foreach (Transform child in containerUI.scrollContainer.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                if (rooms.rooms.Count > 0)
                {
                    foreach (var element in rooms.rooms)
                    {
                        GameObject roomFromList = Instantiate(containerUI.roomObject, containerUI.scrollContainer);
                        roomFromList.name = element.Value.roomName;
                        GameObject[] roomInfo = GameObject.FindGameObjectsWithTag("Respawn");
                        foreach(GameObject x in roomInfo)
                        {
                            if(x.name == "RoomNameText")
                            {
                                x.GetComponent<Text>().text = element.Value.roomName.ToString();
                            }
                            else
                            {
                                x.GetComponent<Text>().text = element.Value.roomSize.ToString();
                            }
                        }
                        containerUI.roomObject.GetComponent<setRoomInLsit>().roomName.text = element.Value.roomName.ToString();
                        containerUI.roomObject.GetComponent<setRoomInLsit>().roomSize.text = element.Value.roomSize.ToString();
                    }
                }
                containerUI.mainMenuUI.SetActive(false);
                containerUI.roomsListUI.SetActive(true);
            });
            //Join to room, or if is in room update player list
            On("playerList", (E) =>
            {
                PlayerList tablica = JsonUtility.FromJson<PlayerList>(E.data.ToString());
                //Join to room
                bool userExist = false;
                foreach(string name in tablica.players)
                {
                    //TODO: add handling deleting player from room
                    if(name.Contains(username))
                    {
                        userExist = true;
                        break;
                    }
                }

                if (!containerUI.lobbyRoomUI.activeSelf && !masterRoom && userExist)
                {
                    containerUI.lobbyRoomUI.SetActive(true);
                    containerUI.roomsListUI.SetActive(false);
                    containerUI.playerListHandler.setPlayerList(tablica.players);
                }
                //Update player list in room
                else if(containerUI.lobbyRoomMasterUI.activeSelf || containerUI.lobbyRoomUI.activeSelf && userExist)
                {

                    if(masterRoom)
                    {
                        containerUI.masterListHandler.setPlayerList(tablica.players);
                        if(tablica.players.Length < 2)
                            containerUI.startButton.GetComponent<Button>().interactable = false;
                    }
                    else if(!masterRoom)
                    {
                        containerUI.playerListHandler.setPlayerList(tablica.players);
                    }
                }
                //Back to main menu ( player kicked from room )
                else if(!userExist)
                {
                    containerUI.lobbyRoomUI.SetActive(false);
                    containerUI.mainMenuUI.SetActive(true);
                }
            });
            // drop players from room (TO DO: add msg when deleted)
            On("deleteRoom", (E) =>
            {
                //Remove room handled in OnBackInRoom()
                if (masterRoom)
                {
                    if(containerUI.lobbyRoomMasterUI.activeSelf)
                    {
                        containerUI.lobbyRoomMasterUI.SetActive(false);
                        containerUI.mainMenuUI.SetActive(true);
                    }
                }
                else if (!masterRoom)
                {
                    Emit("leaveRoomSocket", new JSONObject(JsonUtility.ToJson(new Identity()
                    {
                        roomName = roomName
                    })));
                    if (containerUI.lobbyRoomUI.activeSelf)
                    {
                        //TODO: msg to players deleted when master left room
                        containerUI.lobbyRoomUI.SetActive(false);
                        containerUI.mainMenuUI.SetActive(true);
                    }
                }
                masterRoom = false;
            });
            On("selectCharacter", (E) =>
            {
                if(masterRoom)
                {
                    containerUI.lobbyRoomMasterUI.SetActive(false);
                    containerUI.selectCharacter.SetActive(true);
                }
                else
                {
                    containerUI.lobbyRoomUI.SetActive(false);
                    containerUI.selectCharacter.SetActive(true);
                }
            });
            On("playerReady", (E) =>
            {
                var result = JsonConvert.DeserializeObject<bool>(E.data["roomReady"].ToString());
                if(masterRoom)
                {
                    if(result)
                    {
                        containerUI.startButton.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        containerUI.startButton.GetComponent<Button>().interactable = false;
                    }
                }
                else if(!masterRoom)
                {
                    if(isReady)
                    {
                        Debug.Log("JESTEM READY");
                    }
                    else if(!isReady)
                    {
                        Debug.Log("NIE JESTEM READY");
                    }
                }
            });
            On("selectedCharacters", (E) =>
            {
                var result = JsonConvert.DeserializeObject<bool>(E.data["allSelected"].ToString());
                if(result)
                {
                    containerUI.selectCharacter.SetActive(false);
                    containerUI.allAccepted.SetActive(true);
                }
                else
                {
                    containerUI.selectCharacter.SetActive(false);
                    containerUI.notAllAccepted.SetActive(true);
                }
            });
        }
        //Exit from room if player, if master delete room and all players from them
        public void OnBackInRoom()
        {
            //Delete room, and all users from room
            Emit("leaveRoom", new JSONObject(JsonUtility.ToJson(new Identity()
                {
                    username = username,
                    roomName = roomName,
                    isMaster = masterRoom
                })));
            //Left room, and refresh player list for other users
            if(!masterRoom)
            {
                containerUI.lobbyRoomUI.SetActive(false);
                containerUI.mainMenuUI.SetActive(true);
                roomName = "";
            }
        }
        public void SelectCharacter()
        {
            Emit("selectCharacter", new JSONObject(JsonUtility.ToJson(new Identity()
            {
                roomName = roomName
            })));
        }
        public void OnRegister()
        {
            username = GameObject.Find("UsernameField").GetComponent<InputField>().text;
            password = GameObject.Find("PasswordField").GetComponent<InputField>().text;
            if(username.Length > 0 && password.Length > 0)
            {
                Emit("createAccount", new JSONObject(JsonUtility.ToJson(new GameUser()
                {
                    username = username,
                    password = password
                })));
            }
            else
            {
                Debug.Log("Create account ERROR");
            }
        }
        public void OnLogin()
        {
            username = GameObject.Find("UsernameField").GetComponent<InputField>().text;
            password = GameObject.Find("PasswordField").GetComponent<InputField>().text;
            if(username.Length > 0 && password.Length > 0)
            {
                Emit("signIn", new JSONObject(JsonUtility.ToJson(new GameUser()
                {
                    username = username,
                    password = password
                })));
            }
        }   
        public void OnCreateRoom()
        {
            roomSize = int.Parse(GameObject.Find("RoomSizeDroped").GetComponent<Text>().text);
            roomName = GameObject.Find("RoomName").GetComponent<InputField>().text;
            if (roomName.Length > 0)
            {
                var newRoom = new Room()
                {
                    username = username,
                    roomName = roomName,
                    roomSize = roomSize
                };
                
                Emit("roomCreate", new JSONObject(JsonUtility.ToJson(newRoom)));
            }
        }
        public void OnRoomList()
        {
            Emit("getRoomList");
        }
        public void OnRoomConnect(string selectedRoomName)
        {
            roomName = selectedRoomName;
            var newRoom = new Room()
            {
                username = username,
                roomName = selectedRoomName,
            };
            Emit("roomConnect", new JSONObject(JsonUtility.ToJson(newRoom)));
            //pobieramy nazwe roomu z pola z nazwą, wysyłamy ją jako dane , robimy w serwerze join do danego roomu , nasłuchujemy u klienta na info o zmienie w roomie, i wysyłamy aktualną liste graczy w roomie
        }
        public void DeletePlayerFromRoom(string selectedUsername)
        {
            //wysyłamy username gracza do usunięcia, serwer rozsyła to wszystkich uczestnikow roomu, jezeli wyslany username = nazsemu username to zostajemy wyrzuceni z roomu
            var user = new Identity()
            {
                username = selectedUsername,
                roomName = roomName
            };
            Emit("deletePlayerFromRoom", new JSONObject(JsonUtility.ToJson(user)));
        }
        public void BackToMainMenu()
        {
            containerUI.roomCreateUI.SetActive(false);
            containerUI.mainMenuUI.SetActive(true);
        }
        private void OnApplicationQuit()
        {
            Emit("disconnect", new JSONObject(JsonUtility.ToJson(new Identity()
            {
                username = username,
                roomName = roomName,
                isMaster = masterRoom
            })));
        }
        public override void Update()
        {
            base.Update();
        }
    }
    [Serializable]
    public class GameUser
    {
        public string username;
        public string password;
    }
    [Serializable]
    public class Room
    {
        public string id;
        public string username;
        public string roomName;
        public int roomSize;
        public List<string> players;
    }
    [Serializable]
    public class Rooms
    {
        public Dictionary<string, Room> rooms;
    }
    [Serializable]
    public class PlayerList
    {
        public String[] players;
    }
    [Serializable]
    public class Identity
    {
        public string username;
        public string roomName;
        public bool isMaster;
        public bool isReady;
    }

    public class Token
    {
        public string token;
    }
}