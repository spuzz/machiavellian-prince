using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

    SystemUI systemUI;
    ArmyUI armyUI;
    AgentUI agentUI;
    GameObject currentObjectSelected;

    private void Awake()
    {
        systemUI = FindObjectOfType<SystemUI>();
        armyUI = FindObjectOfType<ArmyUI>();
        agentUI = FindObjectOfType<AgentUI>();
    }
    void Start()
    {
        GetComponent<Canvas>().enabled = true;
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
            systemUI.UpdateSystem(system);
            system.SelectSystem(true);
            SelectNewObject(system.gameObject);
            return;
        }

        //Army army = selectedObject.GetComponent<Army>();
        //if (army)
        //{
        //    armyUI.SetArmies(new List<Army>() { army });
        //    systemUI.gameObject.SetActive(false);
        //}


        Agent agent = selectedObject.GetComponent<Agent>();
        if (agent && agent.GetPlayer().IsHumanPlayer())
        {
            agent.SelectAgent(true);
            agentUI.SelectAgent(agent);
            SelectNewObject(agent.gameObject);
        }
        
    }

    private void SelectNewObject(GameObject gameObject)
    {
        if(currentObjectSelected)
        {
            SolarSystem system = currentObjectSelected.GetComponent<SolarSystem>();
            if (system)
            {
                system.SelectSystem(false);

            }
            Agent agent = currentObjectSelected.GetComponent<Agent>();
            if (agent)
            {
                agent.SelectAgent(false);
            }
        }

        currentObjectSelected = gameObject;
    }

    public void SetAgentTarget(GameObject selectedObject)
    {
        agentUI.MoveAgent(selectedObject);
    }
}
