using Code.Network;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Fields fields;
    public PlayerProperties playerProperties;
    public NavMeshAgent myNavMeshAgent;
    public List<int> moveOptions;
    public int moveTo;

    public Camera playerCamera;
    [Header("Player info UI")]
    public Text stats;
    public Text usernameText;

    private void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void SetPlayerInfo()
    {

    }
}