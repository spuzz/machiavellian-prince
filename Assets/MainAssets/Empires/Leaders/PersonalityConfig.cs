using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PersonalityConfig : ScriptableObject
{
    [Header("General")]
    public string personalityName;

    protected PersonalityBehaviour behaviour;
    abstract public PersonalityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo);

    public void AddComponent(GameObject gameObjectToAttachTo)
    {
        behaviour = GetBehaviourComponent(gameObjectToAttachTo);
        behaviour.SetConfig(this);
    }

    public void MakeDecisions(Empire empire, EmpireController empireController, ref State currentState)
    {
        behaviour.MakeDecisions(empire, empireController, ref currentState);
    }

    public string GetName()
    {
        return personalityName;
    }
}
