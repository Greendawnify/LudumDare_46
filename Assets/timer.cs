using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Mirror.Examples.NetworkRoom;
public class timer : MonoBehaviour
{
    public Text timerText;
    public string TimerTextString;
    public float TimeVal = 3;
    public bool HunterWin, HunterLose;
    public static GameObject YouWin, YouLose;
    public NetworkRoomManagerExt roomManager;
    // Start is called before the first frame update
    void Start()
    {
        YouWin = gameObject.transform.GetChild(0).gameObject;
        YouLose = gameObject.transform.GetChild(1).gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(roomManager == null)
        {
            if(GameObject.Find("RoomManager"))
            roomManager = GameObject.Find("RoomManager").GetComponent<NetworkRoomManagerExt>();
        }
        if (TimeVal >= 0f)
        {
            TimeVal = TimeVal - (.01f * Time.deltaTime);
            timerText.text =( Mathf.Round( TimeVal*100)/100).ToString();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            HunterLose = true;
            HunterWin = false;
           // Time.timeScale = 0;
        }

    }
    public void GoBackToRoom()
    {
        Cursor.lockState = CursorLockMode.None;


        Application.Quit();
        Time.timeScale = 1;
    }
}
