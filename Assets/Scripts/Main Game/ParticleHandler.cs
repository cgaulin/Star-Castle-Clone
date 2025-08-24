using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    //LocalVariables
    private float particleEmissionRate = 0f;

    //Components
    Player player;

    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        particleSystemSmoke = GetComponent<ParticleSystem>();

        //Get emmision component
        particleSystemEmissionModule = particleSystemSmoke.emission;

        //Setting emmision to zero at start
        particleSystemEmissionModule.rateOverTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;

        if (player.IsTireScreeching(out float lateralVelocity))
        {
            //if (lateralVelocity == 0f)
            //{
            //    lateralVelocity = 4f;
            //}
            particleEmissionRate = Mathf.Abs(lateralVelocity) * 20;
        }
    }
}
