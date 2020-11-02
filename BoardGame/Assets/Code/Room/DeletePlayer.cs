using Code.Network;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code
{
    public class DeletePlayer : MonoBehaviour
    {
        [FormerlySerializedAs("neetworkClient")] public NetworkClient networkClient;
        public void SendUsername()
        {
            string username = gameObject.GetComponent<Text>().text;
            networkClient.DeletePlayerFromRoom(username);
        }
    }
}
