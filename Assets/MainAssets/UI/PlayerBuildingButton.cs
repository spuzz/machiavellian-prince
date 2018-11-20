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
    public void UpdateButtons(PlayerBuilding playerBuilding)
    {
        button.gameObject.SetActive(true);
        upgradeButton.gameObject.SetActive(true);
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
        button.gameObject.SetActive(true);
        upgradeButton.gameObject.SetActive(true);
        icon.sprite = DefaultImage;
        button.interactable = true;
        upgradeButton.interactable = false;

    }

    public void HideButton()
    {
        
        button.interactable = false;
        button.gameObject.SetActive(false);
        upgradeButton.interactable = false;
        upgradeButton.gameObject.SetActive(false);
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
