using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CokePickup : MonoBehaviour
{
    CokeSpawn coke;

    void Start()
    {
        coke = FindObjectOfType<CokeSpawn>();
    }

    void Update()
    {
        StopCokeSpawn();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            CokeStuff();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void CokeStuff()
    {
        coke.spawn = true;
        coke.cool = false;
        coke.timer += 10f;
        coke.points += 500;
        coke.counter++;
        coke.audioSource.PlayOneShot(coke.cokePickupAudio);
        FindObjectOfType<Score>().AddCokePoints();
        Destroy(gameObject);
    }

    private void StopCokeSpawn()
    {
        if (coke.counter == 6)
        {
            Destroy(gameObject);
        }
    }
}
