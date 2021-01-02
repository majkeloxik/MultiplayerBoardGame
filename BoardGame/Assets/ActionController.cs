using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    public Action1Info action1Info;
    public Action2Info action2Info;

    public Text titleText;
    public Text descriptionText;
    public Text integrateWithText;
    public Text RewardText;
    public Text option1Text;
    public Text option2Text;
    public Button option1Button;
    public Button option2Button;

    private ObjContainer objContainer;

    private ObjContainer ObjContainer
    {
        get
        {
            return objContainer = (objContainer == null) ? FindObjectOfType<ObjContainer>() : objContainer;
        }
    }

    public void SetActionInfo(int type, int id)
    {
        if (type == 1)
        {
            action1Info = Resources.Load<Action1Info>("Action/" + type.ToString() + "/Action " + id.ToString());
            SetActionValues(type);
        }
        else if (type == 2)
        {
            action2Info = Resources.Load<Action2Info>("Action/" + type.ToString() + "/Action " + id.ToString());
            SetActionValues(type);
        }
    }

    public void SetActionValues(int type)
    {
        option1Text.text = "";
        option2Text.text = "";
        if (type == 1)
        {
            action1Info.integrateWith.Add(ObjContainer.actualPlayer);
            titleText.text = action1Info.title;
            descriptionText.text = action1Info.description;
            integrateWithText.text = action1Info.integrateWith[0].ToString();
            int changes = action1Info.value.Count;
            for (int i = 0; i < changes; i++)
            {
                RewardText.text += action1Info.attribute[i] + ": " + action1Info.value[i] + "\n";
            }
            option1Text.text = "Odbierz";
        }
        else if (type == 2)
        {
            action2Info.integrateWith.Add(ObjContainer.actualPlayer);
            titleText.text = action2Info.title;
            descriptionText.text = action2Info.description;
            integrateWithText.text = action2Info.integrateWith[0].ToString();

            int changes = action2Info.value.Count;
            for (int i = 0; i < changes; i++)
            {
                RewardText.text += action2Info.attribute[i] + ": " + action2Info.value[i] + "\n";
            }
            option1Text.text = action2Info.correctAnswer;
            option2Text.text = action2Info.wrongAnswer;
        }
        SetActionUI(type);
    }

    public void SetActionUI(int actionType)
    {
        if(ObjContainer.actualPlayer == ObjContainer.username)
        {
            option1Button.interactable = true;
            option2Button.interactable = true;
        }
        else
        {
            option1Button.interactable = false;
            option2Button.interactable = false;
        }
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);
        option1Button.onClick.RemoveAllListeners();
        option2Button.onClick.RemoveAllListeners();

        if (actionType == 1)
        {
            
            option1Button.gameObject.SetActive(true);
            option1Button.transform.localPosition = new Vector3(0f, -160f, 0f);

            option1Button.onClick.AddListener(ReciveReward);
        }
        else if (actionType == 2)
        {
            option1Button.transform.localPosition = new Vector3(-150f, -160f, 0f);
            option2Button.transform.localPosition = new Vector3(150f, -160f, 0f);
            option1Button.gameObject.SetActive(true);
            option2Button.gameObject.SetActive(true);

            option1Button.onClick.AddListener(Option1);
            option2Button.onClick.AddListener(Option2);
        }
        else
        {
        }
    }

    public void Option1()
    {
        if(option1Text.text == action2Info.correctAnswer)
        {
            ObjContainer.NetworkClient.PlayerAction("positive");
        }
        else
        {
            ObjContainer.NetworkClient.PlayerAction("negative");
        }
    }
    public void Option2()
    {
        if (option2Text.text == action2Info.correctAnswer)
        {
            ObjContainer.NetworkClient.PlayerAction("positive");
        }
        else
        {
            ObjContainer.NetworkClient.PlayerAction("negative");
        }
    }
    public void ReciveReward()
    {
        ObjContainer.NetworkClient.PlayerAction("type_1_changes");
    }
}