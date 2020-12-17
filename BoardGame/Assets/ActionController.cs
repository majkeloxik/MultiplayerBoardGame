using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    public Action1Info actionInfo;
    
    public Text titleText;
    public Text descriptionText;
    public Text integrateWithText;
    public Text ValueText;
    public Text AttributeText;
    public Text Option1;
    public Text Option2;
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
    private void Start()
    {
        option1Button.GetComponentInChildren<Text>().text = "";
    }
    public void SetActionInfo(int type, int id, int time)
    {
        actionInfo = Resources.Load<Action1Info>("Action/" + type.ToString() + "/Action " + id.ToString());
        SetActionValues(time);
    }
    public void SetActionValues(int time)
    {
        actionInfo.integrateWith.Add(ObjContainer.actualPlayer);
        titleText.text = actionInfo.title;
        descriptionText.text = actionInfo.description;
        integrateWithText.text = actionInfo.integrateWith[0].ToString();
        ValueText.text = actionInfo.value[0].ToString();
        
        AttributeText.text = actionInfo.attribute[0].ToString();
        gameObject.SetActive(true);

        StartCoroutine(OnEnableCoroutine(time));

    }
    IEnumerator OnEnableCoroutine(int time)
    {
        yield return new WaitForSeconds(time);
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
        //TODO: handling when action type is fight
    }
    public void SetActionUI(int actionType)
    {
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);

        if (actionType == 1)
        {
            option1Button.gameObject.SetActive(true);
            option1Button.transform.localPosition = new Vector3(0f, 160f, 0f);
        }
        else if(actionType == 2)
        {
            option1Button.transform.localPosition = new Vector3(-150f, 160f, 0f);
            option2Button.transform.localPosition = new Vector3(150f, 160f, 0f);
            option1Button.gameObject.SetActive(true);

            option1Button.onClick.RemoveAllListeners();
            option1Button.onClick.AddListener(test);
        }
        else
        {

        }
    }
    public void test()
    {

    }
}
