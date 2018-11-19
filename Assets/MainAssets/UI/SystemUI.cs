using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SystemUI : MonoBehaviour {

    [SerializeField] SolarSystem system;

    [SerializeField] TextMeshProUGUI systemName;
    [SerializeField] Button buildQueue;
    [SerializeField] Image buildProgress;

    [SerializeField] Empire empire;

    [SerializeField] ArmyUI armyUI;

    [SerializeField] Player player;

    [SerializeField] List<PlayerBuildingButton> playerBuildingButtons;
    //EmpireUI empireUI;
    BuildController buildController;
    PlayerBuildingController playerBuildingController;
    PlayerBuildingDialog playerBuildingDialog;
    IEnumerable<PlayerBuilding> playerBuildings;
    private void Awake()
    {
        playerBuildingController = GetComponent<PlayerBuildingController>();
        buildController = system.GetComponent<BuildController>();
        playerBuildingDialog = FindObjectOfType<PlayerBuildingDialog>();
        playerBuildingButtons = new List<PlayerBuildingButton>(GetComponentsInChildren<PlayerBuildingButton>());
    }
    void Start()
    {
        var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.onMouseOverSystem += ProcessMouseOverSystem;

            
        armyUI = FindObjectOfType<ArmyUI>();
    }

    void Update()
    {
        empire = system.GetEmpire();
        
        systemName.text = system.GetName().ToString();
        if (!buildController.IsBuilding())
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

        foreach (PlayerBuilding building in playerBuildings)
        {
            playerBuildingButtons[building.GetBuildingNumber()].TurnOnButton(building);
        }
    }


    public SolarSystem GetSystem()
    {
        return system;
    }

    public void UpdateSystem(SolarSystem nSystem)
    {
        this.system = nSystem;
        playerBuildings = system.GetComponent<PlayerBuildingController>().GetPlayerBuildings(player);

        for(int buildingNumber = 0; buildingNumber < 5; buildingNumber++)
        {
            playerBuildingButtons[buildingNumber].TurnOffButton();
        }

    }

    public void BuildingSelected(PlayerBuildingButton playerBuildingButton, bool isEmpty)
    {
        if(isEmpty)
        {
            int buildingNumber = playerBuildingButtons.FindIndex(c => c == playerBuildingButton);
            playerBuildingDialog.Activate(system, player,buildingNumber);
        }
    }
    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    private void ProcessMouseOverSystem(SolarSystem system)
    {
        if (Input.GetMouseButton(0) == true)
        {
            UpdateSystem(system);
        }
    }
}
