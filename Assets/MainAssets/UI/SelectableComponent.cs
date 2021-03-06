﻿using System;
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
    HumanController humanController;
    float scale = 0.1f;
    Color color = Color.white;
    bool showCanvas;
    bool visible = false;
    bool onMap = false;
    FogCamera fogCamera;

    private void Awake()
    {
        fogCamera = FindObjectOfType<FogCamera>();
    }
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
        humanController = FindObjectOfType<HumanController>();
    }


    public void SetColor(Color color)
    {
        this.color = color;
        changeText = true;
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

    public void SetShown(bool show)
    {
        changeText = true;
        onMap = show;
        if (visible && onMap)
        {
            showCanvas = true;
        }
        else
        {
            showCanvas = false;
        }
        
    }

    public void SetVisible(bool vis)
    {
        changeText = true;
        visible = vis;
        if(onMap && visible)
        {
            showCanvas = true;
        }
        else
        {
            showCanvas = false;
        }
    }
    private void LateUpdate()
    {
        if (visible)
        {
            if (fogCamera.IsGameObjectVisible(gameObject.transform.parent.gameObject) == false)
            {
                SetVisible(false);
            }
        }
        if (changeText == true)
        {
            canvas.enabled = showCanvas;
            button.GetComponent<Image>().color = color;
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
        transform.position = transform.parent.position + (Vector3.forward * (30 * this.scale)) + Vector3.up * 0.1f;


    }

    public void Selected()
    {
        hud.SelectObject(transform.parent.gameObject);
        humanController.SelectObject(transform.parent.gameObject);
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
