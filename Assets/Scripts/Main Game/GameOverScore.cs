using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScore : MonoBehaviour
{
    [SerializeField] Text gameOverPlayerPoints;

    // Update is called once per frame
    void Update()
    {
        gameOverPlayerPoints.text = Score.points.ToString("0000000");
    }
}
