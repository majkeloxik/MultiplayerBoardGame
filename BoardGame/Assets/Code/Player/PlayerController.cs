using System.Collections.Generic;
using Code.Network;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Code.Player
{
	public class PlayerController : MonoBehaviour
	{
		public Fields fields;
		public PlayerProperties playerProperties;
		public NavMeshAgent myNavMeshAgent;
		public List<int> moveOptions;
		public int moveTo;
		public Text usernameText;
		private void Start()
		{
			myNavMeshAgent = GetComponent<NavMeshAgent>();
		}

		public void MoveToField()
		{
			foreach (var element in fields.fields)
			{
				if(element.id == moveTo)
				{
					myNavMeshAgent.SetDestination(element.position);
					break;
				}
			}
		}
	}
}
