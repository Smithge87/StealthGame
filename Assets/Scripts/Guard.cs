using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public Transform pathHolder;
    public float speed = 5f;
    public float waitTime = .3f;
    public float turnSpeed = 90;

    void Start()
    {
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i=0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            //-- keep the guard on his initial y-axis instead of centering on the waypoint
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
        StartCoroutine(FollowPath(waypoints));

    }

    void Update()
    {
        
    }

    IEnumerator FollowPath (Vector3[] waypoints)
    {
        //-- there's time compexity here and i need to spend some time understanding it... 
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        //-- look at the target from the get-go before starting
        transform.LookAt(targetWaypoint);
        while (true)
        {
            //-- next line is pretty standard. loop below is a brain bender.
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if(transform.position == targetWaypoint)
            {
                //-- modulo below makes the loop start over when it hits zero which is kinda mind blowing
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                //-- after moving, start looking in the direction of the next waypoint
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            //-- advance one per frame
            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        //-- figure out the angle you need to turn
        Vector3 directionToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(directionToLookTarget.z, directionToLookTarget.x) * Mathf.Rad2Deg;
        //-- until the current angle meets the target angle, keep turning - don't use zero because... float stuff. Abs to handle negative angles
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowards(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    //-- Handles drawing the waypoint Gizmos
    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach(Transform waypoint in pathHolder)
        {
            //-- adds spheres to make the waypoints visible, and draws a line to connect them
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        //-- closes the waypoint loop
        Gizmos.DrawLine(previousPosition, startPosition);
    }
}
