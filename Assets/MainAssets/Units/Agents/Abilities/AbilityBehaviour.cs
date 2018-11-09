using System.Collections;
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

    protected void PlayParticleEffect(GameObject gameObjectTarget)
    {
        GameObject particles = config.GetParticlePrefab();

        if (particles != null)
        {
            var particleObject = Instantiate(particles, gameObjectTarget.transform.position, particles.transform.rotation);
            particleObject.transform.parent = gameObjectTarget.transform;
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

    protected void PlayAbilitySound(GameObject gameObjectTarget)
    {
        var abilitySound = config.GetRandomAudioClip();
        var audioSource = gameObjectTarget.GetComponent<AudioSource>();
        audioSource.PlayOneShot(abilitySound);
    }

    protected void PlayAnimation(GameObject gameObjectTarget)
    {
        //var abilityAnimation = config.GetAbilityAnimation();
        //var overrideController = GetComponent<Character>().GetOverrideController();
        //var animator = GetComponent<Animator>();

        //overrideController[DEFAULT_ATTACK] = abilityAnimation;
        //animator.SetTrigger("Attack");
    }
}

