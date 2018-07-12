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

    // Food Details
    [SerializeField] Text m_foodAvailable;
    [SerializeField] Text m_foodProduction;
    [SerializeField] Text m_foodConsumption;

    // Power Details
    [SerializeField] Text m_powerAvailable;
    [SerializeField] Text m_powerProduction;
    [SerializeField] Text m_powerConsumption;

    [SerializeField] ScrollRect m_resources;

    [SerializeField] Leader m_leader;
    // Use this for initialization
    void Start()
    {
        var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.onMouseOverPlanet += ProcessMouseOverPlanet;
    }

    // Update is called once per frame
    void Update () {
        m_totalPopulation.text = planet.GetCurrentPopulation().ToString();
        m_growthRate.text = planet.GetGrowthRate().ToString("0.00");
        m_happyPopulation.text = planet.GetHappyPopPerc().ToString("0.00");

        m_foodAvailable.text = planet.GetFoodAvailable().ToString();
        m_foodProduction.text = planet.GetFoodProduction().ToString("0.00");
        m_foodConsumption.text = planet.GetFoodConsumption().ToString("0.00");

        m_powerAvailable.text = planet.GetPowerAvailable().ToString();
        m_powerProduction.text = planet.GetPowerProduction().ToString("0.00");
        m_powerConsumption.text = planet.GetPowerConsumption().ToString("0.00");

        m_leader = planet.GetLeader();
    }


    private void ProcessMouseOverPlanet(Planet planet)
    {
        if (Input.GetMouseButton(0) == true)
        {
            StopAllCoroutines();
            this.planet = planet;
        }
    }
}
