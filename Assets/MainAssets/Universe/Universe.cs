﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Universe : MonoBehaviour {



    
    [SerializeField] float timePerDay = 1.0f;
    [SerializeField] float maxTravelDistance = 1000.0f;
    [SerializeField] float borderDistance = 20.0f;

    public int mainPlayer = 1;
    int currentDay;
    public delegate void OnSystemOwnerChanged(SolarSystem system); 
    public event OnSystemOwnerChanged onSystemOwnerChanged; 

    public delegate void OnLeaderLoyaltyChanged(Leader leader); 
    public event OnLeaderLoyaltyChanged onLeaderLoyaltyChanged; 

    public delegate void OnLeaderDeath(Leader leader); 
    public event OnLeaderDeath onLeaderDeath;

    public delegate void OnEmpireLeaderChange(Empire empire, Leader leader);
    public event OnEmpireLeaderChange onEmpireLeaderChange;

    private int xSize = 100;
    private int ySize = 100;
    float timeSinceLastDay;

    private GameObject selected;


    private Shader shaderOutline;
    private Shader shaderNoOutline;
    private List<Empire> empires;
    private List<Player> players;

    SolarSystem[] systems;

    SpeedUI speedUI;

    bool gameOver = false;
    public float GetMaxTravelDistance()
    {
        return maxTravelDistance;
    }
    void Start () {
        currentDay = 1;
        shaderOutline = Shader.Find("Outlined/Uniform");
        shaderNoOutline = Shader.Find("Standard");
        var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.onMouseOverSystem += ProcessMouseOverSystem;

        
        CreateUniverse();
        speedUI = FindObjectOfType<SpeedUI>();
        timeSinceLastDay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            timeSinceLastDay += Time.deltaTime * speedUI.GetSpeed();
            int daysPassed = 0;
            while (timeSinceLastDay > timePerDay)
            {
                daysPassed++;
                currentDay++;
                timeSinceLastDay -= timePerDay;
            }

            if (daysPassed > 0)
            {
                onDayChanged(daysPassed);
            }
        }


    }
    private void CreateUniverse()
    {
        GenerateSystems();
        GenerateBorders();
        GenerateEmpires();
        GeneratePlayers();
        EmpireFindChildren();
        EmpirePickLeaders();
        AssignPlayerEmpires();
        PlayerHireInitialAgents();

    }

    private void PlayerHireInitialAgents()
    {
        foreach(Player player in players)
        {
            player.HireInitialAgents();
        }
    }

    private void EmpireFindChildren()
    {
        foreach (Empire empire in empires)
        {
            empire.FindChildren();
        }
    }

    private void GeneratePlayers()
    {
        players = FindObjectsOfType<Player>().ToList();
    }

    private void AssignPlayerEmpires()
    {
        if (players.Count > empires.Count)
        {
            throw new InvalidProgramException();
        }

        int count = 0;
        foreach(Player player in players)
        {
            player.TakeControlOfEmpire(empires[count]);
            count++;
        }
    }

    private void EmpirePickLeaders()
    {
        foreach(Empire empire in empires)
        {
            empire.PickLeader();
        }
    }

    // TODO: temp code to get Empires from demo scene

    private void GenerateEmpires()
    {
        empires = FindObjectsOfType<Empire>().ToList();
    }

    public IEnumerable<Empire> GetEmpires()
    {
        return empires;
    }

    // TODO: temp code to get systems from demo scene
    private void GenerateSystems()
    {
        systems = FindObjectsOfType<SolarSystem>();


    }
    private void GenerateBorders()
    {
        List<BorderController> borders = new List<BorderController>();
        foreach (SolarSystem system in systems)
        {
            BorderController border = system.transform.Find("Border").GetComponent<BorderController>();
            border.SetBorderDistance(borderDistance);
            foreach (SolarSystem possibleNeighbour in systems)
            {
                if (possibleNeighbour != system && Vector3.Distance(possibleNeighbour.transform.position, system.transform.position) <= borderDistance * 2)
                {
                    border.AddNearbySystem(possibleNeighbour);
                }
            }

            borders.Add(border);
        }

        foreach (BorderController border in borders)
        {
            bool result = true;
            while (result)
            {
                result = border.GrowBorder();
            }
            border.CreateBorderMesh();
        }
        foreach (SolarSystem system in systems)
        {
            system.UpdateBorders();
        }
    }

    private void ProcessMouseOverSystem(SolarSystem system)
    {
        if(Input.GetMouseButton(0) == true)
        {
            
            ClearSelection();
            selected = system.gameObject;
            system.GetComponent<Renderer>().material.shader = shaderOutline;
        }

    }

    public void SystemChange(SolarSystem system)
    {
        onSystemOwnerChanged(system);
    }

    public void LeaderLoyaltyChange(Leader leader)
    {
        onLeaderLoyaltyChanged(leader);
    }

    public void LeaderDeath(Leader leader)
    {
        onLeaderDeath(leader);
    }

    public void EmpireLeaderChange(Empire empire, Leader leader)
    {
        onEmpireLeaderChange(empire, leader);
    }


    private void ClearSelection()
    {
        if(selected)
        {
            selected.GetComponent<Renderer>().material.shader = shaderNoOutline;
        }
        
    }



    public void CheckEndGame()
    {
        
        int empiresAlive = 0;
        foreach(Empire empire in empires)
        {
            if(empire.IsAlive() == true)
            {
                empiresAlive += 1;
            }
        }
        if (empiresAlive <= 1)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
    }
    public Empire GetNeutralEmpire()
    {
        foreach(Empire empire in empires)
        {
            if(empire.GetLeader().ControlledBy() == null)
            {
                return empire;
            }
        }
        return null;

    }

    public delegate void OnDayChanged(int days); // declare new delegate type
    public event OnDayChanged onDayChanged; // instantiate an observer set
}