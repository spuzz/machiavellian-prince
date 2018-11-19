using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingOptionButton : MonoBehaviour {

    [SerializeField] PlayerBuildingDialog playerBuildingDialog;

    [SerializeField] Image iconImage;
    PlayerBuildingConfig playerBuildingConfig;
    private void Awake()
    {
        playerBuildingDialog = FindObjectOfType<PlayerBuildingDialog>();

    }
    public void SetBuildingConfig(PlayerBuildingConfig playerBuildingConfig)
    {
        this.playerBuildingConfig = playerBuildingConfig;
        iconImage.sprite = playerBuildingConfig.GetBuildingImage();
    }

    public void HideButton()
    {
        gameObject.SetActive(false);
    }
    
    public void OnClickButton()
    {
        playerBuildingDialog.SelectBuilding(playerBuildingConfig);
    }
}
