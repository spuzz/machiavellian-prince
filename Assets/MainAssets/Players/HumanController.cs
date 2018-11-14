using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HumanController : MonoBehaviour {

    Player player;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI agentText;
    [SerializeField] TextMeshProUGUI leadersText;
    [SerializeField] TextMeshProUGUI empireText;
    [SerializeField] TextMeshProUGUI systemText;
    [SerializeField] MeshCollider movementPlane;

    Agent selectedAgent;

    SystemUI systemUI;
    AgentUI agentUI;
    HUD hud;

    private void Awake()
    {
        hud = FindObjectOfType<HUD>();
        systemUI = FindObjectOfType<SystemUI>();
        agentUI = FindObjectOfType<AgentUI>();
    }
    void Start () {
        player = GetComponent<Player>();


        var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.onMouseOverSystem += ProcessMouseOverSystem;
        cameraRaycaster.onMouseRightClicked += ProcessMouseRightClick;

    }

    void Update()
    {
        if (!selectedAgent && player.GetTotalAgents() > 0)
        {
            GameObject agent = player.GetAgent(0);
            SelectObject(agent);
            hud.SelectObject(agent);
        }
        goldText.SetText(player.GetGold().ToString());
        agentText.SetText(player.GetTotalAgents().ToString());
        leadersText.SetText(player.GetTotalLeadersControlled().ToString());
        empireText.SetText(player.GetEmpiresControlled().ToString());
        systemText.SetText(player.GetSystemsControlled().ToString());


    }

    public Agent GetSelectedAgent()
    {
        return selectedAgent;
    }

    private void ProcessMouseOverSystem(SolarSystem system)
    {
        if (Input.GetMouseButton(1) == true && selectedAgent)
        {
            selectedAgent.SetTargetSystem(system);
        }
    }

    private void ProcessMouseRightClick(Ray ray)
    {
        RaycastHit raycast = new RaycastHit();
        if (selectedAgent && movementPlane.Raycast(ray, out raycast, 200))
        {
            selectedAgent.SetTarget(raycast.point);
        }
    }




    public void SelectObject(GameObject selectObject)
    {

        Agent agent = selectObject.GetComponent<Agent>();
        if (agent && agent.GetPlayer() == this.player)
        {
            selectedAgent = agent;
        }

    }


}
