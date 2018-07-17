using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour{

	Dictionary<Player, float> m_playerInfluence = new Dictionary<Player, float>();
    [SerializeField] string leaderName;
    const float MinimumInfluence = 100f;
    const float MinimumInfluenceToOverthrow = 1.1f;
    public Player m_controlledBy;
    
    public string GetName()
    {
        return leaderName;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
                m_controlledBy = player;
            }
           
        }
        else
        {
            if(m_playerInfluence[player] >= m_playerInfluence[m_controlledBy] * MinimumInfluenceToOverthrow)
            {
                m_controlledBy = player;
            }
        }
    }
}
