using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("PlayerBase/BaseModule"))]
public class BaseModuleConfig : ScriptableObject {

    [SerializeField] List<AgentConfig> agentConfigs;

    public IEnumerable<AgentConfig> GetAgentConfigs()
    {
        return agentConfigs;
    }
}
