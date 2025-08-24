using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] Text playerPoints;
    [SerializeField] Text playerLives;
    public static int points;
    public bool stopScore = false;
    private int lives;

    [Header("Ring Points")]
    [SerializeField] int redRingPoints = 10;
    [SerializeField] int orangeRingPoints = 30;
    [SerializeField] int yellowRingPoints = 50;

    // Start is called before the first frame update
    private void Start()
    {
        lives = FindObjectOfType<Player>().playerLives;

        playerPoints.text = points.ToString();
        playerLives.text = lives.ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        playerPoints.text = points.ToString("0000000");
    }

    public int ReturnScore()
    {
        return points;
    }

    public void AddCokePoints()
    {
        points += FindObjectOfType<CokeSpawn>().points;
    }

    public void AddRedRingPoints()
    {
        points += redRingPoints;
    }

    public void AddOrangeRingPoints()
    {
        points += orangeRingPoints;
    }

    public void AddYellowRingPoints()
    {
        points += yellowRingPoints;
    }

    public void ManageLives()
    {
        lives = FindObjectOfType<Player>().playerLives;
        playerLives.text = lives.ToString();
    }
}
