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
    SystemUI systemUI;

    private void Start()
    {
        systemUI = FindObjectOfType<SystemUI>();
        HireAgent(AgentTypes[0], systemUI.GetSystem());
    }
    public int GetPlayerNumber() { return playerNumber;  }
    public string GetPlayerName() { return playerName; }
    public Color GetPlayerColor() { return playerColor; }

    public bool HireAgent(AgentConfig agentConfig,SolarSystem system)
    {
        if(gold >= agentConfig.GetCost())
        {
            GameObject agent = Instantiate(agentConfig.GetAgentPrefab(), system.transform);
            agent.GetComponent<Agent>().SetPlayer(this);
            agent.GetComponent<Agent>().SetTargetSystem(system);
            agent.GetComponent<Agent>().SetAgentName("Agent Smith");
            foreach(AbilityConfig ability in agentConfig.GetAbilities())
            {
                agent.GetComponent<Agent>().AddAbility(ability);
            }
            
            gold -= agentConfig.GetCost();
            return true;
        }
        else
        {
            return false;
        }
    }
}
