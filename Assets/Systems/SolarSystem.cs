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

    [SerializeField] Empire empire;

    [SerializeField] GameObject travelRoutePrefab;
    GameObject border;
    Player m_owner;
    List<Agent> agents = new List<Agent>();
    List<Army> armies = new List<Army>();
    Dictionary<SolarSystem, float> nearbySystems = new Dictionary<SolarSystem, float>();

    public int GetCurrentPopulation() { return m_currentPopulation; }
    public float GetGrowthRate() { return m_growthRate; }
    public float GetHappyPopPerc() { return m_happyPopulationPerc; }
    public float GetFoodAvailable() { return m_foodAvailable; }
    public float GetFoodProduction() { return m_foodProduction; }
    public float GetFoodConsumption() { return m_foodConsumption; }
    public float GetPowerAvailable() { return m_powerAvailable; }
    public float GetPowerProduction() { return m_powerProduction; }
    public float GetPowerConsumption() { return m_powerConsumption; }

    float theta_scale = 0.01f;        //Set lower to add more points
    int size; //Total number of points in circle
    float radius = 3f;

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

    public void AddArmy(Army army)
    {
        armies.Add(army);
    }

    public void RemoveArmy(Army army)
    {
        armies.Remove(army);
    }

    // TODO - battle component
    public void Defend(Army attackingArmy)
    {
        int totalDefence = 0;
        foreach(Army army in armies)
        {
            totalDefence += army.GetDefenceValue();
        }


        if(attackingArmy.GetAttackValue() > totalDefence)
        {
            int attackLeft = attackingArmy.GetAttackValue() - totalDefence;
            float percLost = ((float)attackLeft / (float)attackingArmy.GetAttackValue());
            attackingArmy.DepleteArmy(percLost);

            foreach (Army army in armies)
            {
                army.DestroyArmy();
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
        this.empire.TakeSystem(this);
        empire.GiveSystem(this);
        this.empire = empire;
        UpdateBorders();
    }

    public Dictionary<SolarSystem, float> GetNearbySystems()
    {
        return nearbySystems;
    }
    private void UpdateBorders()
    {
        if(empire)
        {
            border.GetComponent<Renderer>().material.color = new Color(this.empire.GetColor().r, this.empire.GetColor().g, this.empire.GetColor().b, 0.5f);
        }
        else
        {
            border.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0.5f);
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

    void Awake()
    {
        border = transform.Find("Border").gameObject;
        UpdateBorders();
        if (empire)
        {
            
            empire.GiveSystem(this);
        }
    }


    
    public void Start()
    {
        
        m_universe = FindObjectOfType<Universe>();
        m_universe.onDayChanged += ProcessDayChange;
        FindNearbySystems();

    }

    private void FindNearbySystems()
    {
        SolarSystem[] systems = SolarSystem.FindObjectsOfType<SolarSystem>();
        foreach(SolarSystem system in systems)
        {
            float distance = Vector3.Distance(transform.position, system.transform.position);
            if(distance > 0 && distance <= m_universe.GetMaxTravelDistance())
            {
                nearbySystems.Add(system,distance);
                CreateLineRenderer(system);
            }
        }
    }

    private void CreateLineRenderer(SolarSystem destination)
    {
        var travelRoute = Instantiate(travelRoutePrefab, transform.position, transform.rotation);
        LineRenderer lineRenderer = travelRoute.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, destination.transform.position);
        lineRenderer.transform.SetParent(transform.Find("TravelRoutes"));
    }

    void Update()
    {
        SetOwner();
    }


    private void ProcessDayChange(int days)
    {
        for(int day = 0; day < days; day++)
        {
            m_currentPopulation += Mathf.FloorToInt(m_currentPopulation * m_growthRate);
            m_growthRate += UnityEngine.Random.Range(-0.05f, 0.05f);
            m_happyPopulationPerc += UnityEngine.Random.Range(-1, 2);

            CalculateConsumption();
            m_foodAvailable = m_foodAvailable + m_foodProduction - m_foodConsumption;
            m_powerAvailable = m_powerAvailable + m_powerProduction - m_powerConsumption;

            CalculateProduction();
        }
        
    }

    private void CalculateProduction()
    {
        if(empire)
        {
            empire.AddGold(100);
            empire.AddBuildingMaterial(10);
            empire.AddSpaceshipMaterial(10);
        }

    }

    private void CalculateConsumption()
    {
        m_powerConsumption = m_currentPopulation * 0.1f;
        m_foodConsumption = m_currentPopulation * 1f;
    }

    public void UpdateStats()
    {
        
    }
}
