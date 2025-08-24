using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererHandler : MonoBehaviour
{
    [SerializeField] private float trailRenderBufferX = 5f;
    [SerializeField] private float trailRenderBufferY = 4f;

    //Components
    private Player player;
    private TrailRenderer trailRenderer;

    void Awake()
    {
        player = GetComponentInParent<Player>();
        trailRenderer = GetComponent<TrailRenderer>();

        //Set trail renderer to not emit in the start
        trailRenderer.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If the car tires are screeching then we'll emitt a trail.
        if (player.IsTireScreeching(out float lateralVelocity) && 
            (player.transform.position.x < trailRenderBufferX && 
            player.transform.position.x > -trailRenderBufferX) && 
            (player.transform.position.y < trailRenderBufferY &&
            player.transform.position.y > -trailRenderBufferY))
        {
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }
}
