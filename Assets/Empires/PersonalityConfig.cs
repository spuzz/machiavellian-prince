using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Personality"))]
public class PersonalityConfig : ScriptableObject
{
    [Header("General")]
    public string PersonalityName;

    [Header("Grow")]
    [Range(0, 100)] public float growEcononyToUse;
    public float growMinDefencePerSystem;
    public float growMinAttackPerSystem;

    [Header("Expand")]
    [Range(0, 100)] public  float expandEcononyToUse;
    public float expandMinDefencePerSystem;
    public float expandMinimumAttackPerSystem;

    [Header("Attack")]
    [Range(0, 100)] public float attackEcononyToUse;
    public float attackMinDefencePerSystem;
    public float attackMinAttackPerSystem;

    [Header("Defend")]
    [Range(0, 100)] public float defendEcononyToUse;
    public float defendMinDefencePerSystem;
    public float defendMinAttackPerSystem;
}
