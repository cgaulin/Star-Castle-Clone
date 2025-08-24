using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CokeSpawn : MonoBehaviour
{
    [Header("Box 1")]
    [SerializeField] float topOne;
    [SerializeField] float bottomOne;
    [SerializeField] float leftOne;
    [SerializeField] float rightOne;

    [Header("Box 2")]
    [SerializeField] float topTwo;
    [SerializeField] float bottomTwo;
    [SerializeField] float leftTwo;
    [SerializeField] float rightTwo;

    [Header("Box 3")]
    [SerializeField] float topThree;
    [SerializeField] float bottomThree;
    [SerializeField] float leftThree;
    [SerializeField] float rightThree;

    [Header("Box 4")]
    [SerializeField] float topFour;
    [SerializeField] float bottomFour;
    [SerializeField] float leftFour;
    [SerializeField] float rightFour;

    [Header("Coke Pickup")]
    [SerializeField] GameObject coke;
    public bool spawn = true;
    public bool cool = false;
    public float timer = 10f;
    public int points = 0;
    public int counter = 0;
    public AudioClip cokePickupAudio;
    public AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        PickABox();
    }

    private void PickABox()
    {
        int boxNum = Random.Range(1, 5);

        if (cool)
        {
            switch (boxNum)
            {
                case 1:
                    SpawnInBoxOne();
                    break;

                case 2:
                    SpawnInBoxTwo();
                    break;

                case 3:
                    SpawnInBoxThree();
                    break;

                case 4:
                    SpawnInBoxFour();
                    break;
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0f) cool = true;
        }
    }

    private void SpawnInBoxOne()
    {
        if (spawn)
        {
            float randomNumX = Random.Range(leftOne, rightOne);
            float randomNumY = Random.Range(topOne, bottomOne);
            Instantiate(coke, new Vector2(randomNumX, randomNumY), Quaternion.identity);
            spawn = false;
        }
    }

    private void SpawnInBoxTwo()
    {
        if (spawn)
        {
            float randomNumX = Random.Range(leftTwo, rightTwo);
            float randomNumY = Random.Range(topTwo, bottomTwo);
            Instantiate(coke, new Vector2(randomNumX, randomNumY), Quaternion.identity);
            spawn = false;
        }
    }

    private void SpawnInBoxThree()
    {
        if (spawn)
        {
            float randomNumX = Random.Range(leftThree, rightThree);
            float randomNumY = Random.Range(topThree, bottomThree);
            Instantiate(coke, new Vector2(randomNumX, randomNumY), Quaternion.identity);
            spawn = false;
        }
    }

    private void SpawnInBoxFour()
    {
        if (spawn)
        {
            float randomNumX = Random.Range(leftFour, rightFour);
            float randomNumY = Random.Range(topFour, bottomFour);
            Instantiate(coke, new Vector2(randomNumX, randomNumY), Quaternion.identity);
            spawn = false;
        }
    }
}
