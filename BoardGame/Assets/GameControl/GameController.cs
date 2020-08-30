using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{


    List<Vector3> Points = new List<Vector3>();
    public List<Player> Player2 = new List<Player>();
    public Transform Player;
    private int PlayerIndexPosition;
    private int wyrzuconaWartosc;
    public Vector3 pointMovetTo;
    private int wartoscWprzod;
    private int wartoscWtyl;
    public void DoSomethingToClicked(Vector3 pozycja, int index)
    {

        wartoscWprzod = PlayerIndexPosition + wyrzuconaWartosc;
        wartoscWtyl = PlayerIndexPosition - wyrzuconaWartosc;

        if ((wartoscWtyl) < 0)
        {
            wartoscWtyl += 16;
        }
        if ((wartoscWprzod) > 15)
        {
            wartoscWprzod -= 16;
        }
        if (index == wartoscWtyl || index == wartoscWprzod)
        {

            pointMovetTo = pozycja;
            Debug.Log("Index aktualnego pola: " + index);
            Debug.Log("Kordy aktualnego pola: " + pointMovetTo);
            Player.position = (pointMovetTo);
            PlayerIndexPosition = index;
        }
        else
        {
            Debug.Log("Nie mozesz sie ruszyc na to pole");
        }
        // Do things with clickedGameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerIndexPosition = 0;
        Debug.Log(wyrzuconaWartosc);
        Player = Player.GetComponent<Transform>();
        for (int i = 0; i < 16; i++)
        {
            // Debug.Log(this.gameObject.transform.GetChild(i).transform.position);
            Points.Add(this.gameObject.transform.GetChild(i).transform.position);
        }
    }


    public void rzutKostka()
    {
        int rnd = Random.Range(1, 7);
        wyrzuconaWartosc = rnd;
        Debug.Log("Wyrzucono : " + wyrzuconaWartosc);
    }

    // Update is called once per frame
    void Update()
    {

    }


}
