using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float rotationSpeed = 30f;
    [SerializeField] float thrust = 1f;
    private Rigidbody2D rb;

    [Header("Player Shooting")]
    [SerializeField] Transform firePoint;
    [SerializeField] Transform firePointTwo;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject laserPrefabTwo;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] Light2D leftHeadlight, rightHeadlight, leftBrakeLight, rightBrakeLight;
    private AudioSource audioSource;

    [Header("Player Lives")]
    public int playerLives = 3;

    [Header("Screen Wrap")]
    [SerializeField] float screenHeight;
    [SerializeField] float screenWidth;

    private Vector2 startPos;
    private Quaternion startRotation;
    private Game gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        startPos = transform.position;
        startRotation = transform.rotation;
        gameManager = FindObjectOfType<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Fire();
        ScreenWrap();
    }

    private void FixedUpdate() 
    {
        Thrust();
    }

    private void PlayerMovement()
    {
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0f, 0f, 1f) * rotationSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0f, 0f, -1f) * rotationSpeed * Time.deltaTime);
        }
    }

    private void Thrust()
    {
        if(Input.GetKey(KeyCode.J))
        {
            rb.AddForce(transform.right * thrust);
            DampenBrakeLights();
        }
        else if (Input.GetKeyUp(KeyCode.J))
        {
            BrightenBrakeLights();
        }
    }

    private void Fire()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation) as GameObject;
            Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * projectileSpeed, ForceMode2D.Impulse);

            GameObject laserTwo = Instantiate(laserPrefab, firePointTwo.position, firePointTwo.rotation) as GameObject;
            Rigidbody2D rbTwo = laserTwo.GetComponent<Rigidbody2D>();
            rbTwo.AddForce(firePointTwo.right * projectileSpeed, ForceMode2D.Impulse);

            audioSource.PlayOneShot(laserSFX);

            BrightenHeadlights();
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            DampenHeadlights();
        }
    }

    private void ScreenWrap()
    {
        Vector2 playerPos = transform.position;

        if(transform.position.y > screenHeight)
        {
            playerPos.y = -screenHeight;
        }
        transform.position = playerPos;

        if(transform.position.y < -screenHeight)
        {
            playerPos.y = screenHeight;
        }
        transform.position = playerPos;

        if(transform.position.x > screenWidth)
        {
            playerPos.x = -screenWidth;
        }
        transform.position = playerPos;

        if(transform.position.x < -screenWidth)
        {
            playerPos.x = screenWidth;
        }
        transform.position = playerPos;
    }

    private float GetLateralVelocity()
    {
        //Returns how fast the car is moving sideways
        return Vector2.Dot(transform.right, rb.velocity);
    }

    public float GetVelocityMagnitude()
    {
        return rb.velocity.magnitude;
    }

    public bool IsTireScreeching(out float lateralVelocity)
    {
        lateralVelocity = GetLateralVelocity();

        //If we have a lot of side movement then the tires should be screeching
        if (GetLateralVelocity() <= 5f) { return true; }

        return false;
    }

    private void BrightenHeadlights()
    {
        leftHeadlight.intensity = 30f;
        rightHeadlight.intensity = 30f;
    }

    private void DampenHeadlights()
    {
        leftHeadlight.intensity = 10f;
        rightHeadlight.intensity = 10f;
    }

    private void BrightenBrakeLights()
    {
        leftBrakeLight.intensity = 5f;
        rightBrakeLight.intensity = 5f;
    }

    private void DampenBrakeLights()
    {
        leftBrakeLight.intensity = 2.5f;
        rightBrakeLight.intensity = 2.5f;
    }

    private void ResetPlayer()
    {
        playerLives--;
        FindObjectOfType<Score>().ManageLives();
        transform.position = startPos;
        transform.rotation = startRotation;
        rb.velocity = Vector2.zero;
        gameManager.ShowScore();
    }

    public void PlayerDeath()
    {
        if(playerLives <= 1)
        {
            gameObject.SetActive(false);
            gameManager.GameOver();
        }
        else
        {
            ResetPlayer();
        }
    }
}
