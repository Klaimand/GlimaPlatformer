using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_FlipCigaretteEnd : MonoBehaviour
{
    PlayerController2D controller;
    Transform fireEmitter;
    ParticleSystem fireEmitterParticleSystem;

    LineRenderer smokeLine;

    //Transform newSmoke;
    //ParticleSystem newSmokeParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController2D>();
        fireEmitter = GameObject.Find("Cigarette_Fire").transform ;
        fireEmitterParticleSystem = fireEmitter.GetComponent<ParticleSystem>();

        smokeLine = GameObject.Find("CigaretteSmoke").GetComponent<LineRenderer>();

        //newSmoke = GameObject.Find("Smoke_Cigsv2").transform;
        //newSmokeParticleSystem = newSmoke.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        linkFireToCigarette();
        changeParticle();
    }

    void LateUpdate()
    {
        checkFlip();
    }

    void checkFlip ()
    {
        float direction = 0f;
        if (controller.playerState == PlayerController2D.PlayerState.BlowedFalling || controller.playerState == PlayerController2D.PlayerState.BlowedAscending)
        {
            direction = !controller.FlipXInst ? -1f : 1f;
        }
        else
        {
            direction = controller.FlipXInst ? -1f : 1f;
        }
        transform.localPosition = new Vector3(direction * Mathf.Abs(transform.localPosition.x), transform.localPosition.y, transform.localPosition.z);
    }

    void linkFireToCigarette ()
    {
        fireEmitter.position = transform.position;
        //newSmoke.position = transform.position;
    }

    void changeParticle ()
    {
        if (controller.getSprintState())
        {
            if (!fireEmitterParticleSystem.isPlaying)
            {
                fireEmitterParticleSystem.Play();
            }
            smokeLine.enabled = false;
            //newSmokeParticleSystem.Stop();
            //newSmokeParticleSystem.gameObject.SetActive(false);
        }
        else
        {
            //newSmokeParticleSystem.gameObject.SetActive(true);
            /*
            if (!newSmokeParticleSystem.isPlaying)
            {
                newSmokeParticleSystem.Play();
            }*/
            smokeLine.enabled = true;
            fireEmitterParticleSystem.Stop();
        }
    }


}
