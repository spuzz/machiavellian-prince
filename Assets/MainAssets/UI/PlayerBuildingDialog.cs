using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuildingDialog : MonoBehaviour {

    [SerializeField] List<PlayerBuildingConfig> buildingOptions;
    List<BuildingOptionButton> buttons;
    SystemUI systemUI;
    Player currentPlayer;
    SolarSystem currentSystem;
    int buildingNumber;
    private void Awake()
    {
        systemUI = FindObjectOfType<SystemUI>();
        buttons = new List<BuildingOptionButton>(GetComponentsInChildren<BuildingOptionButton>());
    }

    void Start () {
        gameObject.SetActive(false);
	}

    public void Activate(SolarSystem system, Player player, int buildingNumber)
    {
        this.buildingNumber = buildingNumber;
        currentSystem = system;
        currentPlayer = player;
        gameObject.SetActive(true);
        int buttonCount = 0;
        foreach(PlayerBuildingConfig playerBuildingConfig in buildingOptions)
        {
            buttons[buttonCount].SetBuildingConfig(playerBuildingConfig);
            buttonCount++;
        }
        while(buttonCount < buttons.Count)
        {
            buttons[buttonCount].HideButton();
            buttonCount++;
        }
    }

    public void Deactivate()
    {
        currentSystem = null;
        currentPlayer = null;
        gameObject.SetActive(false);
    }

    public void SelectBuilding(PlayerBuildingConfig playerBuildingConfig)
    {
        currentSystem.GetComponent<PlayerBuildingController>().BuildPlayerBuilding(playerBuildingConfig, currentPlayer, buildingNumber);
        systemUI.UpdateSystem(currentSystem);
        Deactivate();
    }
}
