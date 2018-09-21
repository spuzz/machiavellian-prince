using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableComponent : MonoBehaviour {

    [Tooltip("The UI canvas prefab")]
    [SerializeField]
    GameObject enemyCanvasPrefab = null;
    string name;
    string unitSelecterText;
    Camera cameraToLookAt;
    bool changeText = false;
    Button button;
    Text text;
    Button unitSelecter;
    Canvas canvas;
    HUD hud;
    float scale = 0.1f;
    void Start()
    {
        cameraToLookAt = Camera.main;
        Instantiate(enemyCanvasPrefab, transform.position, Quaternion.identity, transform);
        canvas = GetComponentInChildren<Canvas>();

        text = canvas.transform.Find("Button").GetComponentInChildren<Text>();
        button = canvas.transform.Find("Button").GetComponent<Button>();
        unitSelecter = canvas.transform.Find("UnitSelecter").GetComponent<Button>();
        button.onClick.AddListener(Selected);
        unitSelecter.onClick.AddListener(UnitSelected);
        hud = FindObjectOfType<HUD>();
        unitSelecter.enabled = false;

    }


    public void SetScale(float scale)
    {
        this.scale = scale;
        changeText = true;
    }

    public void SetUnitSelectorText(string text)
    {
        unitSelecterText = text;
        changeText = true;
    }

    public void UpdateName(string name)
    {
        this.name = name;
        changeText = true;
    }

    private void LateUpdate()
    {
        if(changeText == true)
        {

            text.text = name;
            button.GetComponentInParent<Canvas>().transform.localScale = new Vector3(scale, scale, scale);
            if(unitSelecterText != "")
            {
                unitSelecter.enabled = true;
                unitSelecter.GetComponentInChildren<Text>().text = unitSelecterText;
            }
            
            changeText = false;
        }

        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
    }

    public void Selected()
    {
        hud.SelectObject(transform.parent.gameObject);
        //SolarSystem system = GetComponentInParent<SolarSystem>();
        //if (system)
        //{
        //    systemUI.SetSystem(system);
        //}
    }

    private void UnitSelected()
    {
        Console.Write("Test");
    }

}
