using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour {
    [SerializeField] string m_name;
    [SerializeField] Universe m_universe;
    [SerializeField] int m_currentPopulation;
    [Range(0, 100)] [SerializeField] float m_happyPopulationPerc;

    [SerializeField] float m_powerAvailable;
    [SerializeField] float m_powerProduction;
    [SerializeField] float m_powerConsumption;

    [SerializeField] float m_foodAvailable;
    [SerializeField] float m_foodProduction;
    [SerializeField] float m_foodConsumption;

    [SerializeField] float m_growthRate;

    [SerializeField] int income = 100;
    [SerializeField] int maintenance = 50;

    [SerializeField] Empire empire;
    [SerializeField] int baseDefence = 100;
    [SerializeField] GameObject travelRoutePrefab;
    GameObject border;
    Player m_owner;
    List<Agent> agents = new List<Agent>();
    List<Army> armies = new List<Army>();
    List<SolarSystem> nearbySystems = new List<SolarSystem>();
    List<TravelRoute> travelRoutes = new List<TravelRoute>();

    bool initSystem = true;
    public int GetCurrentPopulation() { return m_currentPopulation; }
    public float GetGrowthRate() { return m_growthRate; }
    public float GetHappyPopPerc() { return m_happyPopulationPerc; }
    public float GetFoodAvailable() { return m_foodAvailable; }
    public float GetFoodProduction() { return m_foodProduction; }
    public float GetFoodConsumption() { return m_foodConsumption; }
    public float GetPowerAvailable() { return m_powerAvailable; }
    public float GetPowerProduction() { return m_powerProduction; }
    public float GetPowerConsumption() { return m_powerConsumption; }


    public string GetName()
    {
        return m_name;
    }

    public void AddAgent(Agent agent)
    {
        agents.Add(agent);
    }

    public List<Agent> GetAgents()
    {
        return agents;
    }

    public Empire GetEmpire()
    {
        return empire;
    }

    public List<Army> GetArmies()
    {
        return armies;
    }
    public void AddArmy(Army army)
    {
        
        armies.Add(army);
        GetComponentInChildren<SelectableComponent>().SetUnitSelectorText(armies.Count.ToString());
    }

    public void RemoveArmy(Army army)
    {
        
        armies.Remove(army);
        GetComponentInChildren<SelectableComponent>().SetUnitSelectorText(armies.Count.ToString());
    }

    public bool HasArmy()
    {
        if (armies.Count != 0)
            return true;
        return false;
    }

    public int GetTotalArmies()
    {
        return armies.Count;
    }
    public void MergeArmies()
    {
        while(armies.Count > 1)
        {
            armies[0].Merge(armies[1]);
        }
    }
    public int GetDefence()
    {
        int defence = baseDefence;
        foreach(Army army in armies)
        {
            defence += army.GetDefenceValue();
        }
        return defence;
    }

    public int GetOffence()
    {
        int offence = 0;
        foreach (Army army in armies)
        {
            offence += army.GetAttackValue();
        }
        return offence;
    }

    public void AddToArmy(int index, UnitConfig unitConfig)
    {
        if(index < armies.Count)
        {
            armies[index].Addunit(unitConfig);
        }
    }

    void Awake()
    {
        border = transform.Find("Border").gameObject;
        //UpdateBorders();
        if (empire)
        {

            empire.GiveSystem(this);
        }
    }



    public void Start()
    {

        m_universe = FindObjectOfType<Universe>();
        m_universe.onDayChanged += ProcessDayChange;
        GetComponentInChildren<SelectableComponent>().UpdateName(m_name);
        GetComponentInChildren<SelectableComponent>().SetUnitSelectorText("0");
        FindNearbySystems();

    }

    void Update()
    {
        SetOwner();
        if (initSystem == true && empire)
        {
            m_universe.SystemChange(this);
            initSystem = false;
        }
    }

    public void Colonised(Empire empire)
    {
        if(!this.empire)
        {
            SetEmpire(empire);
        }
    }

    // TODO - battle component
    public void Defend(Army attackingArmy)
    {
        int totalDefence = GetDefence();


        if (attackingArmy.GetAttackValue() > totalDefence)
        {
            int attackLeft = attackingArmy.GetAttackValue() - totalDefence;
            float percLost = ((float)attackLeft / (float)attackingArmy.GetAttackValue());
            attackingArmy.DepleteArmy(percLost);

            while (armies.Count != 0)
            {
                armies[0].DestroyArmy();
            }
            armies.Clear();
            SetEmpire(attackingArmy.GetEmpire());
            armies.Add(attackingArmy);
        }
        else
        {
            int defenceLeft = totalDefence - attackingArmy.GetAttackValue();
            if (defenceLeft == 0)
            {
                defenceLeft = 1;
            }
            float percLost = ((float)defenceLeft / (float)totalDefence);

            foreach (Army army in armies)
            {
                army.DepleteArmy(percLost);
            }
            attackingArmy.DestroyArmy();
        }

    }



    public void SetEmpire(Empire empire)
    {
        if(this.empire)
        {
            this.empire.TakeSystem(this);
        }
        empire.GiveSystem(this);
        this.empire = empire;
        UpdateBorders();
        m_universe.SystemChange(this);
        foreach (Army army in armies)
        {
            army.DestroyArmy();
        }
        armies.Clear();
    }

    public void DepleteArmies(float percLost)
    {
        foreach (Army army in armies)
        {
            army.DepleteArmy(percLost);
        }
    }

    public List<TravelRoute> GetTravelRoutes()
    {
        return travelRoutes;
    }

    public List<SolarSystem> GetNearbySystems()
    {
        return nearbySystems;
    }

    public void UpdateBorders()
    {
        if(empire)
        {
            border.GetComponent<MeshRenderer>().material.color = new Color(this.empire.GetColor().r, this.empire.GetColor().g, this.empire.GetColor().b, 0.2f);
        }
        else
        {
            border.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0.5f);
        }
    }

    public void RemoveAgent(Agent agent)
    {
        if(agents.Find(c => c.GetAgentName() == agent.GetAgentName()))
        {
            agents.Remove(agent);
        }
    }
    private void SetOwner()
    {
        if(empire)
        {
            m_owner = empire.GetLeader().m_controlledBy;
            if (m_owner)
            {
                GetComponent<Renderer>().material.SetColor("_OutlineColor", m_owner.GetPlayerColor());
            }
        }
        
    }



    private void FindNearbySystems()
    {
        SolarSystem[] systems = SolarSystem.FindObjectsOfType<SolarSystem>();
        foreach(SolarSystem system in systems)
        {
            float distance = Vector3.Distance(transform.position, system.transform.position);
            if(distance > 0 && distance <= m_universe.GetMaxTravelDistance())
            {
                nearbySystems.Add(system);
                CreateLineRenderer(system, distance);
            }
        }
    }

    private void CreateLineRenderer(SolarSystem destination, float distance)
    {

        TravelRoute route = destination.travelRoutes.Find(c => c.ContainsSystem(this) == true);
        if(!route)
        {
            var travelRoute = Instantiate(travelRoutePrefab, transform.position, transform.rotation);
            LineRenderer lineRenderer = travelRoute.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, destination.transform.position);
            lineRenderer.transform.SetParent(transform.Find("TravelRoutes"));
            route = travelRoute.GetComponent<TravelRoute>();
            route.systemOne = this;
            route.systemTwo = destination;
            route.SetDistance(distance);
        }
        travelRoutes.Add(route);
    }



    private void ProcessDayChange(int days)
    {
        for(int day = 0; day < days; day++)
        {
            m_currentPopulation = Mathf.FloorToInt(m_currentPopulation * (1.0f + (m_growthRate /100.0f)));
            m_growthRate += UnityEngine.Random.Range(-0.1f, 0.1f);
            m_happyPopulationPerc += UnityEngine.Random.Range(-1, 2);

            CalculateConsumption();
            m_foodAvailable = m_foodAvailable + m_foodProduction - m_foodConsumption;
            m_powerAvailable = m_powerAvailable + m_powerProduction - m_powerConsumption;

            CalculateProduction();
        }
        
    }

    public int GetNetIncome()
    {
        return income - maintenance;
    }
    private void CalculateProduction()
    {
        if(empire)
        {
            empire.AddGold(income - maintenance);
        }

    }

    private void CalculateConsumption()
    {
        //m_powerConsumption = m_currentPopulation * 0.1f;
        //m_foodConsumption = m_currentPopulation * 1f;
    }

    public void UpdateStats()
    {
        
    }
}
