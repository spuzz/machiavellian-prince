using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MissionDialog : MonoBehaviour {

    AbilityConfig currentConfig;
    Agent currentAgent;
    GameObject currentTarget;
    public void Start()
    {
        gameObject.SetActive(false);
    }
    public void UseAbility(AbilityConfig config, Agent agent, GameObject target = null)
    {
        currentConfig = config;
        currentAgent = agent;
        currentTarget = target;
        gameObject.SetActive(true);
    }

    public void OnConfirmClicked()
    {
        if(!currentConfig || !currentAgent)
        {
            throw new InvalidProgramException("Invalid ability");
        }
        currentConfig.Use(currentTarget, currentAgent);
        gameObject.SetActive(false);
    }

    public void OnCancelClicked()
    {
        gameObject.SetActive(false);
    }
}
