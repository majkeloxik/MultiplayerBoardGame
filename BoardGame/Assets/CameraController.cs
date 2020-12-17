using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public CameraFolow cameraFolow;
    public float transitionSpeed;
    private ObjContainer objContainer;
    public GameObject cameraResetButtonObj;
    public Button cameraResetButton; 
    private ObjContainer ObjContainer
    {
        get
        {
            return objContainer = (objContainer == null) ? FindObjectOfType<ObjContainer>() : objContainer;
        }
    }
    private void Start()
    {
        /*
        var GameUI = GameObject.FindGameObjectWithTag("MainGameUI");
        RawImage newPlayerInfo;
        newPlayerInfo = Instantiate(Resources.Load<RawImage>("Prefabs/PlayerInfo"), new Vector3(), Quaternion.identity, GameUI.transform);
        newPlayerInfo.transform.localPosition = new Vector3(-800f + 1 * 1600f, 40f, 10f);
        newPlayerInfo.texture = Resources.Load<Texture>("Avatars/avatar2");
        */
    }

    public void SetCamera()
    {
        string actPlayer = ObjContainer.actualPlayer;
        if (ObjContainer.username == actPlayer)
        {
            cameraResetButtonObj.SetActive(true);
            cameraResetButton.interactable = true;
            gameObject.GetComponent<Camera>().enabled = true;
            //ObjContainer.playersList.Where(x => x.name != ObjContainer.actualPlayer).ToList().ForEach(playerObj => playerObj.GetComponent<PlayerController>().playerCamera.enabled = false);
            foreach(var playerObj in ObjContainer.playersList)
            {
                if(playerObj.name != actPlayer)
                {
                    playerObj.GetComponent<PlayerController>().playerCamera.enabled = false;
                }
            }
            cameraFolow.ResetCameraPosition();
            cameraFolow.enabled = true;
            
        }
        else
        {
            cameraResetButtonObj.SetActive(false);
            ObjContainer.actualPlayerObj.GetComponent<PlayerController>().playerCamera.enabled = true;
            cameraFolow.enabled = false;
            gameObject.GetComponent<Camera>().enabled = false;
            cameraResetButton.enabled = false;
            cameraResetButton.interactable = false;
        }
    }
}
