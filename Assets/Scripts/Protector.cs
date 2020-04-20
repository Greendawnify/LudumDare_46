using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public enum ProtectorPower { LASER, SHIELD, DECOY, COUNT };

public class Protector : NetworkBehaviour
{
    [SerializeField] Transform leftArm;
    [SerializeField] Animator anim;
    NetworkAnimator nanim;
    public ProtectorPower power = ProtectorPower.LASER;
    int powerInt = 0;


    [Header("Shield Throwing")]
    [SerializeField] int numberOfShields;
    [SerializeField] GameObject shieldPowerIndicator;
    [SerializeField] GameObject shildLandingObject;
    [SerializeField] GameObject shieldThrowablePrefab;
    [SerializeField] float speed;
    [SerializeField] float maxParabolaHeight;
    [SerializeField] float angleChangeSpeed;
    float gravity;
    Vector3 velocityForce;

    [Header("Laser")]
    [SerializeField] GameObject laserPowerIndicator;
    [SerializeField] GameObject laserPointDecal;
    [SerializeField] Transform raycsatPosition;
    [SerializeField] LayerMask mask;
    [SerializeField] Horse horse;
    Vector3 laserPointPlace = Vector3.zero;

    [Header("Decoy")]
    [SerializeField] int numberOfDecoys;
    [SerializeField] GameObject decoyPrefab;
    [SerializeField] Transform decoySpawnPos;

    [Header("UI")]
    [SerializeField] Text abilitiesText;
    [SerializeField] Text decoyAmmoText;
    [SerializeField] Text shieldAmmoText;

    public playerParent PlayerParent;
    public GameObject ProtectorSpawn;

    // either a line render or a light

    // Start is called before the first frame update
    void Start()
    {
        nanim = gameObject.GetComponent<NetworkAnimator>();
        horse = GameObject.Find("Unnicorn").gameObject.GetComponent<Horse>();
        ProtectorSpawn = GameObject.FindGameObjectWithTag("ProtectorSpawn");
        gameObject.transform.position = ProtectorSpawn.transform.position;

        // setting up ui
        abilitiesText.text = power.ToString();
        decoyAmmoText.text = numberOfDecoys.ToString();
        shieldAmmoText.text = numberOfShields.ToString();
    }
    
    public void ChangePower()
    {
        power++;
        if ((int)power > 2)
        {
            power = 0;
        }
        TurnOffPowerIndicators();

        if (power == ProtectorPower.LASER)
        {

            laserPowerIndicator.SetActive(true);
        }
        else if (power == ProtectorPower.SHIELD)
        {
            shieldPowerIndicator.SetActive(true);
        }
        else if (power == ProtectorPower.DECOY)
        {

        }
        abilitiesText.text = power.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
        {
            return;
        }
        PlayerParent = ClientScene.localPlayer.gameObject.GetComponent<playerParent>();
        
        if (Input.GetKeyDown(KeyCode.E)) {
            PlayerParent.CmdChangePower();
            
        }

        if (Input.GetMouseButtonDown(0)) {
            switch (power) {
                case ProtectorPower.LASER:
                    //horse.SetMovemnentPointer(true, laserPointDecal.transform);
                    PlayerParent.CmdlaserPointSet(true);
                    
                    break;
                case ProtectorPower.SHIELD:
                    nanim.SetTrigger("Throw");
                    shildLandingObject.SetActive(true);
                    break;
                case ProtectorPower.DECOY:
                    if (numberOfDecoys > 0)
                    {
                        // create the decoy and make him run away
                        nanim.SetTrigger("Throw");

                        PlayerParent.CmdCreateDecoy();
                        numberOfDecoys--;
                        decoyAmmoText.text = numberOfDecoys.ToString();
                    }
                    break;
            }
        }

        if (Input.GetMouseButton(0)) {
            switch (power) {
                case ProtectorPower.LASER:
                    LaserPointRaycast();
                    break;
                case ProtectorPower.SHIELD:
                    UpdateShieldThrow();
                    break;
                case ProtectorPower.DECOY:
                    break;
            }
        }


        if (Input.GetMouseButtonUp(0)) {

            switch (power) {
                case ProtectorPower.LASER:
                    //horse.SetMovemnentPointer(false, null);
                    PlayerParent.CmdlaserPointSet(false);
                    //laserPointDecal.SetActive(false);
                    horse.LaserOff();
                    break;
                case ProtectorPower.SHIELD:
                    if (numberOfShields > 0)
                    {
                        // throw sheild
                        PlayerParent.CmdThrowShield();
                        // ThrowShield();
                        shildLandingObject.SetActive(false);
                        numberOfShields--;
                        shieldAmmoText.text = numberOfShields.ToString();
                    }
                    break;
                case ProtectorPower.DECOY:
                    break;
            }

        }
    }
   
    
    public void laserPointSet(bool setActiveBool)
    {
        if (setActiveBool)
        {
            laserPointDecal.SetActive(true);
        }
        else
        {
            laserPointDecal.SetActive(false);
        }
    }
    void UpdateShieldThrow() {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel > 0f)
        {
            shildLandingObject.transform.localPosition += new Vector3(0f, 0f, angleChangeSpeed * Time.deltaTime);
        }
        else if (scrollWheel < 0f)
        {
            shildLandingObject.transform.localPosition -= new Vector3(0f, 0f, angleChangeSpeed * Time.deltaTime);

        }

        shildLandingObject.transform.localPosition = new Vector3(0f, shildLandingObject.transform.localPosition.y, Mathf.Clamp(shildLandingObject.transform.localPosition.z, 5f, 10f));
    }

    public void CreateDecoy() {
        Instantiate(decoyPrefab, decoySpawnPos.position, transform.rotation);
    }

    public void ThrowShield() {
        GameObject newShield = Instantiate(shieldThrowablePrefab, leftArm.position, leftArm.rotation);
        //NetworkServer.Spawn(newShield, connectionToClient);
        if (BallasticMovement.solveBallisticArc(leftArm.position, speed, shildLandingObject.transform.position, 
            maxParabolaHeight, out velocityForce, out gravity)) {

            var projectile = Instantiate(shieldThrowablePrefab);
            var motion = projectile.GetComponent<ProjectileMotion>();
            motion.Initialize(leftArm.position, gravity);

            motion.AddImpulse(velocityForce);
        }


    }

    void LaserPointRaycast() {
        RaycastHit hit;
        Ray ray = new Ray(raycsatPosition.position, raycsatPosition.forward);
        Debug.DrawRay(ray.origin, ray.direction * 30f, Color.cyan, .5f);
        if (Physics.Raycast(ray, out hit, 30f, mask)) {
            // check if it is the ground layer
            if (hit.collider.CompareTag("Ground")) {
                // hitting the terrain

                horse.LaserOn();
            }
        }

    }

    void TurnOffPowerIndicators() {
        laserPowerIndicator.SetActive(false);
        shieldPowerIndicator.SetActive(false);
    }
}
