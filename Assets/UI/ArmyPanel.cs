using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmyPanel : MonoBehaviour {

    [SerializeField] Army army;

    [SerializeField] TextMeshProUGUI armyName;
    [SerializeField] TextMeshProUGUI status;
    [SerializeField] TextMeshProUGUI offence;
    [SerializeField] TextMeshProUGUI defence;
    [SerializeField] Button buildQueue;
    [SerializeField] Image buildProgress;
    ArmyTrainer armyTrainer;


    public void SetArmy(Army army)
    {
        this.army = army;
    }

    void Start()
    {
        var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.onMouseOverSystem += ProcessMouseOverSystem;

    }

    // Update is called once per frame
    void Update () {
        if(army)
        {
            armyTrainer = army.GetComponent<ArmyTrainer>();
            armyName.text = army.GetName();
            offence.text = army.GetAttackValue().ToString();
            defence.text = army.GetDefenceValue().ToString();
            status.text = army.GetArmyStatus().ToString();
        }

        if(armyTrainer)
        {
            if (!armyTrainer.IsBuilding())
            {
                buildQueue.gameObject.SetActive(false);
                buildProgress.gameObject.SetActive(false);
            }
            else
            {
                buildQueue.gameObject.SetActive(true);
                buildQueue.GetComponent<Image>().sprite = armyTrainer.GetUnitBuilding().GetPortraitIcon();
                buildProgress.gameObject.SetActive(true);
                buildProgress.fillAmount = 1.0f - (float)armyTrainer.GetDaysLeftToBuild() / (float)armyTrainer.GetUnitBuilding().GetBuildTime();
            }
        }
    }

    private void ProcessMouseOverSystem(SolarSystem system)
    {
        if (Input.GetMouseButton(0) == true)
        {
           // this.system = system;
        }
    }
}
