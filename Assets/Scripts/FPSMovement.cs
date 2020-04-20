using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class FPSMovement : NetworkBehaviour
{

    [SerializeField] CharacterController controller;
    [SerializeField] Animator anim;
    NetworkAnimator nanim;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject headObject;
    [SerializeField] PlayerAudioManager audioManager;

    Hunter hunterRef;


    [Header("Movement Speeds")]
    public float speed = 12f;
    public float runningSpeed;
    public float crouchingSpeed;
    public float jumpForce;
    bool isRunning = false;

    //[Header("Audio Seeds")]
    //[SerializeField] float crouchStepPace;
    //[SerializeField] float walkStepPace;
    //[SerializeField] float runStepPace;
    //int pace = 1; // 0 = crouch, 1 = walk, 2 = run

    [Header("Crouch Camera Lerp")]
    [SerializeField] Transform crouchHeadPosition;
    [SerializeField] Transform standingHeadPosition;
    //float startLerpTime;
    //float journeyLength;
    //public Transform standPosition, crouchPosition;


    public float gravity = -9.81f;
    public float jumpHeight;
    public float groundDistance;
    
    

    bool isGrounded = false;
    bool isCrouched = false;
    Vector3 velocity;

    void Start()
    {
        if (GetComponent<Hunter>()) {
            hunterRef = GetComponent<Hunter>();
        }
        nanim = gameObject.GetComponent<NetworkAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
        {
            return;
        }
        isRunning = false;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // crouched input
        if (Input.GetKeyDown(KeyCode.LeftControl) && hunterRef == null) {
            isCrouched = !isCrouched;

            if (isCrouched)
            {
                headObject.transform.localPosition = crouchHeadPosition.localPosition;
               nanim.animator.SetBool("isCrouched", true);
                
            }
            else {
                headObject.transform.localPosition = standingHeadPosition.localPosition;
                nanim.animator.SetBool("isCrouched", false);
            }
            

        }

        if (isCrouched || (hunterRef != null && hunterRef.GetIsScoped()))
        {
            controller.Move(move * crouchingSpeed * Time.deltaTime);
            // make sure to switch colliders to smaller one.
        }
        else
        {

            // the player is holding the running button down or not
            if (Input.GetKey(KeyCode.LeftShift)) // running
            {

                if (isGrounded)
                {// running on the ground
                    controller.Move(move * runningSpeed * Time.deltaTime);
                }
                else
                {// running in the air
                    controller.Move(move * crouchingSpeed * Time.deltaTime);
                }
                isRunning = true;
            }
            else
            {// not running
                if (isGrounded)
                {// not running on ground
                    controller.Move(move * speed * Time.deltaTime);
                }
                else
                {// not running in air
                    controller.Move(move * crouchingSpeed * Time.deltaTime);
                }
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouched) {
            velocity.y = Mathf.Sqrt(jumpHeight * -jumpForce * gravity);
            nanim.SetTrigger("Jump");
            StartCoroutine(ResetJumpTrigger());
        }

        if (x != 0f || z != 0f)
        {
            // character is moving
            Debug.Log("Character is moving");
            if (isRunning)
               nanim.animator.SetFloat("speed", 1f);
            else {
                nanim.animator.SetFloat("speed", 0.5f);
            }
        }
        else {
            nanim.animator.SetFloat("speed", 0f);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator ResetJumpTrigger() {
        yield return new WaitForSeconds(0.25f);
        nanim.ResetTrigger("Jump");
    }
}
