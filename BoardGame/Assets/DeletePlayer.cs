using Project.Neetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeletePlayer : MonoBehaviour
{
    public NeetworkClient neetworkClient;
    public void SendUsername()
    {
        string username = gameObject.GetComponent<Text>().text;
        neetworkClient.DeletePlayerFromRoom(username);
    }
}
