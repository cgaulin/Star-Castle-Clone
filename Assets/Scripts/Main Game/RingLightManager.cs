using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RingLightManager : MonoBehaviour
{
    [SerializeField] GameObject[] yellowRingLights;
    [SerializeField] GameObject[] orangeRingLights;
    [SerializeField] GameObject[] redRingLights;
    [SerializeField] Light2D backgroundLight;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip lightTurnOn;
    [SerializeField] float lightCooldown, backgroundLightDelay, backgroundLightIntensity;

    private bool yellowFinished, orangeFinished, redFinished = false;
    private bool isPlaying, done = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(TurnOnLights());
    }

    private IEnumerator TurnOnLights()
    {
        for (int i = 0; i < yellowRingLights.Length; i++)
        {
            yield return new WaitForSeconds(lightCooldown);
            yellowRingLights[i].SetActive(true);
            StartCoroutine(LightAudio());
        }
        yellowFinished = true;

        if (yellowFinished)
        {
            for (int i = 0; i < orangeRingLights.Length; i++)
            {
                yield return new WaitForSeconds(lightCooldown);
                orangeRingLights[i].SetActive(true);
            }
            orangeFinished = true;
        }

        if (orangeFinished)
        {
            for (int i = 0; i < redRingLights.Length; i++)
            {
                yield return new WaitForSeconds(lightCooldown);
                redRingLights[i].SetActive(true);
            }
            redFinished = true;
        }

        if (redFinished)
        {
            done = true;
            yield return new WaitForSeconds(1f);
            if (backgroundLight.intensity >= backgroundLightIntensity)
            {
                backgroundLight.intensity = backgroundLightIntensity;
            }
            else
            {
                backgroundLight.intensity += (backgroundLightDelay * Time.deltaTime);
            }
        }
        yield return null;
    }

    private IEnumerator LightAudio()
    {
        if (!isPlaying && !done)
        {
            isPlaying = true;
            audioSource.PlayOneShot(lightTurnOn);
            audioSource.pitch += 0.01f;
            yield return new WaitForSeconds(lightCooldown);
            isPlaying = false;
        }
    }
}
