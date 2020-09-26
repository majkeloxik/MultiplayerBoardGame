using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.UI;
using System.ComponentModel;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Project.Neetworking
{
    public class NeetworkClient : SocketIOComponent
    {

        private string playerName;
        private string username;
        private string password;
        private string roomName;
        private int roomSize;
        private bool masterRoom = false;
        private bool isReady=false;
        private Dictionary<string, GameObject> serverObjects;
        private ContainerUI containerUI;
        // Start is called before the first frame update



        public void StartGame(string scena)
        {
            SceneManager.LoadScene(scena);
        }
        public void IsReady()
        {
            isReady = !isReady;
            Emit("playerReady", new JSONObject(JsonUtility.ToJson(new identity()
            {
                username = username,
                roomName = roomName,
                isReady = isReady
            })));

        }
        public override void  Start()
        {
            base.Start();
            initialize();
            setupEvents();
            containerUI = GameObject.Find("ContainerUI").GetComponent<ContainerUI>(); 
        }
        private void initialize()
        {
            serverObjects = new Dictionary<string, GameObject>();
        }
        public void lobbyEvent()
        {
            On(roomName, (E) =>
            {
                Debug.Log("jestem w roomie");
            });
        }
        public void setupEvents()
        {
            On("open", (E) =>
            {
                Debug.Log("connection made to the server");
            });
            On("spawn", (E) =>
            {
                Debug.Log("DZIALA");
            });
            On("disconnected", (E) =>
            {
                Debug.Log("disconnected");
            });
            On("usernameExist", (E) =>
            {
                Debug.Log("Username exist!");
                containerUI.registerSucc.SetActive(false);
                containerUI.loginError.SetActive(false);
                containerUI.usernameExist.SetActive(true);
            });
            On("loginError", (E) =>
            {
                Debug.Log("Login Error!");
                containerUI.registerSucc.SetActive(false);
                containerUI.usernameExist.SetActive(false);
                containerUI.loginError.SetActive(true);
            });
            On("signed", (E) =>
            {
                var x = E.data["username"].ToString();
                Debug.Log("Logged in as " + x);
                playerName = x.ToString();
                containerUI.loginUI.SetActive(false);
                containerUI.mainMenuUI.SetActive(true);
            });
            On("userRegistered", (E) =>
            {
                Debug.Log("User" + E.data["username"].ToString() + "registered!");
                containerUI.loginError.SetActive(false);
                containerUI.usernameExist.SetActive(false);
                containerUI.registerSucc.SetActive(true);
            });
            On("createdRoom", (E) =>
            {
                PlayerList tablica = JsonUtility.FromJson<PlayerList>(E.data.ToString());
                containerUI.mainMenuUI.SetActive(false);
                containerUI.lobbyRoomMasterUI.SetActive(true);
                containerUI.roomCreateUI.SetActive(false);
                masterRoom = true;
                containerUI.masterListHandler.setPlayerList(tablica.players);
                Debug.Log("Room created!");
            });
            On("roomError", (E) =>
            {
                Debug.Log("room name exist");
            });
            On("roomList", (E) =>
            {
                containerUI.roomsListUI.SetActive(true);
                Rooms newRoom = JsonUtility.FromJson<Rooms>(E.data.ToString());

                foreach (Transform child in containerUI.scrollContainer.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                foreach (var element in newRoom.rooms)
                {
                    GameObject roomFromList = new GameObject();
                    roomFromList = Instantiate(containerUI.roomObject, containerUI.scrollContainer);
                    roomFromList.name = element.roomName;
                    containerUI.roomObject.GetComponent<setRoomInLsit>().roomName.text = element.roomName.ToString();
                    containerUI.roomObject.GetComponent<setRoomInLsit>().roomSize.text = element.roomSize.ToString();
                }
                containerUI.mainMenuUI.SetActive(false);
                containerUI.roomsListUI.SetActive(true);
            });
            //Join to room, or if is in room update player list
            On("playerList", (E) =>
            {
                PlayerList tablica = JsonUtility.FromJson<PlayerList>(E.data.ToString());
                //Join to room
                if (!containerUI.lobbyRoomUI.activeSelf && !masterRoom)
                {
                    containerUI.lobbyRoomUI.SetActive(true);
                    containerUI.roomsListUI.SetActive(false);
                    containerUI.playerListHandler.setPlayerList(tablica.players);
                }
                //Update player list in room
                else if(containerUI.lobbyRoomMasterUI.activeSelf || containerUI.lobbyRoomUI.activeSelf)
                {
                    if(masterRoom)
                    {
                        Debug.Log("dlugosc tablicy po usunieciu: " + tablica.players.Length);
                        Debug.Log("BeforePlayerListUpdatedMaster");

                        containerUI.masterListHandler.setPlayerList(tablica.players);
                        Debug.Log("PlayerListUpdatedMaster");
                    }
                    else if(!masterRoom)
                    {
                        
                        containerUI.playerListHandler.setPlayerList(tablica.players);
                        Debug.Log("PlayerListUpdatedUser");
                    }
                }
            });
            // drop players from room (TO DO: add msg when deleted)
            On("deleteRoom", (E) =>
            {
                //var isMaster = JsonUtility.FromJson<identity>(E.data.ToString());
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
                    Emit("leaveRoom", new JSONObject(JsonUtility.ToJson(new identity()
                    {
                        roomName = roomName
                    })));
                    if (containerUI.lobbyRoomUI.activeSelf)
                    {
                        //msg to players deleted when master left room
                        Debug.Log("Room deleted by room Master");
                        containerUI.lobbyRoomUI.SetActive(false);
                        containerUI.mainMenuUI.SetActive(true);
                    }
                }
                masterRoom = false;
            });
            On("deletePlayerFromRoom", (E) =>
            {
                var dropUser = E.data["dropUser"].ToString();
                dropUser = dropUser.Replace("\"", "");
                if(username == dropUser)
                {
                    Debug.Log("USUWAM");
                    Emit("leaveRoom", new JSONObject(JsonUtility.ToJson(new identity()
                    {
                        roomName = roomName
                    })));
                    containerUI.lobbyRoomUI.SetActive(false);
                    containerUI.mainMenuUI.SetActive(true);
                }
            });
            On("playerReady", (E) =>
            {
                identity ident = JsonUtility.FromJson<identity>(E.data.ToString());
                if(ident.isReady && !masterRoom)
                {
                    Debug.Log("isready" + ident.isReady);

                }
                else if(!ident.isReady && !masterRoom)
                {
                    Debug.Log("not ready" + ident.isReady);
                }
            });
        }
        public void OnBackInRoom()
        {
            //Delete room, and all users from room
            Emit("deleteFromRoom", new JSONObject(JsonUtility.ToJson(new identity()
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
        public void OnRegister()
        {
            username = GameObject.Find("UserNameText").GetComponent<Text>().text;
            password = GameObject.Find("UserPasswordText").GetComponent<Text>().text;
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
            username = GameObject.Find("UserNameText").GetComponent<Text>().text;
            password = GameObject.Find("UserPasswordText").GetComponent<Text>().text;
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
            roomSize = int.Parse(containerUI.roomSize.text);
            roomName = containerUI.roomName.text;
            if(roomName.Length > 0)
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
            var user = new identity()
            {
                username = selectedUsername,
                roomName = roomName
            };
            Emit("deletePlayerFromRoom", new JSONObject(JsonUtility.ToJson(user)));
        }
        private void OnApplicationQuit()
        {
            Emit("disconnect", new JSONObject(JsonUtility.ToJson(new identity()
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
        public List<GameUser> players;
    }
    [Serializable]
    public class Rooms
    {
        public Room[] rooms;
    }
    public class PlayerList
    {
        public String[] players;
    }
    [Serializable]
    public class identity
    {
        public string username;
        public string roomName;
        public bool isMaster;
        public bool isReady;
    }
}

