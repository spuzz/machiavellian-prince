using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmpireUI : MonoBehaviour {


    [SerializeField] TextMeshProUGUI empireName;
    [SerializeField] TextMeshProUGUI leader;

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
            empireName.text = empire.GetName();
            leader.text = empire.GetLeader().GetName();
        }
    }

}
