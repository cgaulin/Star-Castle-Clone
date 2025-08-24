using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private float resetCooldown = 3f;
    [SerializeField] private float enemyShootDelay = 5f;
    [SerializeField] private Transform mainCamera;

    private Vector3 mainCameraStartPos;
    private Canvas canvas;
    private Player player;
    private Enemy enemy;
    private Mine mineMaker;

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        mineMaker = FindObjectOfType<Mine>();
        mainCameraStartPos = mainCamera.position;
    }

    public void ShowScore()
    {
        StartCoroutine(ScoreTime());
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }

    private IEnumerator ScoreTime()
    {
        mainCamera.position = Vector3.forward;
        canvas.enabled = true;
        player.enabled = false;
        mineMaker.enabled = false;
        DestroyMines();
        enemy.ResetPosition();
        enemy.canShoot = false;
        yield return new WaitForSeconds(resetCooldown);
        mainCamera.position = mainCameraStartPos;
        canvas.enabled = false;
        player.enabled = true;
        mineMaker.enabled = true;
        StartCoroutine(EnemyShootDelayRoutine());
    }

    private IEnumerator EnemyShootDelayRoutine()
    {
        yield return new WaitForSeconds(enemyShootDelay);
        enemy.canShoot = true;
    }

    private void DestroyMines()
    {
        GameObject[] mines = GameObject.FindGameObjectsWithTag("Mine");
        foreach(GameObject mine in mines)
        {
            Destroy(mine);
        }
    }
}
