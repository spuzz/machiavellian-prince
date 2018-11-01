using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour {

    [SerializeField] AbilityConfig abilityConfig;
    [SerializeField] Image icon;
    Image buttonImage;
    Button button;
    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();

        if(abilityConfig)
        {
            TurnOnButton();
        }
        else
        {
            TurnOffButton();
        }

        
    }

    public void ActivateAbility()
    {

    }

    public void TurnOnButton()
    {
        button.interactable = true;
        buttonImage.enabled = true;
        icon.enabled = true;
        icon.sprite = abilityConfig.GetButtonImage();
    }

    public void TurnOffButton()
    {
        button.interactable = false;
        buttonImage.enabled = false;
        icon.enabled = false;
        icon.sprite = null;
    }
}
