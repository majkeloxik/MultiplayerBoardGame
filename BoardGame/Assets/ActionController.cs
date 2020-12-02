using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    public ActionInfo actionInfo;
    
    public Text titleText;
    public Text descriptionText;
    public Text integrateWithText;
    public Text ValueText;
    public Text AttributeText;
    private ObjContainer objContainer;
    private ObjContainer ObjContainer
    {
        get
        {
            return objContainer = (objContainer == null) ? FindObjectOfType<ObjContainer>() : objContainer;
        }
    }
    public void SetActionInfo(int type, int id, int time)
    {
        actionInfo = Resources.Load<ActionInfo>("Action/" + type.ToString() + "/Action " + id.ToString());
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
}
