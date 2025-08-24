using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    //Max speed allowed when passing this waypoint
    [Header("Speed set once we reach the waypoint")]
    public float maxSpeed = 0f;

    [Header("This is the waypoint we are going towards, not yet reached")]
    public float minDistanceToReachWaypoint = 5f;

    public WaypointNode[] nextWaypointNode;
    public GameObject nextWaypointNodeGameObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Collider")
        {
            nextWaypointNodeGameObject.SetActive(true);
        }
    }
}
