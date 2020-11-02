using System.Collections;
using System.Collections.Generic;
using Code.Network;
using UnityEngine;
using UnityEngine.UI;

public class SetCharacter : MonoBehaviour
{
    public Button warrior;
    public Button archer;
    public Button mage;
    public NetworkClient netClient;
    public ContainerUI containerUI;
    public void setWarrior()
    {
        netClient.character = "warrior";
        containerUI.SetInteractableButton("characterSelected", true);
        archer.image.color = Color.white;
        mage.image.color = Color.white;
        warrior.image.color = Color.green;
    }
    public void setArcher()
    {
        netClient.character = "archer";
        containerUI.SetInteractableButton("characterSelected", true);
        archer.image.color = Color.green;
        mage.image.color = Color.white;
        warrior.image.color = Color.white;
    }
    public void setMage()
    {
        netClient.character = "mage";
        containerUI.SetInteractableButton("characterSelected", true);
        archer.image.color = Color.white;
        mage.image.color = Color.green;
        warrior.image.color = Color.white;
    }
}
