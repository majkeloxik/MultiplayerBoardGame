using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private GameController control;
    void Start()
    {
        control = GameObject.Find("GameController").GetComponent<GameController>();
    }// end start
    void OnMouseUp()
    {
        control.DoSomethingToClicked(this.transform.position, transform.GetSiblingIndex());

    }//end OnMouseUp
     // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

    }
}
