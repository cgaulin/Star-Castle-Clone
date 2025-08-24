using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private GameObject[] checkpoints;
    public int totalCheckpointsRemaining;

    // Start is called before the first frame update
    void Start()
    {
        totalCheckpointsRemaining = checkpoints.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetCheckpoints()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].SetActive(true);
            totalCheckpointsRemaining = checkpoints.Length;
        }
    }
}
