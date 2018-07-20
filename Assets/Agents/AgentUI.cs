using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentUI : MonoBehaviour {

    Dropdown agents;
    Dropdown operations;
    Dropdown targets;
    [SerializeField] Planet planet;
    Agent selectedAgent;
	// Use this for initialization
	void Start () {
        var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.onMouseOverPlanet += ProcessMouseOverPlanet;
        agents = transform.Find("Agent").GetComponent<Dropdown>();
        operations = transform.Find("Operation").GetComponent<Dropdown>();
        targets = transform.Find("Target").GetComponent<Dropdown>();
        if (planet)
        {
            UpdateAgents(planet);
            UpdateOperation();
            UpdateTarget();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnAgentChange()
    {

    }
    private void ProcessMouseOverPlanet(Planet planet)
    {
        if (Input.GetMouseButton(0) == true)
        {
            StopAllCoroutines();
            UpdateAgents(planet);


        }
    }
    private void UpdateAgents(Planet planet)
    {
        agents.options.Clear();
        List<string> options = new List<string>();
        foreach (Agent agent in planet.GetAgents())
        {
            options.Add(agent.GetAgentName());

        }
        agents.AddOptions(options);
    }

    public void UpdateOperation()
    {
        operations.options.Clear();
        List<string> options = new List<string>();
        selectedAgent = planet.GetAgents().Find(c => c.GetAgentName() == agents.options[agents.value].text);
        if(!selectedAgent)
        {
            return;
        }
        foreach (AbilityConfig ability in selectedAgent.GetAbilities())
        {
            options.Add(ability.GetName());

        }
        operations.AddOptions(options);
    }

    public void UpdateTarget()
    {
        if (!selectedAgent)
        {
            return;
        }
        AbilityConfig ability = selectedAgent.GetAbilities().Find(c => c.GetName() == operations.options[agents.value].text);
        if(!ability)
        {
            return;
        }
        switch (ability.GetTargetType())
        {
            case AbilityConfig.TARGETTYPE.Leader:
                List<Leader> leaders = selectedAgent.GetTargetPlanet().GetEmpire().GetPotentialLeaders();
                targets.options.Clear();
                List<string> options = new List<string>();
                foreach (Leader leader in leaders)
                {
                    options.Add(leader.GetName());
                }
                targets.AddOptions(options);
                break;
        }

    }
}
