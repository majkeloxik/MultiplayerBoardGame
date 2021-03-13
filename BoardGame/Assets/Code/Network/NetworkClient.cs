//using Code.Player;
using Newtonsoft.Json;
using SocketIO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Network
{
    public class NetworkClient : SocketIOComponent
    {
        public string username;
        private string password;

        private string roomName;
        private int roomSize;

        private bool masterRoom = false;
        private bool isReady = false;

        [SerializeField]
        private ContainerUI containerUI;

        [SerializeField]
        private ObjContainer objContainer;

        private ObjContainer ObjContainer
        {
            get
            {
                return objContainer = (objContainer == null) ? FindObjectOfType<ObjContainer>() : objContainer;
            }
        }

        public string character;

        public override void Start()
        {
            
            base.Start();
            SetupEvents();
        }

        private void SetupEvents()
        {
            On("disconnect", (E) =>
            {
                Debug.LogError("SERVER DISCCONECTED");
            });
            On("open", (E) =>
            {
                Debug.Log("connection made to the server");
            });
            On("auth", (E) =>
            {
                var newToken = E.data["token"].ToString();
                newToken = newToken.Replace("\"", "");
                Token token = new Token
                {
                    token = newToken
                };
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
                    containerUI.SetActiveUI("loginError", true);
                    Debug.Log("Login Error!");
                }
                else if (handler == "signed")
                {
                    Debug.Log("Signed!");
                    containerUI.SetActiveUI("signed", true);
                }
                else if (handler == "usernameExist")
                {
                    Debug.Log("Username exist!");
                    containerUI.SetActiveUI("usernameExist", true);
                }
                else if (handler == "registred")
                {
                    containerUI.passwordLogin.GetComponent<InputField>().text = password;
                    containerUI.usernameLogin.GetComponent<InputField>().text = username;
                    containerUI.SetActiveUI("registred", true);
                }
            });
            //TODO: change playerList
            On("createdRoom", (E) =>
            {
                PlayerList tablica = JsonUtility.FromJson<PlayerList>(E.data.ToString());
                masterRoom = true;
                isReady = true;
                containerUI.SetActiveUI("createdRoom", true);
                containerUI.SetPlayerList("master", tablica);
            });
            On("roomError", (E) =>
            {
                Debug.Log("room name exist");
            });
            On("roomList", (E) =>
            {
                Transform playersContainer = containerUI.GetPlayerContainer();
                GameObject playerInRoom = containerUI.GetPlayerInRoomUI();

                Rooms rooms = JsonConvert.DeserializeObject<Rooms>(E.data.ToString());

                foreach (Transform child in playersContainer.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                if (rooms.rooms.Count > 0)
                {
                    foreach (var element in rooms.rooms)
                    {
                        GameObject roomFromList = Instantiate(playerInRoom, playersContainer);
                        roomFromList.name = element.Value.roomName;
                        roomFromList.GetComponent<setRoomInLsit>().roomName.text = element.Value.roomName.ToString();
                        containerUI.GetPlayerContainer().sizeDelta = new Vector2(containerUI.GetPlayerContainer().sizeDelta.x, containerUI.GetPlayerContainer().sizeDelta.x + 200);
                        roomFromList.GetComponent<setRoomInLsit>().roomSize.text = "size: " + element.Value.players.Count + "/" + element.Value.roomSize.ToString();
                    }
                }
                containerUI.SetActiveUI("roomList", true);
            });
            //Join to room, or if player is in room update player list
            On("playerList", (E) =>
            {
                PlayerList tablica = JsonUtility.FromJson<PlayerList>(E.data.ToString());
                GameObject lobbyRoom = containerUI.GetLobbyRoom("player");
                GameObject masterLobbyRoom = containerUI.GetLobbyRoom("master");

                //Join to room
                bool userExist = false;
                foreach (string name in tablica.players)
                {
                    //TODO: add handling deleting player from room
                    if (name.Contains(username))
                    {
                        userExist = true;
                        break;
                    }
                }
                //player join with room
                if (!lobbyRoom.activeSelf && !masterRoom && userExist)
                {
                    containerUI.SetActiveUI("joinToRoom", true);
                    containerUI.SetPlayerList("player", tablica);
                }
                //Update player list in room
                else if (masterLobbyRoom.activeSelf || lobbyRoom.activeSelf && userExist)
                {
                    if (masterRoom)
                    {
                        containerUI.SetPlayerList("master", tablica);
                        if (tablica.players.Length < 2)
                            containerUI.SetInteractableButton("startGameButton", false);
                    }
                    else if (!masterRoom)
                    {
                        containerUI.SetPlayerList("player", tablica);
                    }
                }
                //Back to main menu ( player kicked from room )
                else if (!userExist)
                {
                    containerUI.SetActiveUI("kickedFromRoom", true);
                }
            });
            // drop players from room (TO DO: add msg when deleted)
            On("deleteRoom", (E) =>
            {
                GameObject lobbyRoom = containerUI.GetLobbyRoom("player");
                GameObject masterLobbyRoom = containerUI.GetLobbyRoom("master");
                //Remove room handled in OnBackInRoom()
                if (masterRoom)
                {
                    if (masterLobbyRoom.activeSelf)
                    {
                        containerUI.SetActiveUI("deleteRoomMaster", true);
                    }
                }
                else if (!masterRoom)
                {
                    Emit("leaveRoomSocket", new JSONObject(JsonUtility.ToJson(new Identity()
                    {
                        roomName = roomName
                    })));
                    if (lobbyRoom.activeSelf)
                    {
                        //TODO: msg to players
                        //when master left room ( players deleted from room )
                        containerUI.SetActiveUI("deleteRoomPlayer", true);
                    }
                }
                masterRoom = false;
            });
            On("selectCharacter", (E) =>
            {
                if (masterRoom)
                {
                    containerUI.SetActiveUI("selectCharacterMaster", true);
                }
                else
                {
                    containerUI.SetActiveUI("selectCharacterPlayer", true);
                }
            });
            On("playerReady", (E) =>
            {
                var result = JsonConvert.DeserializeObject<bool>(E.data["roomReady"].ToString());
                var playerReadyName = JsonConvert.DeserializeObject<String>(E.data["username"].ToString());
                if (masterRoom)
                {
                    if (result)
                    {
                        containerUI.SetInteractableButton("startGameButton", true);
                    }
                    else
                    {
                        containerUI.SetInteractableButton("startGameButton", false);
                    }
                }
                else if (!masterRoom)
                {
                    if (isReady)
                    {
                        //Debug.Log("READY");
                    }
                    else if (!isReady)
                    {
                        //Debug.Log("NOT READY");
                    }
                }
                containerUI.SetPlayerReadyInRoom(playerReadyName, result);
            });
            //TODO: handling change scene to main game
            On("selectedCharacters", (E) =>
            {
                var result = JsonConvert.DeserializeObject<bool>(E.data["allSelected"].ToString());

                if (result)
                {
                    SceneManager.LoadScene("MainGame", LoadSceneMode.Single);

                    //Wait for generate world

                    Emit("createGame", new JSONObject(JsonUtility.ToJson(new Identity()
                    {
                        roomName = roomName,
                        isMaster = masterRoom
                    })));
                }
                else
                {
                    containerUI.SetActiveUI("allSelected", false);
                }
            });
            //GAME
            On("createGame", (E) =>
            {
                //Generate MAP
                //board - board representation as matrix
                var boardArrayString = E.data["board"].ToString();
                var board = JsonConvert.DeserializeObject<int[][]>(boardArrayString);

                FindObjectOfType<MapGenerator>().GenerateMap(board);

                //Spawn players
                GameProperties allPlayers = new GameProperties
                {
                    gameRoom = JsonConvert.DeserializeObject<Dictionary<string, PlayerProperties>>(E.data["gameRoom"].ToString())
                };

                int indexToClone = 0;
                GameObject playersCont = new GameObject("Players");

                float startPos = -800f;
                float avatarPos;
                if (allPlayers.gameRoom.Count == 2)
                {
                    avatarPos = 1600f;
                }
                else if (allPlayers.gameRoom.Count == 3)
                {
                    avatarPos = 800f;
                }
                else
                {
                    avatarPos = 400f;
                }
                var GameUI = GameObject.FindGameObjectWithTag("MainGameUI");
                foreach (var element in allPlayers.gameRoom)
                {
                    GameObject newCharacter;
                    RawImage newPlayerInfo;
                    //Spawn player on first field and on correct slot
                    Vector3 spawnPosition = ObjContainer.MapGenerator.fieldsList[0].GetComponent<FieldInfo>().slots[indexToClone].transform.position;

                    if (element.Value.character.characterClass == "mage")
                    {
                        newCharacter = Instantiate(Resources.Load<GameObject>("Prefabs/mageCharacter1"), spawnPosition, Quaternion.identity, playersCont.transform);
                        newPlayerInfo = Instantiate(Resources.Load<RawImage>("Prefabs/PlayerInfo"), new Vector3(startPos + indexToClone * avatarPos, 430f, 0f), Quaternion.identity, GameUI.transform);
                        newPlayerInfo.transform.localPosition = new Vector3(startPos + indexToClone * avatarPos, 430f, 0f);
                        newPlayerInfo.texture = Resources.Load<Texture>("Avatars/avatar1");
                    }
                    else if (element.Value.character.characterClass == "archer")
                    {
                        newCharacter = Instantiate(Resources.Load<GameObject>("Prefabs/archer/archerCharacter1"), spawnPosition, Quaternion.identity, playersCont.transform);
                        newPlayerInfo = Instantiate(Resources.Load<RawImage>("Prefabs/PlayerInfo"), new Vector3(startPos + indexToClone * avatarPos, 430f, 0f), Quaternion.identity, GameUI.transform);
                        newPlayerInfo.transform.localPosition = new Vector3(startPos + indexToClone * avatarPos, 430f, 0f);
                        newPlayerInfo.texture = Resources.Load<Texture>("Avatars/avatar2");
                    }
                    else
                    {
                        newCharacter = Instantiate(Resources.Load<GameObject>("Prefabs/warriorCharacter1"), spawnPosition, Quaternion.identity, playersCont.transform);
                        newPlayerInfo = Instantiate(Resources.Load<RawImage>("Prefabs/PlayerInfo"), new Vector3(startPos + indexToClone * avatarPos, 430f, 0f), Quaternion.identity, GameUI.transform);
                        newPlayerInfo.transform.localPosition = new Vector3(startPos + indexToClone * avatarPos, 430f, 0f);
                        newPlayerInfo.texture = Resources.Load<Texture>("Avatars/avatar3");
                    }

                    newPlayerInfo.name = "PlayerInfo_" + element.Key;
                    newPlayerInfo.GetComponent<PlayerInfoUI>().playerNameText.text = element.Key;
                    newCharacter.name = element.Key;
                    newCharacter.GetComponent<PlayerController>().playerProperties = element.Value;

                    newCharacter.GetComponent<PlayerController>().usernameText = element.Key;
                    ObjContainer.playersList.Add(newCharacter);

                    newPlayerInfo.GetComponent<PlayerInfoUI>().SetPlayerInfo(indexToClone);
                    indexToClone++;

                }
                ObjContainer.MapGenerator.fieldsList[0].GetComponent<FieldInfo>().enabled = false;
            });
            On("playerTurn", (E) =>
            {
                var actualPlayer = E.data["whoMove"].ToString();
                actualPlayer = actualPlayer.Replace("\"", "");

                ObjContainer.playerAction.SetActive(false);
                ObjContainer.whoMoveImage.SetActive(false);

                ObjContainer.SetWhoMove(actualPlayer);

                ObjContainer.PlayerEndTurn();

                ObjContainer.whoMoveImage.SetActive(true);

                ObjContainer.CameraController.SetCamera();

                if (actualPlayer == username)
                {
                    ObjContainer.dice.interactable = true;
                }
                else
                {
                    ObjContainer.dice.interactable = false;
                }
            });
            On("possibleMoves", (E) =>
            {
                var isRandom = JsonConvert.DeserializeObject<bool>(E.data["isRandom"].ToString());
                if (isRandom && ObjContainer.dice.interactable)
                {
                    ObjContainer.dice.interactable = false;
                }
                //All see dice Value
                string diceValue = E.data["diceV"].ToString();

                ObjContainer.SetDiceValue(diceValue);
                //Only player who move see where can go
                if (ObjContainer.actualPlayer == username)
                {
                    var possibleMovesString = E.data["possibleMoves"].ToString();
                    var possibleMoves = JsonConvert.DeserializeObject<int[]>(possibleMovesString);
                    ObjContainer.PossibleMoves(true, possibleMoves);
                }
            });
            //Player change position on selected before
            On("playerMoveTo", (E) =>
            {
                int whereMove = Convert.ToInt32(E.data["whereMove"].ToString());
                var isRandom = JsonConvert.DeserializeObject<bool>(E.data["isRandom"].ToString());
                if (isRandom)
                {
                    ObjContainer.PlayerMovedTo(whereMove);
                }

                string playerMove = E.data["whoMove"].ToString();
                playerMove = playerMove.Replace("\"", "");

                int playerIndex = ObjContainer.playersList.FindIndex(x => x.name == playerMove);
                int indexWhereMove = ObjContainer.fieldsList.FindIndex(t => t.GetComponent<FieldInfo>().id == whereMove);

                //Field id , place selected by player
                ObjContainer.whereMove = whereMove;

                int typeID = Convert.ToInt32(E.data["typeID"].ToString());
                int actionID = Convert.ToInt32(E.data["actionID"].ToString());
                //Before change position add type and id action ( after character enter on poss field, set active )
                ObjContainer.ActionController.SetActionInfo(typeID, actionID);
                ObjContainer.playersList[playerIndex].GetComponent<NavMeshAgent>().SetDestination(ObjContainer.fieldsList[indexWhereMove].transform.position);
                

                //TODO: handling when start action ( colider on field // wait for the time)
            });
            On("showAction", (E) =>
            {
                if (username == ObjContainer.actualPlayer)
                {
                    ObjContainer.playerAction.SetActive(true);
                }
                else
                {
                    ObjContainer.playerAction.SetActive(true);
                }
            });
            On("characterValues", (E) =>
            {
                var whoMove = E.data["whoMove"].ToString();
                whoMove = whoMove.Replace("\"", "");

                var changeValue = JsonConvert.DeserializeObject<ChangeValues[]>(E.data["changeValues"].ToString());
                int playerIndex = ObjContainer.playersList.FindIndex(nm => nm.name == whoMove);
                foreach (var x in changeValue)
                {
                    var increaseValues = ObjContainer.playersList[playerIndex].GetComponent<PlayerController>();
                    increaseValues.playerProperties.properties += x;
                    increaseValues.playerProperties.character += x;
                }
                ObjContainer.SetLevelValue(playerIndex);
                GameObject.FindGameObjectWithTag("MainGameUI").transform.Find("PlayerInfo_" + whoMove).GetComponent<PlayerInfoUI>().SetPlayerInfo(playerIndex);
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
            if (!masterRoom)
            {
                containerUI.SetActiveUI("leaveRoom", true);
                roomName = "";
            }
        }

        public void SelectCharacter()
        {
            Emit("selectCharacter", new JSONObject(JsonUtility.ToJson(new Identity()
            {
                roomName = roomName,
                isMaster = masterRoom
            })));
        }

        public void OnRegister()
        {
            username = GameObject.Find("UsernameFieldRegister").GetComponent<InputField>().text;
            password = GameObject.Find("PasswordFieldRegister").GetComponent<InputField>().text;
            string passwordValid = GameObject.Find("PasswordFieldValid").GetComponent<InputField>().text;
            
            if (username.Length > 0 && password.Length > 0 && password == passwordValid)
            {
                Emit("createAccount", new JSONObject(JsonUtility.ToJson(new GameUser()
                {
                    username = username,
                    password = password
                })));
            }
            else if(passwordValid != password)
            {
                containerUI.passwordValidError.SetActive(true);
            }
            else
            {
                
            }
        }

        public void OnLogin()
        {
            //TODO: correct login handling
            username = GameObject.Find("UsernameFieldLogin").GetComponent<InputField>().text;
            password = GameObject.Find("PasswordFieldLogin").GetComponent<InputField>().text;

            if (username.Length > 0 && password.Length > 0)
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
            else
            {
                containerUI.SetActiveUI("roomNameToShort", true);
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
            containerUI.SetActiveUI("backToMainMenuFromCreator", true);
        }

        public void DiceValue()
        {
            Emit("diceValue", new JSONObject(JsonUtility.ToJson(new Identity()
            {
                username = username,
                roomName = roomName
            })));
        }

        public void StartGame()
        {
            Emit("characterSelected", new JSONObject(JsonUtility.ToJson(new Identity()
            {
                isMaster = masterRoom,
                username = username,
                roomName = roomName,
                character = character
            })));
            containerUI.SetInteractableButton("characterSelected", false);
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

        public void FieldSelected(int fieldID)
        {
            ObjContainer.PlayerMovedTo(fieldID);
            Emit("playerMoveTo", new JSONObject(JsonUtility.ToJson(new Identity()
            {
                username = username,
                roomName = roomName,
                fieldID = fieldID
            })));
        }

        public void PlayerAction(string category)
        {
            Emit("playerAction", new JSONObject(JsonUtility.ToJson(new Identity()
            {
                username = username,
                roomName = roomName,
                changesCategory = category
            })));
        }

        public void EndTurn()
        {
        }

        public override void Update()
        {
            base.Update();
        }

        private new void OnApplicationQuit()
        {
            Emit("disconnect", new JSONObject(JsonUtility.ToJson(new Identity()
            {
                username = username,
                roomName = roomName,
                isMaster = masterRoom
            })));
        }
    }

    [Serializable]
    public class Board
    {
        public int[][] board;
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
    public class Character
    {
        public string characterClass;
        public int mana;
        public int attack;
        public int defence;
        public int health;
        public Ability ability1;
        public Ability ability2;
        public Ability ability3;

        public Character()
        {
            this.mana = 0;
            this.attack = 0;
            this.defence = 0;
            this.health = 0;
        }

        public Character(int mana, int attack, int defence, int health)
        {
            this.mana = mana;
            this.attack = attack;
            this.defence = defence;
            this.health = health;
        }

        public static Character operator +(Character ob1, ChangeValues ob2)
        {
            return new Character(ob1.mana + ob2.mana, ob1.attack + ob2.attack, ob1.defence + ob2.defence, ob1.health + ob2.health);
        }
    }

    [Serializable]
    public class Ability
    {
        public string name;
        public int abilityLevel;
        public int cooldown;
        public int dmgValue;

        public Ability(string abName, int abLevel, int cdr, int dmgV)
        {
            name = abName;
            abilityLevel = abLevel;
            cooldown = cdr;
            dmgValue = dmgV;
        }
    }

    [Serializable]
    public class ChangeValues
    {
        public int mana;
        public int attack;
        public int defence;
        public int health;
        public int exp;
        public int gold;

        public override string ToString()
        {
            return "Changed values:" + mana + " " + attack + " " + defence + " " + health + " " + exp + " " + gold;
        }
    }

    [Serializable]
    public class PlayerProperties
    {
        public Properties properties;
        public Character character;
    }

    [Serializable]
    public class Properties
    {
        public int exp;
        public int gold;
        public int actualField;
        public int diceValue;

        public Properties()
        {
            this.exp = 0;
            this.gold = 0;
        }

        public Properties(int exp, int gold)
        {
            this.exp = exp;
            this.gold = gold;
        }

        public static Properties operator +(Properties ob1, ChangeValues ob2)
        {
            return new Properties(ob1.exp + ob2.exp, ob1.gold + ob2.gold);
        }
    }

    [Serializable]
    public class GameProperties
    {
        public Dictionary<string, PlayerProperties> gameRoom;
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
        public string character;
        public int fieldID;
        public string changesCategory;
    }

    [Serializable]
    public class Token
    {
        public string token;
    }
}