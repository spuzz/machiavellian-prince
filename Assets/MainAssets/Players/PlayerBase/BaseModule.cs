using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseModule : MonoBehaviour {

    List<AgentConfig> agentConfigs;

    public IEnumerable<AgentConfig> GetAgentConfigs()
    {
        return agentConfigs;
    }

    public void AddAgentConfig(AgentConfig agentConfig)
    {
        agentConfigs.Add(agentConfig);
    }
}
