using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour{

	Dictionary<Player, float> m_playerInfluence = new Dictionary<Player, float>();
    [SerializeField] string leaderName;
    const float MinimumInfluence = 100f;
    const float MinimumInfluenceToOverthrow = 1.1f;
    public Player m_controlledBy;
    [SerializeField] PersonalityConfig personality;
    [SerializeField] State currentState;
    Empire empire;
    bool inControl;
    Universe universe;

    bool init = true;
    void Start()
    {
        personality.AddComponent(gameObject);
        empire = GetComponentInParent<Empire>();
        universe = FindObjectOfType<Universe>();
    }

    private void Update()
    {
        if(init)
        {
            if(m_controlledBy)
            {
                UpdateLoyalty(m_controlledBy);
            }
            init = false;
        }
    }

    public string GetName()
    {
        return leaderName;
    }

    public Empire GetEmpire()
    {
        return empire;
    }

    public Player ControlledBy()
    {
        return m_controlledBy;
    }

    public bool IsInControl()
    {
        return inControl;
    }

    public void SetInControl(bool control)
    {
        inControl = control;
    }

    private void OnDestroy()
    {
        KillLeader();
    }

    public void KillLeader()
    {
        universe.LeaderDeath(this);
    }

    public void LeadEmpire(EmpireController empireController)
    {
        personality.MakeDecisions(empire, empireController,ref currentState);
    }

    public void IncreaseInfluence(Player player, float value)
    {
        if(m_playerInfluence.ContainsKey(player))
        {
            m_playerInfluence[player] += value;
        }
        else
        {
            m_playerInfluence[player] = value;
        }

        if(!m_controlledBy) 
        {
            if(m_playerInfluence[player] >= MinimumInfluence)
            {
                UpdateLoyalty(player);
            }

        }
        else
        {
            if(m_playerInfluence[player] >= m_playerInfluence[m_controlledBy] * MinimumInfluenceToOverthrow)
            {
                UpdateLoyalty(player);
            }
        }
    }

    private void UpdateLoyalty(Player player)
    {
        m_controlledBy = player;
        universe.LeaderLoyaltyChange(this);
    }
}
