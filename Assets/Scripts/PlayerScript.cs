using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Mirror.Examples.NetworkRoom;
public class PlayerScript : NetworkBehaviour
{
    public static List<PlayerScript> UserPool = new List<PlayerScript>();//this is the list of all the users
    [SyncVar]
    public  bool isHunter;
    
    public bool ServersRoleHunter;
    [SyncVar]
    public bool isServer;
    public Text Player1Ready, Player2Ready;
    public gameManagerScript gameManager;
    public GameObject PlayerList;
    public GameObject ReadyButtonPrefab;
    private void OnEnable()
    {
        UserPool.Add(this);
    }
    
    private void OnDisable()
    {
        UserPool.Remove(this);
    }
    
    void Start()
    {
        ClientScene.localPlayer.GetComponent<NetworkRoomPlayerExt>().CmdChangeReadyState(true);

      //  gameManager = GameObject.Find("GameManager").gameObject.GetComponent<gameManagerScript>();
        if (isLocalPlayer && ClientScene.localPlayer.isServer)
        {
            isServer = true;
        }
        if (isLocalPlayer) { 
            gameObject.name = gameObject.name + "LocalPlayer";               
        }
        PlayerList = GameObject.Find("PlayerList");
      //  GameObject myReadyButton = Instantiate(ReadyButtonPrefab,PlayerList.transform);
      //  NetworkServer.Spawn(myReadyButton);
        //myReadyButton.GetComponent<NetworkIdentity>().AssignClientAuthority(this.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
        //myReadyButton.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => CmdtoggleReady(true));
        //myReadyButton.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => CmdtoggleReady(false));
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isServer)
        {
            foreach(PlayerScript players in UserPool)
            {
                if(players != this)
                {
                    if(players.isHunter)
                    {
                        isHunter = false;
                    }
                    else
                    {
                        isHunter = true;
                    }
                }
            }
        }
    }
    
    [Command]
    public void CmdtoggleReady(bool isReady)
    {

        RpcToggleReady(isReady);
    }
    [ClientRpc]
    public void RpcToggleReady(bool isReady)
    {
        if (isReady)
        {
           // gameManager.P1isReady = true;
        }
        else
        {
         //   gameManager.P1isReady = false;
        }
    }
    

}
