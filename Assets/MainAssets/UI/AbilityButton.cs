using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour {

    [SerializeField] AbilityConfig abilityConfig;
    [SerializeField] Image icon;
    MissionDialog missionDialog;
    Image buttonImage;
    Button button;
    HumanController humanController;
    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        missionDialog = FindObjectOfType<MissionDialog>();
        humanController = FindObjectOfType<HumanController>();
        if (abilityConfig)
        {
            TurnOnButton();
        }
        else
        {
            TurnOffButton();
        }

        
    }

    public void SetAbility(AbilityConfig ability)
    {
        abilityConfig = ability;
    }

    public void ActivateAbility()
    {

        Agent selectedAgent = humanController.GetSelectedAgent();
        if (selectedAgent && abilityConfig)
        {
            missionDialog.UseAbility(abilityConfig, selectedAgent,selectedAgent.GetCurrentSystem().gameObject);
        }
       
    }

    public void TurnOnButton()
    {
        button.interactable = true;
        buttonImage.enabled = true;
        icon.enabled = true;
        icon.sprite = abilityConfig.GetButtonImage();
    }

    public void SetInteractable(bool interact)
    {
        button.interactable = interact;
    }


    public void TurnOffButton()
    {
        button.interactable = false;
        buttonImage.enabled = false;
        icon.enabled = false;
        icon.sprite = null;
    }
}
