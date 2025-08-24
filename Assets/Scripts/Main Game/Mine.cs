using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [Header("General Mine Settings")]
    [SerializeField] Transform direction;
    [SerializeField] float rotationSpeed;
    [SerializeField] LayerMask yellowLayer;
    [SerializeField] GameObject minePrefab;
    [SerializeField] float minRange;
    [SerializeField] float maxRange;
    [SerializeField] float initialCooldownMin;
    [SerializeField] float initalCooldownMax;

    [Header("Mine SFX Settings")]
    [SerializeField] AudioClip mineSFX;
    [SerializeField] float minPitchRange = 0.55f;
    [SerializeField] float maxPitchRange = 0.65f;

    private RaycastHit2D raycastHit;
    private GameObject yellowRingPiece;
    private AudioSource audioSource;

    private bool cool = true;
    private bool startCooldown = false;
    private float mineCooldown = 0f;
    private float startMineCooldown;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ContinuousRotate();
        SearchInnerRing();
    }

    private void ContinuousRotate()
    {
        //Rotates Raycast around inner ring
        direction.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private bool SearchInnerRing()
    {
        raycastHit = Physics2D.Raycast(direction.position, direction.TransformDirection(Vector2.up), 1f, yellowLayer);
        Color rayColor;
        if(raycastHit.collider != null)
        {
            rayColor = Color.green;
            if(cool && startCooldown)
            {
                if(raycastHit.transform.gameObject.tag != "Hit")
                {
                    if(mineCooldown <= 0f)
                    {
                        //Places a mine on inner ring and puts mine maker on cooldown, also tags ring piece as "Hit"
                        mineCooldown = Random.Range(minRange, maxRange);
                        GameObject mine = Instantiate(minePrefab, direction.position, Quaternion.identity) as GameObject;
                        mine.transform.SetParent(raycastHit.transform, false);
                        yellowRingPiece = raycastHit.transform.gameObject;
                        yellowRingPiece.tag = "Hit";
                        StartCoroutine(ResetHitTag());
                        audioSource.PlayOneShot(mineSFX);
                        audioSource.pitch = Random.Range(minPitchRange, maxPitchRange);
                    }
                    else
                    {
                        mineCooldown -= Time.deltaTime;
                    }
                }
            }
            else
            {
                StartCoroutine(InitialCooldown());
            }
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(direction.position, direction.TransformDirection(Vector2.up) * 1f, rayColor);
        return raycastHit.collider != null;
    }

    private IEnumerator InitialCooldown()
    {
        float cooldown = Random.Range(initialCooldownMin, initalCooldownMax);
        cool = false;
        yield return new WaitForSeconds(cooldown);
        startCooldown = true;
        cool = true;
    }

    private IEnumerator ResetHitTag()
    {
        yield return new WaitForSeconds(mineCooldown);
        if (yellowRingPiece != null) { yellowRingPiece.tag = "Untagged"; }
    }
}
