using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemUI : MonoBehaviour {

    [SerializeField] SolarSystem system;

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

    [SerializeField] Empire m_empire;
    // Use this for initialization

    public SolarSystem GetSystem()
    {
        return system;
    }
    void Start()
    {
        var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.onMouseOverSystem += ProcessMouseOverSystem;
    }

    // Update is called once per frame
    void Update () {
        m_totalPopulation.text = system.GetCurrentPopulation().ToString();
        m_growthRate.text = system.GetGrowthRate().ToString("0.00");
        m_happyPopulation.text = system.GetHappyPopPerc().ToString("0.00");

        m_foodAvailable.text = system.GetFoodAvailable().ToString();
        m_foodProduction.text = system.GetFoodProduction().ToString("0.00");
        m_foodConsumption.text = system.GetFoodConsumption().ToString("0.00");

        m_powerAvailable.text = system.GetPowerAvailable().ToString();
        m_powerProduction.text = system.GetPowerProduction().ToString("0.00");
        m_powerConsumption.text = system.GetPowerConsumption().ToString("0.00");

        m_empire = system.GetEmpire();
    }


    private void ProcessMouseOverSystem(SolarSystem system)
    {
        if (Input.GetMouseButton(0) == true)
        {
            StopAllCoroutines();
            this.system = system;
        }
    }
}
