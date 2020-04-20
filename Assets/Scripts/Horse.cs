using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum HorseMovement { ROAM, FOLLOW, RUN, POINTER, COUNT } 
public class Horse : MonoBehaviour
{
    public HorseMovement movementType = HorseMovement.ROAM;
    [SerializeField] Protector protector;
    [SerializeField] float runSpeed;
    [SerializeField] float roamSpeed;
    [SerializeField] Animator anim;

    NavMeshAgent agent;
    Rigidbody rb;
    AudioSource source;

    Vector3 dest = Vector3.zero;
    int waypointCount = -1;

    [Header("Moving to Waypoints")]
    [SerializeField] WaypointSystem waypointSystem;
    Waypoint waypointRef;
    bool headedToNextWaypoint = false;

    [Header("Roaming")]
    List<Transform> nearByPoints = new List<Transform>();
    bool headedToNearByPoint = false;
    bool isRunning = false;

    [Header("Pointer")]
    [SerializeField] float maxDistanceToNoticePointer;
    [SerializeField] float pointerSpeed;
    bool followingLaser = false;

    // Start is called before the first frame update

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        agent.Warp(transform.position);
        
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (protector == null) {
            protector = FindObjectOfType<Protector>();
        }
        if (movementType == HorseMovement.ROAM)
        {
            // headed to new waypoint
            if (headedToNextWaypoint)
            {
                if (Vector3.Distance(transform.position, dest) <= 5f)
                {
                    // at next waypoint
                    headedToNextWaypoint = false;
                    agent.speed = roamSpeed;
                    // start waiting process of going to the near by areas
                    StartCoroutine(waypointSystem.Wait());
                }
            }
            else
            { // roaming to near by points
                Roam();


            }
        }
        else if (movementType == HorseMovement.RUN)
        { // called when the hunter shoots at the horse
            // go to next waypoint
            StopCoroutine(waypointSystem.Wait());
            agent.speed = runSpeed;
            waypointSystem.MoveToNewPosition();
            movementType = HorseMovement.ROAM;
        }
        else if (movementType == HorseMovement.POINTER) {
            transform.LookAt(protector.transform);


        }
    }

    private void FixedUpdate()
    {
        if (movementType == HorseMovement.POINTER) {
            
        }
    }

    void Roam() {
        if (!headedToNearByPoint) {
            headedToNearByPoint = true;
            SetNextNearBypoint();
            StartCoroutine(Wait());
            anim.SetFloat("speed", 0.5f);
        }
        else {
            if (Vector3.Distance(transform.position, dest) <= 2f) {
                // at a near by position
                anim.SetFloat("speed", 0f);
                // pick up animations

            }
        }

    }

    public void SetNextWaypointDestination(Waypoint point) {
        waypointCount++;
        Debug.Log("Heading to Waypoint " + waypointCount);
        StopCoroutine(Wait());
        headedToNextWaypoint = true;
        headedToNearByPoint = false;
        waypointRef = point;

        // go to waypoint
        dest = point.transform.position;
        agent.Warp(transform.position);
        agent.SetDestination(dest);
        nearByPoints.Clear();
        nearByPoints = point.nearByPoints;

        if (!isRunning)
        {
            anim.SetFloat("speed", 0.5f);
        }
        else {
            anim.SetFloat("speed", 1f);
        }
    }

    public void SetMovementRun() {// called by Hunter

        if (!waypointSystem.last && !headedToNextWaypoint)
        {
            Debug.Log("Hunter is shooting at me. So I am running awway");
            movementType = HorseMovement.RUN;
            isRunning = true;
            anim.SetFloat("speed", 1f);
        }
    }

    public void LaserOn() {
        float dist = Vector3.Distance(transform.position, protector.transform.position);
        Debug.Log("ughghgh");

        if (dist <= maxDistanceToNoticePointer && !followingLaser)
        {
            // horse is close enough to see pointer and go there
            followingLaser = true;
            StopCoroutine(Wait());
            StopCoroutine(waypointSystem.Wait());
            headedToNearByPoint = true;
            headedToNextWaypoint = false;

            movementType = HorseMovement.POINTER;
            agent.SetDestination(protector.transform.position);
            anim.SetFloat("speed", 0.5f);
        }
        else if (dist > maxDistanceToNoticePointer && followingLaser) {
            // horse was following laser but then got moved out of range
            followingLaser = false;
            headedToNextWaypoint = false;
            headedToNearByPoint = false;
            StartCoroutine(Wait());
            StartCoroutine(waypointSystem.Wait());

            movementType = HorseMovement.ROAM;
            agent.SetDestination(waypointRef.transform.position);
            anim.SetFloat("speed", 0.5f);
        }

    }

    public void LaserOff() {
        float dist = Vector3.Distance(transform.position, protector.transform.position);

        if (dist <= maxDistanceToNoticePointer && followingLaser) {
            followingLaser = false;
            headedToNextWaypoint = false;
            headedToNearByPoint = false;
            StartCoroutine(Wait());
            StartCoroutine(waypointSystem.Wait());

            movementType = HorseMovement.ROAM;
            agent.SetDestination(waypointRef.transform.position);
            anim.SetFloat("speed", 0.5f);
        }
    }

    void SetNextNearBypoint() 
    {
        int choice = Random.Range(0, nearByPoints.Count);
        Debug.Log("Choice is =" + choice);
        dest = nearByPoints[choice].position;
        agent.SetDestination(dest);
        isRunning = false;
        anim.SetFloat("speed", 0.5f);
    }

    public void GoDirectlyToNearBypoint(List<Transform> list) {
        nearByPoints.Clear();
        nearByPoints = list;
        SetNextNearBypoint();
    }



    IEnumerator Wait() {
        yield return new WaitForSeconds(4f);
        headedToNearByPoint = false;
    }
}
