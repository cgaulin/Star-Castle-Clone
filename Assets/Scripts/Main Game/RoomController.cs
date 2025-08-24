using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [Header("Wall Stuff")]
    public GameObject[] walls;
    [SerializeField] GameObject wallWithDoor;
    [SerializeField] float wallCooldown = 1f;
    [SerializeField] float doorCooldown = 1f;

    [Header("Enemy")]
    [SerializeField] Enemy enemy;

    void Start()
    {

    }

    void Update()
    {
        StartCoroutine(WallsAppear());
    }

    private IEnumerator WallsAppear()
    {
        if (enemy.isDestroyed)
        {
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].SetActive(true);
                yield return new WaitForSeconds(wallCooldown);
            }

            wallWithDoor.SetActive(true);
            yield return new WaitForSeconds(doorCooldown);

            for (int i = 0; i < wallWithDoor.transform.childCount; i++)
            {
                wallWithDoor.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        yield return null;
    }
}
