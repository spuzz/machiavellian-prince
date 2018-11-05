using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentUI : MonoBehaviour {

    [SerializeField] Image agentPortait;
    [SerializeField] TextMeshProUGUI agentName;
    [SerializeField] GameObject scrollViewContent;
    [SerializeField] List<AbilityButton> abilities;

    bool buttonsEnabled = true;
    Agent currentAgent;
    private void Update()
    {
        if(currentAgent)
        {
            if(currentAgent.GetCurrentSystem() && buttonsEnabled == false)
            {
                EnableAbilities(true);
            }
            else if( buttonsEnabled == true)
            {
                EnableAbilities(false);
            }
        }

    }

    public void SelectAgent(Agent agent)
    {
        agentPortait.sprite = agent.GetPortrait();
        agentName.SetText(agent.GetAgentName());
        int abilityNumber = 0;
        foreach(AbilityConfig ability in agent.GetAbilities())
        {
            abilities[abilityNumber].SetAbility(ability);
            abilities[abilityNumber].TurnOnButton();
            abilityNumber++;
        }
        
        while(abilityNumber < abilities.Count)
        {
            abilities[abilityNumber].TurnOffButton();
            abilityNumber++;
        }

        currentAgent = agent;
    }

    public void MoveAgent(GameObject targetGameObject)
    {
        SolarSystem system = targetGameObject.GetComponent<SolarSystem>();
        if (system)
        {
            currentAgent.SetTargetSystem(system);
            return;
        }
        else
        {
            currentAgent.SetTarget(targetGameObject.transform.position);
        }
    }

    public void EnableAbilities(bool enable)
    {
        foreach (AbilityButton ability in abilities)
        {
            ability.SetInteractable(enable);
        }
        buttonsEnabled = enable;
    }


}
