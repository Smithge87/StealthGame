using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public Transform pathHolder;

    void Start()
    {
        
    }

    void Update()
    {
        
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
