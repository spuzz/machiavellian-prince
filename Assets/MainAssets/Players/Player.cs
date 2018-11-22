using System;
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
    [SerializeField] bool isVisible = false;
    [SerializeField] GameObject playerBasePrefab;

    List<GameObject> visibleObjects = new List<GameObject>();
    [SerializeField] List<SolarSystem> systemsWithBuildings = new List<SolarSystem>();

    Universe universe;
    FogCamera fogCamera;
    PlayerVisibility playerVisibility;
    List<Leader> leadersControlled = new List<Leader>();
    List<PlayerBase> playerBases = new List<PlayerBase>();
    int systemsControlled;
    int empiresControlled;
    List<Empire> empires = new List<Empire>();
    bool alive = true;
    
    private void Awake()
    {
        universe = FindObjectOfType<Universe>();
        fogCamera = FindObjectOfType<FogCamera>();
        
        empiresControlled = 0;
    }

    private void Start()
    {
        universe.onSystemOwnerChanged += OnSystemChange;
        universe.onLeaderLoyaltyChanged += OnLeaderLoyaltyChange;
        universe.onLeaderDeath += OnLeaderDeath;
        universe.onEmpireLeaderChange += OnEmpireLeaderChange;
        universe.onDayChanged += OnDayChange;

    }

    private void Update()
    {
        if(alive && empires.Count == 0)
        {
            PlayerLost();
            return;
        }

    }

    public SolarSystem GetHomeSystem()
    {
        return empires[0].GetSystems()[0];
    }

    public bool IsAlive()
    {
        return alive;
    }
    public void TakeControlOfEmpire(Empire empire)
    {
        foreach (Leader leader in empire.GetPotentialLeaders())
        {
            leader.IncreaseInfluence(this, 1000);
            OnLeaderLoyaltyChange(leader);
        }
        UpdateStats();
    }

    public void AddPlayerBase(PlayerBase playerBase)
    {
        playerBases.Add(playerBase);
        if (isVisible)
        {
            playerVisibility.AddObject(playerBase.gameObject);
        }
    }

    public GameObject GetPlayerBasePrefab()
    {
        return playerBasePrefab;
    }


    public void AddAgent(GameObject agent)
    {
        agents.Add(agent);


        if (isVisible)
        {
            playerVisibility.AddObject(agent.gameObject);
        }
    }
    private void PlayerLost()
    {
        if(IsHumanPlayer())
        {
            universe.GameOver();
        }
        else
        {
            alive = false;
            foreach(GameObject agent in agents)
            {
                Destroy(agent);
            }
            agents.Clear();
            universe.CheckEndGame();
        }
    }

    public bool IsHumanPlayer()
    {
        if(GetComponent<HumanController>())
        {
            return true;
        }
        return false;
    }

    public void SetVislble(bool visible)
    {
        isVisible = visible;
        if (isVisible)
        {
            playerVisibility = GetComponent<PlayerVisibility>();
            foreach(Empire empire in empires)
            {
                playerVisibility.AddEmpire(empire);
                foreach(SolarSystem system in empire.GetSystems())
                {
                    if(system.GetComponent<PlayerBuildingController>().GetPlayerSpyNetwork(this))
                    {
                        playerVisibility.AddObject(system.gameObject);
                    }
                }
            }
            foreach(GameObject agent in agents)
            {
                playerVisibility.AddObject(agent);
            }

            foreach (PlayerBase playerBase in playerBases)
            {
                playerVisibility.AddObject(playerBase.gameObject);
            }

        }
        else
        {
            playerVisibility = null;
        }
    }

    public bool IsVisible()
    {
        return isVisible;
    }

    public int GetPlayerNumber() { return playerNumber;  }
    public string GetPlayerName() { return playerName; }
    public Color GetPlayerColor() { return playerColor; }

    public void AddSystemWithBuildings(SolarSystem system)
    {
        systemsWithBuildings.Add(system);
    }

    public long GetGold()
    {
        return gold;
    }

    public bool UseGold(int cost)
    {
        if(gold < cost)
        {
            return false;
        }
        gold -= cost;
        return true;
    }

    public int GetTotalAgents()
    {
        return agents.Count;
    }

    public IEnumerable<GameObject> GetAgents()
    {
        return agents;
    }

    public GameObject GetAgent(int number)
    {
        if(number < 0 || number >= agents.Count)
        {
            return null;
        }
        return agents[number];
    }


    public int GetTotalLeadersControlled()
    {
        return leadersControlled.Count;
    }

    public int GetSystemsControlled()
    {
        return systemsControlled;
    }

    public int GetEmpiresControlled()
    {
        return empiresControlled;
    }



    private void UpdateStats()
    {
        empiresControlled = 0;
        systemsControlled = 0;
        foreach (Empire empire in empires)
        {
            empiresControlled += 1;
            systemsControlled += empire.GetTotalSystemsControlled();
        }

    }

    private void AddEmpire(Empire empire)
    {
        empires.Add(empire);
        if(playerVisibility)
        {
            playerVisibility.AddEmpire(empire);
        }

    }
    private void RemoveEmpire(Empire empire)
    {
        empires.Remove(empire);
        if (playerVisibility)
        {
            playerVisibility.RemoveEmpire(empire);
        }
    }


    private void OnDayChange(int days)
    {
        int goldChange = 0;
        foreach(SolarSystem system in systemsWithBuildings)
        {
            float systemGold = system.GetNetIncome() / 10.0f;
            float percentageAdjustment = 100;
            PlayerBuildingController playerBuildingController = system.GetComponent<PlayerBuildingController>();
            foreach(PlayerBuilding playerBuilding in playerBuildingController.GetPlayerBuildings(this))
            {
                if(!playerBuilding.IsInConstrution())
                {
                    IEnumerable<BuildingEffectConfig> effects = playerBuilding.GetEffects();
                    foreach (BuildingEffectConfig effect in effects)
                    {
                        systemGold += effect.GetPlayerGold();
                        percentageAdjustment += effect.GetPlayerGoldPerc();
                    }
                }

            }
            goldChange += Convert.ToInt32(systemGold * (percentageAdjustment / 100f));
        }
        gold += goldChange;
    }

    private void OnSystemChange(SolarSystem system)
    {
        UpdateStats();
    }


    private void OnLeaderLoyaltyChange(Leader leader)
    {
        if(leadersControlled.Contains(leader))
        {
            leadersControlled.Remove(leader);
            if (empires.Contains(leader.GetEmpire()))
            {
                RemoveEmpire(leader.GetEmpire());
            }
        }

        if(leader.ControlledBy() == this)
        {
            leadersControlled.Add(leader);
            if (!empires.Contains(leader.GetEmpire()))
            {
                AddEmpire(leader.GetEmpire());
            }
        }
        UpdateStats();
    }

    private void OnLeaderDeath(Leader leader)
    {
        if(leadersControlled.Contains(leader))
        {
            leadersControlled.Remove(leader);
        }
        UpdateStats();
    }

    private void OnEmpireLeaderChange(Empire empire, Leader leader)
    {
        if (leader.ControlledBy() == this)
        {
            if (!empires.Contains(empire))
            {
                AddEmpire(empire);
            }
        }
        else
        {
            if (empires.Contains(empire))
            {
                RemoveEmpire(empire);
            }
        }
        UpdateStats();
    }

}
