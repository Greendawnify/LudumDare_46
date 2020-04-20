using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Mirror.Examples.NetworkRoom;
using UnityEngine.SceneManagement;

public class gameManagerScript : MonoBehaviour
{
    public static gameManagerScript singleton;
    public GameObject PlayersListParent;
    public GameObject ReadyPlayer2;
    public Text P1readyPanel, P2readyPanel;
    public GameObject PlayButton;
    public NetworkRoomManagerExt networkRoom;
 
    public bool P1isReady, P2isReady;
    public CanvasGroup canvasMain;
    public void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.Contains("Menu")){

            canvasMain.interactable = (true);
            canvasMain.blocksRaycasts = (true);
            canvasMain.alpha = 1f;
        }
        else
        {
            canvasMain.interactable=(false);
            canvasMain.blocksRaycasts=(false);
            canvasMain.alpha = 0f;

        }
        
        
        
            if (networkRoom.allPlayersReady)
            {
            if (NetworkServer.connections.Count == 2)
            {
                PlayButton.SetActive(true);
            }
            }
           /* if (ClientScene.localPlayer.connectionToServer.isReady)
            {
                CmdtoggleP1Ready(true);
            }
            else
            {
                CmdtoggleP1Ready(false);
            }*/
        
    }
    
    public void SetHunter()
    {
        ClientScene.localPlayer.gameObject.GetComponent<PlayerScript>().isHunter= (true);
    }
    public void SetProtector()
    {
        ClientScene.localPlayer.gameObject.GetComponent<PlayerScript>().isHunter = (false);
    }



    public void ChangeToGame()
    {
        networkRoom.ServerChangeScene(networkRoom.GameplayScene);
    }

}
