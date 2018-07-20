using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour {

    [SerializeField] float timePerDay = 1.0f;
    public int mainPlayer = 1;
    int currentDay;

    float startTime;
    private GameObject selected;
    private Shader shaderOutline;
    private Shader shaderNoOutline;

    void Start () {
        currentDay = 1;
        shaderOutline = Shader.Find("Outlined/Uniform");
        shaderNoOutline = Shader.Find("Standard");
        var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.onMouseOverPlanet += ProcessMouseOverPlanet;
    }


    private void ProcessMouseOverPlanet(Planet planet)
    {
        if(Input.GetMouseButton(0) == true)
        {
            
            ClearSelection();
            selected = planet.gameObject;
            planet.GetComponent<Renderer>().material.shader = shaderOutline;
        }

    }

    
    private void ClearSelection()
    {
        if(selected)
        {
            selected.GetComponent<Renderer>().material.shader = shaderNoOutline;
        }
        
    }

    // Update is called once per frame
    void Update () {
        int newDay = Mathf.FloorToInt(Time.time / timePerDay);
        if(newDay != currentDay)
        {
            onDayChanged(newDay - currentDay);
            currentDay = newDay;
        }

    }

    public delegate void OnDayChanged(int days); // declare new delegate type
    public event OnDayChanged onDayChanged; // instantiate an observer set
}
