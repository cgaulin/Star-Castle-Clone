using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;

public class RingGrow : MonoBehaviour
{
    [SerializeField] private GameObject ringParent;
    [SerializeField] private GameObject newYellowRingPrefab;
    [SerializeField] private GameObject newOrangeRingPrefab;

    private GameObject redRing, orangeRing, yellowRing;
    private Vector3 redScale, orangeScale, yellowScale;
    private Color redRingColor, orangeRingColor;
    private float redRingMovementSpeed, orangeRingMovementSpeed;
    private int redRingLayer = 15;
    private int orangeRingLayer = 14;

    private int ringDestructionCount = 0;
    private Score score;

    private Transform tweenOrangeRing, tweenYellowRing;
    private Transform tweenNewYellowRing, tweenNewOrangeRing;

    private void Start()
    {
        FindRings();
        SetLocalScales();

        redRingColor = redRing.transform.GetComponentInChildren<SpriteRenderer>().color;
        orangeRingColor = orangeRing.transform.GetComponentInChildren<SpriteRenderer>().color;

        score = FindObjectOfType<Score>();
    }

    private void Update()
    {
        FindRings();
        SetLayers();
        ChangeLineColor();
        RingDestructionHandler();
        SetDoTweenRings();
        UpdateRingMovementSpeeds();
        HandleRingScore();
    }

    private void FindRings()
    {
        redRing = GameObject.Find("Outer Ring");
        orangeRing = GameObject.Find("Middle Ring");
        yellowRing = GameObject.Find("Inner Ring");
    }

    private void SetLocalScales()
    {
        redScale = redRing.transform.localScale;
        orangeScale = orangeRing.transform.localScale;
        yellowScale = yellowRing.transform.localScale;
    }

    private void SetDoTweenRings()
    {
        tweenOrangeRing = orangeRing.transform;
        tweenYellowRing = yellowRing.transform;
    }

    private void UpdateRingMovementSpeeds()
    {
        redRingMovementSpeed = redRing.GetComponent<RingMovement>().rotationSpeed = 75;
        orangeRingMovementSpeed = orangeRing.GetComponent<RingMovement>().rotationSpeed = -75;
    }

    private void SetLayers()
    {
        for (int i = 0; i < redRing.transform.childCount; i++)
        {
            redRing.transform.GetChild(i).gameObject.layer = redRingLayer;
        }

        for (int i = 0; i < orangeRing.transform.childCount; i++)
        {
            orangeRing.transform.GetChild(i).gameObject.layer = orangeRingLayer;
        }
    }

    private void ChangeLineColor()
    {
        for (int i = 0; i < redRing.transform.childCount; i++)
        {
            redRing.transform.GetChild(i).GetComponent<SpriteRenderer>().color = redRingColor;
            redRing.transform.GetChild(i).GetComponentInChildren<Light2D>().color = redRingColor;
        }

        for (int i = 0; i < orangeRing.transform.childCount; i++)
        {
            orangeRing.transform.GetChild(i).GetComponent<SpriteRenderer>().color = orangeRingColor;
            orangeRing.transform.GetChild(i).GetComponentInChildren<Light2D>().color = orangeRingColor;
        }
    }

    private void RingDestructionHandler()
    {
        if (redRing.transform.childCount <= 0 && orangeRing.transform.childCount > 0)
        {
            HandleRedRingDestruction();
            ringDestructionCount++;
        }
        else if (redRing.transform.childCount <= 0 && orangeRing.transform.childCount <= 0)
        {
            HandleRedAndOrangeRingDestruction();
            ringDestructionCount += 2;
        }
        else if (yellowRing.transform.childCount <= 0)
        {
            HandleYellowRingDestruction();
            ringDestructionCount++;
        }
    }

    private void HandleRingScore()
    {
        if (ringDestructionCount >= 3) score.stopScore = true;
    }

    private void HandleYellowRingDestruction()
    {
        Destroy(yellowRing);
        InstantiateNewYellowRing();
    }

    private void HandleRedAndOrangeRingDestruction()
    {
        Destroy(redRing);
        Destroy(orangeRing);
        InstantiateNewOrangeRing();
        InstantiateNewYellowRing();
        GrowYellowToRedRing();
    }

    private void HandleRedRingDestruction()
    {
        Destroy(redRing);
        InstantiateNewYellowRing();
        GrowOrangeToRedRing();
        GrowYellowToOrangeRing();
    }

    private void InstantiateNewYellowRing()
    {
        GameObject yellowRingClone = Instantiate(newYellowRingPrefab, ringParent.transform) as GameObject;
        yellowRingClone.transform.position = Vector2.zero;
        tweenNewYellowRing = yellowRingClone.transform;
        yellowRingClone.transform.localScale = Vector3.zero;
        tweenNewYellowRing.DOScale(yellowScale, 1.5f);
        yellowRingClone.name = yellowRing.name;
    }

    private void InstantiateNewOrangeRing()
    {
        GameObject orangeRingClone = Instantiate(newOrangeRingPrefab, ringParent.transform) as GameObject;
        orangeRingClone.transform.position = Vector2.zero;
        tweenNewOrangeRing = orangeRingClone.transform;
        orangeRingClone.transform.localScale = Vector3.zero;
        tweenNewOrangeRing.DOScale(orangeScale, 2f);
        orangeRingClone.name = orangeRing.name;
    }

    private void GrowOrangeToRedRing()
    {
        orangeRing.name = "Outer Ring";
        tweenOrangeRing.DOScale(redScale, 2f);
    }

    private void GrowYellowToOrangeRing()
    {
        yellowRing.name = "Middle Ring";
        tweenYellowRing.DOScale(orangeScale, 2f);
        FindRings();
        UpdateRingMovementSpeeds();
    }

    private void GrowYellowToRedRing()
    {
        yellowRing.name = "Outer Ring";
        tweenYellowRing.DOScale(redScale, 2f);

        //See if this can be optimized
        FindRings();
        UpdateRingMovementSpeeds();
        SetLayers();
        ChangeLineColor();
    }
}
