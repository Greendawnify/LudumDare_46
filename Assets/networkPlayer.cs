using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class networkPlayer : NetworkBehaviour
{
    public Behaviour[] ComponetsToDisable;

    void Start()
    {
        if (!hasAuthority)
        {

            for (int i = 0; i < ComponetsToDisable.Length; i++)//disableing components is a must such as camera and VRIK because those components get confused and can lead to unwanted behavior such as body sticking
            {
                if (ComponetsToDisable[i] != null)
                {
                    ComponetsToDisable[i].enabled = false;
                }

            }
        }
    }

        // Update is called once per frame
        void Update()
        {

        }
    }

