using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbilityConfig : ScriptableObject
{
    public enum TARGETTYPE
    {
        Leader,
        Planet
    };

    [Header("Special Ability General")]
    [SerializeField] int cost = 10;
    [SerializeField] GameObject particlePrefab = null;
    [SerializeField] AnimationClip abilityAnimation;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] string abilityName;
    [SerializeField] TARGETTYPE targetType;

    protected AbilityBehaviour behaviour;

    abstract public AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo);

    public void AddComponent(GameObject gameObjectToAttachTo)
    {
        behaviour = GetBehaviourComponent(gameObjectToAttachTo);
        behaviour.SetConfig(this);
    }

    public void Use(GameObject target = null)
    {
        behaviour.Use(target);
    }

    public float GetCost()
    {
        return cost;
    }
    public string GetName()
    {
        return abilityName;
    }

    public TARGETTYPE GetTargetType()
    {
        return targetType;
    }

    public GameObject GetParticlePrefab()
    {
        return particlePrefab;
    }

    public AudioClip GetRandomAudioClip()
    {
        return audioClips[Random.Range(0, audioClips.Length)];
    }

    public AnimationClip GetAbilityAnimation()
    {
        return abilityAnimation;
    }

}

