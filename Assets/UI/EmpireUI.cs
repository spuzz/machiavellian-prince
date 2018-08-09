using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmpireUI : MonoBehaviour {


    [SerializeField] TextMeshProUGUI empireName;
    [SerializeField] TextMeshProUGUI leader;
    [SerializeField] TextMeshProUGUI status;
    [SerializeField] TextMeshProUGUI systems;
    [SerializeField] TextMeshProUGUI armies;
    [SerializeField] TextMeshProUGUI offence;
    [SerializeField] TextMeshProUGUI defence;
    [SerializeField] TextMeshProUGUI economy;

    [SerializeField] Empire empire;
    // Use this for initialization

    public void SetEmpire(Empire empire)
    {
        this.empire = empire;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update () {
        if(empire)
        {
            DiplomacyController diplomacy = empire.GetComponent<DiplomacyController>();
            empireName.text = empire.GetName();
            leader.text = empire.GetLeader().GetName();
            if(diplomacy.GetEmpiresAtWar().Count > 0)
            {
                status.text = "War";
            }
            else
            {
                status.text = "Peace";
            }
            
            systems.text = empire.GetSystems().Count.ToString();
            armies.text = empire.GetArmies().Count.ToString();
            offence.text = empire.GetTotalOffence().ToString();
            defence.text = empire.GetTotalDefence().ToString();
            economy.text = empire.GetGold().ToString();
        }
    }

}
