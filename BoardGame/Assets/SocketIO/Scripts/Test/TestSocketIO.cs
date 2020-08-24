#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections;
using UnityEngine;
using SocketIO;
using System;
using WebSocketSharp;
using UnityEngine.UI;
using System.Net.Sockets;

public class TestSocketIO : MonoBehaviour
{
	private SocketIOComponent socket;
	public Text roomName;
	public Text roomSize;
	public void Start() 
	{
		socket = GetComponent<SocketIOComponent>();
		//socket.On("open", OnConnected);
		socket.On("spawn", OnSpawned);
		socket.On("lobbyRoom", LobbyRoom);
	}

    private void OnSpawned(SocketIOEvent obj)
    {
		Debug.Log("spawned" + obj.data);
        Room room = new Room();
        var tmp = JsonUtility.FromJson<Room>(obj.data.ToString());
        Debug.Log(tmp);

    }
	public void OnCreatedRoom()
    {
		Room room = new Room();
		room.Name = roomName.text;
		room.RoomSize = int.Parse(roomSize.text);

		string json = JsonUtility.ToJson(room);
		socket.Emit("createdRoom", new JSONObject(json));
		GetComponent<RoomController>().sizeStatus.text = "Server size: " + roomSize.text;
		GetComponent<RoomController>().nameStatus.text = "Server name: " + roomName.text.ToString();
	}
	public void onClickLobbyRoom()
    {
        socket.Emit("lobbyRoom");
		Debug.Log("Wysłano");
    }
	private void LobbyRoom(SocketIOEvent obj)
    {
		Debug.Log("here");
		Debug.Log(obj.data);
    }
    private void OnConnected(SocketIOEvent obj)
    {

    }
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}