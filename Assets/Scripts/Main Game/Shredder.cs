using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    public void LaserShredder()
    {
        GameObject[] lasers = GameObject.FindGameObjectsWithTag("Laser");
        for(int i = 0; i < lasers.Length; i++)
        {
            Destroy(lasers[i]);
        }
    }
}
