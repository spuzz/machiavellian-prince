using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PersonalityBehaviour : MonoBehaviour {

    protected PersonalityConfig config;

    public abstract void MakeDecisions(Empire empire, EmpireController empireController, ref State currentState);

    public void SetConfig(PersonalityConfig configToSet)
    {
        config = configToSet;
    }

}
