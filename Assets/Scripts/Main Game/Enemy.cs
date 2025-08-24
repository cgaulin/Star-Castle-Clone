using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float delay;
    [SerializeField] float projectileSpeed;
    [SerializeField] float cooldown;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] ParticleSystem enemyDestroyParticles;
    private AudioSource audioSource;
    public bool canShoot = true;
    public bool isDestroyed = false;
    private bool shootCooldown = false;
    private bool cool = true;
    private LayerMask ringLayers;
    private Vector2 startPos;

    private Shredder laserShredder = new Shredder();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ringLayers = LayerMask.GetMask("Yellow Layer") | LayerMask.GetMask("Orange Layer") | LayerMask.GetMask("Red Layer");
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        TrackPlayer();
    }

    private void FollowPlayer()
    {
        //Follow Player
        Vector3 directionToFace = player.position - transform.position;
        float angle = Mathf.Atan2(-directionToFace.y, -directionToFace.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * delay);
    }

    private bool TrackPlayer()
    {
        //Track Player
        RaycastHit2D raycastHit = Physics2D.Raycast(firePoint.position, firePoint.TransformDirection(Vector2.left), 10f, ringLayers);
        Color rayColor;
        if(raycastHit.collider != null)
        {
            rayColor = Color.red;
        }
        else
        {
            rayColor = Color.green;
            if(cool)
            {
                StartCoroutine(Shoot());
            }
        }
        Debug.DrawRay(firePoint.position, firePoint.TransformDirection(Vector2.left) * 10f, rayColor);
        return raycastHit.collider != null;
    }

    private void InstantiateShoot()
    {
        if(canShoot)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity) as GameObject;
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.AddForce(-firePoint.right * projectileSpeed, ForceMode2D.Impulse);
            audioSource.PlayOneShot(laserSFX);
        }
    }

    public void ResetPosition()
    {
        transform.position = startPos;
    }

    private IEnumerator Shoot()
    {
        InstantiateShoot();
        cool = false;
        yield return new WaitForSeconds(cooldown);
        cool = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        laserShredder.LaserShredder();
        isDestroyed = true;
        Instantiate(enemyDestroyParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
