using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] string playerName;
    [SerializeField] int playerNumber;
    [SerializeField] Color playerColor;
    [SerializeField] List<GameObject> agents;
    [SerializeField] List<AgentConfig> AgentTypes;
    [SerializeField] long gold;
    PlanetUI planetUI;

    private void Start()
    {
        planetUI = FindObjectOfType<PlanetUI>();
        HireAgent(AgentTypes[0], planetUI.GetPlanet());
    }
    public int GetPlayerNumber() { return playerNumber;  }
    public string GetPlayerName() { return playerName; }
    public Color GetPlayerColor() { return playerColor; }

    public bool HireAgent(AgentConfig agentConfig,Planet planet)
    {
        if(gold >= agentConfig.GetCost())
        {
            GameObject agent = Instantiate(agentConfig.GetAgentPrefab(), planet.transform);
            agent.GetComponent<Agent>().SetPlayer(this);
            agents.Add(agent);
            gold -= agentConfig.GetCost();
            return true;
        }
        else
        {
            return false;
        }
    }
}
