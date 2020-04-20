using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointSystem : MonoBehaviour
{
    public Horse agent;
    public List<Waypoint> waypoints = new List<Waypoint>();
    [HideInInspector] public bool last = false;

    [SerializeField] float movementBetweenWapointWait;


    int positonInList = -1;
    int lastPosition;
    Vector3 nextWaypointPostion;


    // Start is called before the first frame update
    void Start()
    {
        lastPosition = waypoints.Count;
        StartCoroutine(StartWait());
;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveToNewPosition() {
        positonInList++;

        if (positonInList == lastPosition)
        {
            Debug.Log("At the end of the road");
            agent.GoDirectlyToNearBypoint(waypoints[waypoints.Count - 1].nearByPoints);
        }
        else
        {

            agent.SetNextWaypointDestination(waypoints[positonInList]);
        }


    }

    public IEnumerator Wait() {
        Debug.Log("Start wait in waypoint system");
        yield return new WaitForSeconds(movementBetweenWapointWait);
        MoveToNewPosition();
    }

    IEnumerator StartWait() {
        yield return new WaitForSeconds(2f);
        MoveToNewPosition();
    }
}
