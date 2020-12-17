using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class setActiveTEST : MonoBehaviour
{

    private void Start()
    {
        int nb = 5;
        int typeID = Convert.ToInt32(nb.ToString());
        Debug.Log(typeID);

    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.position);
        var tmp = collision.transform.position;
        transform.position = tmp * 2;
    }
    private void OnTriggerEnter(Collider other)
    {
        var tmp = other.transform.position;
        transform.position = -tmp;
    }
}
