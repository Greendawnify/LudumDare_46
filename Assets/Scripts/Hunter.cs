using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class Hunter : NetworkBehaviour
{
    [SerializeField] int numberOfShots;

    [Header("Lerping to Sniper")]
    [SerializeField] Animator hunterHeadAnimator;
    bool scoping = false;

    [Header("Shooting Sniper")]
    [SerializeField] Transform raycastPoint;
    [SerializeField] PlayerAudioManager audioManager;
    [SerializeField] LayerMask mask;
    [SerializeField] float shootWait;
    bool startWait = false;
    float waitValue;

    [Header("Horse Interaction")]
    [SerializeField] Horse horse;

    [Header("UI")]
    [SerializeField] Text bulletText;

    public ParticleSystem Tracer;
    public timer Timer;
    public GameObject HunterSpawn;

    public Transform ShotHit;
    public GameObject HitParticle, missParticle;


    // Start is called before the first frame update
    void Start()
    {
        horse = GameObject.FindObjectOfType<Horse>();
        Timer = GameObject.FindObjectOfType<timer>();
        waitValue = shootWait;
        HunterSpawn = GameObject.FindGameObjectWithTag("HunterSpawn");
        gameObject.transform.position = HunterSpawn.transform.position;
        bulletText.text = numberOfShots.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
        {
            return;
        }
        if(numberOfShots <= 0)
        {
            Timer.HunterWin = (false);
            Timer.HunterLose = (true);
        }
        if (Input.GetMouseButtonDown(1)) { // look down scope
            scoping = !scoping;
            Debug.Log("scoping");
            if (scoping)
            {
                hunterHeadAnimator.ResetTrigger("StopAiming");
                hunterHeadAnimator.SetTrigger("Aim");
                audioManager.ZoomIn();
                
            }
            else {
                hunterHeadAnimator.ResetTrigger("Aim");
                hunterHeadAnimator.SetTrigger("StopAiming");
                
            }
        }

        if (Input.GetMouseButtonDown(0) && !startWait && numberOfShots > 0) { // shoot sniper
            startWait = true;
            Debug.Log("Bang");
            
            CmdShoot();
            
            hunterHeadAnimator.SetTrigger("Fire");
            numberOfShots--;
            bulletText.text = numberOfShots.ToString();
            // update some sort of UI to let the player know how many bullets they have left
        }

        if (startWait) {
            waitValue -= Time.deltaTime;
            if (waitValue <= 0f) {
                startWait= false;
                waitValue = shootWait;
            }
        }
        


    }
    [Command]
    public void CmdShoot() {
        RpcShoot();
    }
    [ClientRpc]
    public void RpcShoot()
    {
        audioManager.SniperSound();
        Tracer.Play();
        DoRaycast();
        horse.SetMovementRun();
    }

    void DoRaycast() {
        RaycastHit hit;
        Ray ray = new Ray(raycastPoint.position, -raycastPoint.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 5f);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Collide)) // hit someting with sniper
        {
            ShotHit.position = hit.point;
            if (hit.collider.CompareTag("Shield"))
            {
                Debug.Log("Shield blocked");
                CmdShotShield();
            }
            else if (hit.collider.CompareTag("Protector")) {

                CmdShotProtector();
                Debug.Log("You killed the protector");
            }
            else if(hit.collider.CompareTag("Horse")){
                Debug.Log("You killed the horse");
                CmdShotUni();
            }
            else
            {
                CmdShotShield();
                Debug.Log("I hit " + hit.collider.name + " with my regular sniper round");
            }

        }
        
    }
    [Command]
    public void CmdShotProtector()
    {
        RpcShotProtector();
    }
    [ClientRpc]
    public void RpcShotProtector()
    {
      //  Instantiate(HitParticle, ShotHit.transform.position, Quaternion.identity);
        Timer.HunterWin=(true);
        Timer.HunterLose = (false);
    }
    [Command]
    public void CmdShotUni()
    {
        RpcShotUni();
    }
    [ClientRpc]
    public void RpcShotUni()
    {
       // Instantiate(HitParticle, ShotHit.transform.position, Quaternion.identity);
        Timer.HunterWin = (true);
        Timer.HunterLose = (false);
    }
    [Command]
    public void CmdShotShield()
    {
        
        RpcShotShield();
    }
    [ClientRpc]
    public void RpcShotShield()
    {
       // Instantiate(missParticle, ShotHit.transform.position, Quaternion.identity);
        Debug.Log("plink sound");
    }

    public bool GetIsScoped() {
        return scoping;
    }
}
