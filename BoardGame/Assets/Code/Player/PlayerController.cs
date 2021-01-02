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
    public string usernameText;
    [Header("Level")]
    public int level = 1;
    public int actualExp = 0;
    public int maxExp = 100;
    public int increaseMaxExp = 25;
    private void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
    }
}