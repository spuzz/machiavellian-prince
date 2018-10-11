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
    [SerializeField] Button buildQueue;
    [SerializeField] Image buildProgress;

    [SerializeField] Empire empire;

    [SerializeField] ArmyUI armyUI;

    EmpireUI empireUI;
    BuildController buildController;
    

    public SolarSystem GetSystem()
    {
        return system;
    }

    public void SetSystem(SolarSystem nSystem)
    {
        this.system = nSystem;

       
    }

    void Start()
    {
        var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.onMouseOverSystem += ProcessMouseOverSystem;

        empireUI = FindObjectOfType<EmpireUI>();
        armyUI = FindObjectOfType<ArmyUI>();
    }

    // Update is called once per frame
    void Update () {
        empire = system.GetEmpire();
        buildController = system.GetComponent<BuildController>();
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
        if(!buildController.IsBuilding())
        {
            buildQueue.gameObject.SetActive(false);
            buildProgress.gameObject.SetActive(false);
        }
        else
        {
            buildQueue.gameObject.SetActive(true);
            buildQueue.GetComponent<Image>().sprite = buildController.GetBuildingInConstruction().GetPortraitIcon();
            buildProgress.gameObject.SetActive(true);
            buildProgress.fillAmount = 1.0f - (float)buildController.GetDaysLeftToBuild() / (float)buildController.GetBuildingInConstruction().GetBuildTime();
        }
        armyUI.SetArmies(system.GetArmies());
    }



    private void ProcessMouseOverSystem(SolarSystem system)
    {
        if (Input.GetMouseButton(0) == true)
        {
            SetSystem(system);
        }
    }
}
