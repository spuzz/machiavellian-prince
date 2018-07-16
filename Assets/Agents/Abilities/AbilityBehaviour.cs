﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbilityBehaviour : MonoBehaviour
{
    protected AbilityConfig config;

    const float PARTICLE_CLEAN_UP_DELAY = 20;
    public abstract void Use(GameObject target = null, Agent agent = null);

    public void SetConfig(AbilityConfig configToSet)
    {
        config = configToSet;
    }

    protected void PlayParticleEffect()
    {
        GameObject particles = config.GetParticlePrefab();

        if (particles != null)
        {
            var particleObject = Instantiate(particles, gameObject.transform.position, particles.transform.rotation);
            particleObject.transform.parent = transform;
            particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }
    }

    IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
    {
        while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
        {
            yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
        }
        Destroy(particlePrefab);
        yield return new WaitForEndOfFrame();
    }

    protected void PlayAbilitySound()
    {
        var abilitySound = config.GetRandomAudioClip();
        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(abilitySound);
    }

    protected void PlayAnimation()
    {
        //var abilityAnimation = config.GetAbilityAnimation();
        //var overrideController = GetComponent<Character>().GetOverrideController();
        //var animator = GetComponent<Animator>();

        //overrideController[DEFAULT_ATTACK] = abilityAnimation;
        //animator.SetTrigger("Attack");
    }
}

