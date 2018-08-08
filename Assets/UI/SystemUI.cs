using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SystemUI : MonoBehaviour {

    [SerializeField] SolarSystem system;

    [SerializeField] TextMeshProUGUI systemName;
    [SerializeField] TextMeshProUGUI empireName;
    [SerializeField] TextMeshProUGUI pop;
    [SerializeField] TextMeshProUGUI food;
    [SerializeField] TextMeshProUGUI power;
    [SerializeField] TextMeshProUGUI armies;
    [SerializeField] TextMeshProUGUI offence;
    [SerializeField] TextMeshProUGUI defence;
    [SerializeField] TextMeshProUGUI economy;

    [SerializeField] Empire empire;
    EmpireUI empireUI;
    // Use this for initialization

    public SolarSystem GetSystem()
    {
        return system;
    }
    void Start()
    {
        var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.onMouseOverSystem += ProcessMouseOverSystem;

        empireUI = FindObjectOfType<EmpireUI>();
    }

    // Update is called once per frame
    void Update () {
        empire = system.GetEmpire();
        pop.text = system.GetCurrentPopulation().ToString();
        food.text = system.GetFoodAvailable().ToString();
        power.text = system.GetPowerAvailable().ToString();
        systemName.text = system.GetName().ToString();
        if(empire)
        {
            empireName.text = empire.GetName().ToString();
            empireUI.SetEmpire(empire);
            empireUI.gameObject.SetActive(true);
        }
        else
        {
            empireName.text = "None";
            empireUI.gameObject.SetActive(false);
        }
        armies.text = system.GetTotalArmies().ToString();
        offence.text = system.GetOffence().ToString();
        defence.text = system.GetDefence().ToString();
        economy.text = system.GetNetIncome().ToString();
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
