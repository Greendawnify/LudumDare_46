using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;


    [Header("Is User Hunter")]
    [SerializeField] bool isHunter;
    [SerializeField] Hunter hunterRef;
    public float scopeSensitivity = 150f;

    [Header("Looking around")]
    public float mouseSensitivity = 500f;
    [SerializeField] float minLookUp;
    [SerializeField] float maxLookUp;
    
   

    float xRotation;
    float mouseMoveValue;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mouseMoveValue = mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        // iff scoping half the sensitivity
        if (isHunter)
        {
            if(hunterRef.GetIsScoped())
                mouseMoveValue = scopeSensitivity;
        }
        else {
            mouseMoveValue = mouseSensitivity;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseMoveValue * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseMoveValue * Time.deltaTime;

        //Debug.Log("Mouse values. mouseX = " + mouseX + ". mouseY = " + mouseY);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookUp, maxLookUp);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        
    }
}
