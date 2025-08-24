using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour
{
    [SerializeField] BoxCollider2D bc;
    public bool goToNextLevel = false;

    public void DisableDoorEntry()
    {
        bc.enabled = false;
    }

    public void EnableDoorEntry()
    {
        bc.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) goToNextLevel = true;
    }
}
