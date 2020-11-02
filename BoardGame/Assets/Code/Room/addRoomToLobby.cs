using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class addRoomToLobby : MonoBehaviour
{
    public Text start_text;
    public List<Text> text_list;
    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject text_obj = new GameObject(i.ToString());
            text_obj.transform.SetParent(this.transform);
            text_obj.AddComponent<Text>().text = i.ToString() + "TEXT";
            text_obj.GetComponent<Text>().color= Color.black;
            text_obj.GetComponent<Text>().font = start_text.font;

            Vector3 pos = start_text.GetComponent<Text>().rectTransform.position;
            text_obj.GetComponent<Text>().rectTransform.position = new Vector3(pos.x, pos.y - 30, pos.z);
            start_text = text_obj.GetComponent<Text>();
        }

    }
}
