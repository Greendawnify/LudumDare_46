using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.NetworkRoom;
public class playerParent : NetworkBehaviour
{
    [SyncVar]
    public bool isHunter;
    public GameObject HunterPrefab, ProtectorPrefab, Hunter, Protector;

    public bool isLocalPlayerbool;
    public GameObject Character;
    public NetworkRoomPlayerExt roomPlayer;
    public NetworkRoomManagerExt roomManager;
    public bool PlayerSpawned= false;
    public timer Timer;
    public Animator ProtectorAnim, HunterAnim;
   // public NetworkAnimator nanim;
    public void Start()
    {
        
        Timer = GameObject.Find("Timer").GetComponent<timer>();
        roomManager = GameObject.Find("RoomManager").GetComponent<NetworkRoomManagerExt>();
        //nanim = gameObject.GetComponent<NetworkAnimator>();
       // nanim.animator = ProtectorAnim;
        if (isLocalPlayer)
        {

            foreach (NetworkRoomPlayer players in roomManager.roomSlots)
            {
                if (players.gameObject.GetComponent<NetworkIdentity>().hasAuthority)
                {
                    isHunter = players.gameObject.GetComponent<PlayerScript>().isHunter;
                    CmdisHunter(isHunter);
                }
            }
            
            
            //CmdParentThem();
        }
           // StartCoroutine(SpawnPlayer());
    }
    [Command]
    public void CmdSetAnimatorPro(GameObject ClientPlayer,bool isServerBool)
    {
        if (isServerBool)
        {
            RpcSetAnimatorPro();
        }
        else
        {
            Protector = Instantiate(ProtectorPrefab);
            NetworkServer.Spawn(Protector, ClientPlayer.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
            Protector.transform.position = GameObject.Find("ProtectorSpawn").transform.position;
        }
    }
    [ClientRpc]
    public void RpcSetAnimatorPro()
    {

        Protector = Instantiate(ProtectorPrefab);
        NetworkServer.Spawn(Protector, connectionToClient);
        Protector.transform.position = GameObject.Find("ProtectorSpawn").transform.position;
    }
    [Command]
    public void CmdSetAnimatorHunt(GameObject ClientPlayer, bool isServerBool)
    {

        if (isServerBool)
        {
            RpcSetAnimatorHunt();
        }
        else
        {
            Hunter = Instantiate(HunterPrefab);
            NetworkServer.Spawn(Hunter, ClientPlayer.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
            Hunter.transform.position = GameObject.Find("HunterSpawn").transform.position;
        }
    }
    [ClientRpc]
    public void RpcSetAnimatorHunt()
    {
        Hunter = Instantiate(HunterPrefab);
        NetworkServer.Spawn(Hunter, connectionToClient);
        Hunter.transform.position = GameObject.Find("HunterSpawn").transform.position;
    }

   
    
    // Update is called once per frame
    void Update()
    {
        if (!Hunter)
        {
            if (GameObject.FindObjectOfType<Hunter>())
            {
                Hunter = GameObject.FindObjectOfType<Hunter>().gameObject;
            }
        }
        if (!Protector)
        {
            if (GameObject.FindObjectOfType<Protector>())
            {
                Protector = GameObject.FindObjectOfType<Protector>().gameObject;
            }
        }
        
        if (!PlayerSpawned)
        {
           
                if (isLocalPlayer)
                {
                    if (isHunter)
                    {
                    if (isServer) {
                        CmdSetAnimatorHunt(this.gameObject,true);
                    }
                    else
                    {
                        CmdSetAnimatorHunt(this.gameObject, false);
                    }
                       
                    }
                    else
                    {
                    if (isServer)
                    {
                        CmdSetAnimatorPro(this.gameObject,false);
                    }
                    else
                    {
                        CmdSetAnimatorPro(this.gameObject,false);
                    }
                    }
                    PlayerSpawned = true;
                    Debug.Log("YAY");
                }
            
        }
            if (isHunter)
            {
               //  Hunter.SetActive(true);
                // Protector.SetActive(false);
            if (isLocalPlayer)
            {
                if (Timer)
                {
                    if (Timer.HunterLose)
                    {
                        timer.YouLose.SetActive(true);
                    }
                    if (Timer.HunterWin)
                    {
                        timer.YouWin.SetActive(true);
                    }
                }
            }
            }
            else
            {
               // Protector.SetActive(true);
               // Hunter.SetActive(false);
            if (isLocalPlayer)
            {
                if (Timer)
                {
                    if (Timer.HunterLose)
                    {
                        timer.YouWin.SetActive(true);
                    }
                    if (Timer.HunterWin)
                    {
                        timer.YouLose.SetActive(true);
                    }
                }
            }
            }
       
       
        if (hasAuthority)
        {
            isLocalPlayerbool = true;
        }
    }

    [Command]
    public void CmdisHunter(bool isHunterBool)
    {
        RpcIsHunter(isHunterBool);
     }
    [ClientRpc]
    public void RpcIsHunter(bool isHunterBool)
    {
        isHunter = isHunterBool;
    }
    [Command]
    public void CmdChangePower()
    {
        RpcChangePower();
    }
    [ClientRpc]
    public void RpcChangePower()
    {
        Protector.GetComponent<Protector>().ChangePower();
    }
    [Command]
    public void CmdlaserPointSet(bool setActiveBool)
    {
        RpclaserPointSet(setActiveBool);
    }
    [ClientRpc]
    public void RpclaserPointSet(bool setActiveBool)
    {
        Protector.GetComponent<Protector>().laserPointSet(setActiveBool);

    }
    [Command]
    public void CmdThrowShield()
    {
        RpcThrowSheild();
    }
    [ClientRpc]
    public void RpcThrowSheild()
    {
        Protector.GetComponent<Protector>().ThrowShield();
    }
    
   [Command]
    public void CmdCreateDecoy()
    {
       RpcCreateDecoy();
    }
    [ClientRpc]
    public void RpcCreateDecoy()
    {
        Protector.GetComponent<Protector>().CreateDecoy();
    }
}
