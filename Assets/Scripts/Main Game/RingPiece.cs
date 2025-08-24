using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPiece : MonoBehaviour
{
    [SerializeField] int health = 4;
    [SerializeField] ParticleSystem destroyParticles;
    private ParticleSystem.MainModule destroyParticlesMain;
    private Shredder shredder = new Shredder();
    private Color orangeColor;

    private Score score;
    private int redLayer = 15;
    private int orangeLayer = 14;
    private int yellowLayer = 12;

    void Start()
    {
        score = FindObjectOfType<Score>();
        destroyParticlesMain = destroyParticles.main;
        orangeColor = new Color32(255, 122, 4, 255);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Laser")
        {
            Laser laser = other.gameObject.GetComponent<Laser>();
            health -= laser.GetDamage();
            shredder.LaserShredder();
            if (health <= 0)
            {
                if(gameObject.layer == redLayer)
                {
                    destroyParticlesMain.startColor = Color.red;
                    if (!score.stopScore) score.AddRedRingPoints();
                }
                else if (gameObject.layer == orangeLayer)
                {
                    destroyParticlesMain.startColor = orangeColor;
                    if (!score.stopScore) score.AddOrangeRingPoints();
                }
                else if (gameObject.layer == yellowLayer)
                {
                    destroyParticlesMain.startColor = Color.yellow;
                    if (!score.stopScore) score.AddYellowRingPoints();
                }
                Instantiate(destroyParticles, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        else
        {
            return;
        }
    }
}
