using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private CheckpointManager checkpointManager;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Collider") //Referring to the car collider
        {
            gameObject.SetActive(false);
            checkpointManager.totalCheckpointsRemaining--;
        }
    }
}
