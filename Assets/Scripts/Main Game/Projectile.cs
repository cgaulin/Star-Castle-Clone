using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float coolMin;
    [SerializeField] float coolMax;

    private float cooldown;
    private bool ready = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            FindObjectOfType<Player>().PlayerDeath();
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Collider" && gameObject.tag == "Mine" && ready)
        {
            GetComponent<MineNavigation>().enabled = false;
            StartCoroutine(ResetNavigation());
        }
        else
        {
            return;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "Collider" && gameObject.tag == "Mine")
        {
            ready = true;
        }
    }

    private IEnumerator ResetNavigation()
    {
        cooldown = Random.Range(coolMin, coolMax);
        yield return new WaitForSeconds(cooldown);
        GetComponent<MineNavigation>().enabled = true;
    }
}
