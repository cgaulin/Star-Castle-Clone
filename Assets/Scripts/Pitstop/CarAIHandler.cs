using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarAIHandler : MonoBehaviour
{
    public enum AIMode { followPlayer, followWaypoints };

    [Header("AI Settings")]
    public AIMode aiMode;
    public float maxSpeed = 4f;

    //Local Variables
    Vector3 targetPosition = Vector3.zero; 
    Transform targetTransform = null;

    //Components
    TopDownCarController topDownCarController;

    //Waypoints
    WaypointNode currentWaypoint = null;
    WaypointNode[] allWaypoints;

    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
        allWaypoints = FindObjectsOfType<WaypointNode>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame and is frame dependent
    void FixedUpdate()
    {
        Vector2 inputVector = Vector2.zero;

        switch (aiMode)
        {
            case AIMode.followPlayer:
                FollowPlayer();
                break;

            case AIMode.followWaypoints:
                FollowWaypoints();
                break;
        }

        inputVector.x = TurnTowardTarget();
        inputVector.y = ApplyThrottleOrBrake(inputVector.x);

        //Send the input to the car controller
        topDownCarController.SetInputFactor(inputVector);
    }

    private void FollowPlayer()
    {
        if (targetTransform == null)
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (targetTransform != null)
            targetPosition = targetTransform.position;
    }

    private void FollowWaypoints()
    {
        //Pick the closest waypoint if we don't have waypointset
        if (currentWaypoint == null)
        {
            currentWaypoint = FindClosestWaypoint();
        }

        //Set target on waypoint's position
        if (currentWaypoint != null)
        {
            targetPosition = currentWaypoint.transform.position;

            //Store how close we are to target
            float distanceToWaypoint = (targetPosition - transform.position).magnitude;

            //Check if we are close enough to consider we have reached the waypoint
            if (distanceToWaypoint <= currentWaypoint.minDistanceToReachWaypoint)
            {
                if (currentWaypoint.maxSpeed > 0f)
                    maxSpeed = currentWaypoint.maxSpeed;
                else maxSpeed = 32f; //Just a high number

                //If there are multiple waypoints connected to one, it will pick one of the points at random
                currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0, currentWaypoint.nextWaypointNode.Length)]; 
            }
        }
    }

    //Find closest waypoint node to AI
    private WaypointNode FindClosestWaypoint()
    {
        return allWaypoints
        .OrderBy(t => Vector3.Distance(transform.position, t.transform.position))
        .FirstOrDefault();
    }

    private float TurnTowardTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize(); //We only care about the direction

        //Calclulate an angle towards the target
        float angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1;

        //Want car to turn as much as possible if the angle is greater than 45 degrees and not turn as much if less than 45 degrees
        float steerAmount = angleToTarget / 45.0f;

        //Clamp steering to between -1 and 1
        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        return steerAmount;
    }

    private float ApplyThrottleOrBrake(float inputX)
    {
        //If we are going too fast, do not accelerate further
        if (topDownCarController.GetVelocityMagnitude() > maxSpeed) return 0f;

        //Apply throttle forward based on how much the car wants to turn. If it's a sharp turn this will cause the car to apply less speed forward
        return 1.05f - Mathf.Abs(inputX) / 1.0f;
    }
}
