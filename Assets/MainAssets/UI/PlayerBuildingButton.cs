using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuildingButton : MonoBehaviour {

    [SerializeField] Image icon;
    [SerializeField] Image buttonImage;
    [SerializeField] Button button;
    [SerializeField] Image upgradeIcon;
    [SerializeField] Image upgradeButtonImage;
    [SerializeField] Button upgradeButton;
    [SerializeField] Sprite DefaultImage;
    PlayerBuilding playerBuilding;
    SystemUI systemUI;

    private void Awake()
    {
        systemUI = FindObjectOfType<SystemUI>();
    }
    public void TurnOnButton(PlayerBuilding playerBuilding)
    {
        button.interactable = true;
        icon.sprite = playerBuilding.GetImage();
        upgradeButton.interactable = true;
    }

    public void SetInteractable(bool interact)
    {
        button.interactable = interact;
        upgradeButton.interactable = interact;
    }


    public void TurnOffButton()
    {
        icon.sprite = DefaultImage;
        upgradeButton.interactable = false;
    }

    public void OnButtonClick()
    {
        if(!playerBuilding)
        {
            systemUI.BuildingSelected(this, true);
        }
        else
        {
            systemUI.BuildingSelected(this, false);
        }
    }
}
