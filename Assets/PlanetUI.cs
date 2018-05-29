using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUI : MonoBehaviour {

    [SerializeField] Planet planet;

    // Pop Details
    [SerializeField] Text m_totalPopulation;
    [SerializeField] Text m_growthRate;
    [SerializeField] Text m_happyPopulation;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        m_totalPopulation.text = planet.GetCurrentPopulation().ToString();
        m_growthRate.text = planet.GetGrowthRate().ToString("0.00");
        m_happyPopulation.text = planet.GetHappyPopPerc().ToString("0.00");
    }
}
