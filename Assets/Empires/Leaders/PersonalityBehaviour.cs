using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PersonalityBehaviour : MonoBehaviour {

    protected PersonalityConfig config;
    protected State currentState;

    public abstract void MakeDecisions(Empire empire, EmpireController empireController);

    public void SetConfig(PersonalityConfig configToSet)
    {
        config = configToSet;
    }

    protected void UpdateState(State state)
    {
        currentState = state;
    }
}
