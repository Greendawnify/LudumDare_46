using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class ServerButton : MonoBehaviour
    {
        
        public Text buttonText;
        public string ServerName = "  Server Found: ";

        void Start()
        {
            buttonText.text = ServerName;
        }

        public void ChangeName(string servername)
        {
            buttonText.text = ServerName + servername;
        }

    }
