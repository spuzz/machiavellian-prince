using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

    SystemUI systemUI;
    ArmyUI armyUI;
    AgentUI agentUI;

    void Start()
    {
        GetComponent<Canvas>().enabled = true;
        systemUI = FindObjectOfType<SystemUI>();
        armyUI = FindObjectOfType<ArmyUI>();
        agentUI = FindObjectOfType<AgentUI>();
    }

    // Update is called once per frame
    void Update () {

    }

    public void SelectObject(GameObject selectedObject)
    {
        SolarSystem system = selectedObject.GetComponent<SolarSystem>();
        if (system)
        {
            systemUI.gameObject.SetActive(true);
            systemUI.SetSystem(system);
            return;
        }

        Army army = selectedObject.GetComponent<Army>();
        if (army)
        {
            armyUI.SetArmies(new List<Army>() { army });
            systemUI.gameObject.SetActive(false);
        }


        Agent agent = selectedObject.GetComponent<Agent>();
        if (agent && agent.GetPlayer().IsHumanPlayer())
        {
            agentUI.SelectAgent(agent);
        }
    }

    public void SetAgentTarget(GameObject selectedObject)
    {
        agentUI.MoveAgent(selectedObject);
    }
}
