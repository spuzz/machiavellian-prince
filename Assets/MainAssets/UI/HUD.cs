using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

    SystemUI systemUI;
    PlayerBaseUI playerBaseUI;
    AgentUI agentUI;
    GameObject currentObjectSelected;
    List<GameObject> selectionUI = new List<GameObject>();

    private void Awake()
    {
        systemUI = FindObjectOfType<SystemUI>();
        agentUI = FindObjectOfType<AgentUI>();
        playerBaseUI = FindObjectOfType<PlayerBaseUI>();
        selectionUI.Add(systemUI.gameObject);
        selectionUI.Add(agentUI.gameObject);
        selectionUI.Add(playerBaseUI.gameObject);
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

            
            systemUI.UpdateSystem(system);
            system.SelectSystem(true);
            SelectNewObject(system.gameObject);
            ChooseUI(systemUI.gameObject);
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
            ChooseUI(agentUI.gameObject);
            return;
        }

        PlayerBase playerBase = selectedObject.GetComponent<PlayerBase>();
        if (playerBase && playerBase.GetPlayer().IsHumanPlayer())
        {
            playerBase.SelectBase(true);
            SelectNewObject(playerBase.gameObject);
            ChooseUI(playerBaseUI.gameObject);
            return;
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

            PlayerBase playerBase = currentObjectSelected.GetComponent<PlayerBase>();
            if (playerBase)
            {
                playerBase.SelectBase(false);

            }
        }

        currentObjectSelected = gameObject;
    }

    public void SetAgentTarget(GameObject selectedObject)
    {
        agentUI.MoveAgent(selectedObject);
    }

    public void ChooseUI(GameObject uiChoice)
    {
        foreach(GameObject uiObject in selectionUI)
        {
            if(uiChoice == uiObject)
            {
                uiObject.SetActive(true);
            }
            else
            {
                uiObject.SetActive(false);
            }
        }
    }
}
