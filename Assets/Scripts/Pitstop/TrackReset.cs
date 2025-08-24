using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class TrackReset : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lapNumberText;
    [SerializeField] private Transform startTrackTrigger;
    [SerializeField] private GameObject player;
    [SerializeField] private CheckpointManager checkpointManager;

    private int lapNumber = 1;
    private const int maxLaps = 3;

    private void Update()
    {
        lapNumberText.text = lapNumber.ToString();
        FinishRace();
    }

    private void IncrementLapCount() { lapNumber++; }

    public bool FinishRace()
    {
        if (lapNumber == maxLaps + 1) { return true; }
        else { return false; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        player.transform.position = startTrackTrigger.position;

        if (checkpointManager.totalCheckpointsRemaining <= 0)
        {
            IncrementLapCount();
            checkpointManager.ResetCheckpoints();
        }
        
    }
}
