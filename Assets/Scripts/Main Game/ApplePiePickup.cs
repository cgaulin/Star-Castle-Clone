using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplePiePickup : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip piePickup;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(piePickup);
            Destroy(gameObject);
        }
    }
}
