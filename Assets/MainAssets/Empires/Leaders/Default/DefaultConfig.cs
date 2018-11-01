using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = ("Personality/Default"))]
public class DefaultConfig : PersonalityConfig
{
    [Header("Default Specific")]
    [SerializeField] Grow grow;
    [SerializeField] Attack attack;
    [SerializeField] BuildUp buildUp;

    public Attack GetAttack()
    {
        return attack;
    }

    public BuildUp GetBuildUp()
    {
        return buildUp;
    }

    public Grow GetGrow()
    {
        return grow;
    }
    public override PersonalityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo)
    {
        return gameObjectToAttachTo.AddComponent<DefaultBehaviour>();
    }

}
