using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MineNavigation : MonoBehaviour
{
    [Header("Orange Layer")]
    [SerializeField] LayerMask orangeLayer;
    [SerializeField] float orangeLayerMinRange;
    [SerializeField] float orangeLayerMaxRange;

    [Header("Red Layer")]
    [SerializeField] LayerMask redLayer;
    [SerializeField] float redLayerMinRange;
    [SerializeField] float redLayerMaxRange;

    [Header("Seeking Mine Settings")]
    [SerializeField] float minRange;
    [SerializeField] float maxRange;
    [SerializeField] float thrust;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float mineTickdown;

    private Transform target;
    private Rigidbody2D rb;
    private Vector2 direction;

    private RaycastHit2D orangeRaycastHit;
    private GameObject orangeRingPiece;

    private RaycastHit2D redRaycastHit;
    private GameObject redRingPiece;

    private float orangeCooldown, redCooldown, seekingCooldown;
    private bool orangeCool, redCool, seekingCool, onOrangeLayer, onRedLayer, targetPlayer;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        TrackMiddleRing();
        if(onOrangeLayer)
        {
            TrackOuterRing();
        }
    }

    void FixedUpdate()
    {
        if(onRedLayer)
        {
            TargetPlayer();
        }
    }

    private bool TrackMiddleRing()
    {
        orangeRaycastHit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), 0.6f, orangeLayer);
        Color rayColor;
        if(orangeRaycastHit.collider != null)
        {
            rayColor = Color.green;
            if(orangeCool)
            {
                if(orangeRaycastHit.transform.gameObject.tag != "Hit")
                {
                    transform.SetParent(orangeRaycastHit.transform, false);
                    onOrangeLayer = true;
                    orangeRingPiece = orangeRaycastHit.transform.gameObject;
                    orangeRingPiece.tag = "Hit";
                    StartCoroutine(ResetOrangeHitTag());
                }
            }
            else
            {
                StartCoroutine(OrangeCooldown());
            }
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * 0.6f, rayColor);
        return orangeRaycastHit.collider != null;
    }

    private bool TrackOuterRing()
    {
        redRaycastHit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), 0.6f, redLayer);
        Color rayColor;
        if(redRaycastHit.collider != null)
        {
            rayColor = Color.green;
            if(redCool)
            {
                if(redRaycastHit.transform.gameObject.tag != "Hit")
                {
                    transform.SetParent(redRaycastHit.transform, false);
                    onRedLayer = true;
                    redRingPiece = redRaycastHit.transform.gameObject;
                    redRingPiece.tag = "Hit";
                    StartCoroutine(ResetRedHitTag());
                }
            }
            else
            {
                StartCoroutine(RedCooldown());
            }
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * 0.6f, rayColor);
        return redRaycastHit.collider != null;
    }

    private void TargetPlayer()
    {
        if(seekingCool)
        {
            targetPlayer = true;
            transform.parent = null;
            rb.AddForce(transform.right * thrust);
            InitiateTargetPlayer();
            MineDissolve();
        }
        else
        {
            StartCoroutine(SeekingCooldown());
        }
    }

    private void MineDissolve()
    {
        Vector3 newScale = transform.localScale;
        newScale /= 1.004f;
        transform.localScale = newScale;

        mineTickdown -= Time.deltaTime;
        if(mineTickdown <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void InitiateTargetPlayer()
    {
        direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotateAmount * rotationSpeed;
        rb.velocity = transform.up * speed;
        thrust -= 1f;
        if(thrust <= 0)
        {
            thrust = 0;
        }
    }

    private IEnumerator OrangeCooldown()
    {
        orangeCooldown = Random.Range(orangeLayerMinRange, orangeLayerMaxRange);
        yield return new WaitForSeconds(orangeCooldown);
        orangeCool = true;
    }

    private IEnumerator RedCooldown()
    {
        redCooldown = Random.Range(redLayerMinRange, redLayerMaxRange);
        yield return new WaitForSeconds(redCooldown);
        redCool = true;
    }

    private IEnumerator SeekingCooldown()
    {
        seekingCooldown = Random.Range(minRange, maxRange);
        yield return new WaitForSeconds(seekingCooldown);
        seekingCool = true;
    }

    private IEnumerator ResetOrangeHitTag()
    {
        yield return new WaitForSeconds(orangeCooldown);
        if (orangeRingPiece != null) { orangeRingPiece.tag = "Untagged"; }

    }

    private IEnumerator ResetRedHitTag()
    {
        yield return new WaitForSeconds(redCooldown);
        if (redRingPiece != null) {  redRingPiece.tag = "Untagged"; }
    }
}
