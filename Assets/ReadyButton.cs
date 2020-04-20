using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Mirror.Examples.NetworkRoom;
public class ReadyButton : NetworkBehaviour
{
    public Text readyText;
    public GameObject Ready, Cancel;
    public gameManagerScript gameManager;
    public void Start()
    {
        gameManager = GameObject.Find("GameManager").gameObject.GetComponent<gameManagerScript>();
    }
    public void Update()
    {
        if(!hasAuthority)
        {
            Ready.GetComponent<Button>().interactable =(false);
            Cancel.GetComponent<Button>().interactable = (false);
        }
        else
        {
            Ready.GetComponent<Button>().interactable = (true);
            Cancel.GetComponent<Button>().interactable = (true);
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
        if(isReady)
        {
            readyText.text = "Player:  Ready";
            ClientScene.localPlayer.GetComponent<NetworkRoomPlayerExt>().CmdChangeReadyState(true);

        }
        else
        {
            readyText.text = "Player:  Not Ready";
            ClientScene.localPlayer.GetComponent<NetworkRoomPlayerExt>().CmdChangeReadyState(false);
        }
    }
}
