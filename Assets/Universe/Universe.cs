using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Universe : MonoBehaviour {

    [SerializeField] float timePerDay = 1.0f;
    [SerializeField] float maxTravelDistance = 1000.0f;
    public int mainPlayer = 1;
    int currentDay;
    public delegate void OnSystemOwnerChanged(SolarSystem system); // declare new delegate type
    public event OnSystemOwnerChanged onSystemOwnerChanged; // instantiate an observer set
    float startTime;
    private GameObject selected;
    private Shader shaderOutline;
    private Shader shaderNoOutline;
    private int xSize = 100;
    private int ySize = 100;

    public float GetMaxTravelDistance()
    {
        return maxTravelDistance;
    }
    void Start () {
        currentDay = 1;
        shaderOutline = Shader.Find("Outlined/Uniform");
        shaderNoOutline = Shader.Find("Standard");
        var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.onMouseOverSystem += ProcessMouseOverSystem;
    }


    private void ProcessMouseOverSystem(SolarSystem system)
    {
        if(Input.GetMouseButton(0) == true)
        {
            
            ClearSelection();
            selected = system.gameObject;
            system.GetComponent<Renderer>().material.shader = shaderOutline;
        }

    }

    public void SystemChange(SolarSystem system)
    {
        onSystemOwnerChanged(system);
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

    public void CheckEndGame()
    {
        Empire[] empires = FindObjectsOfType<Empire>();
        int empiresAlive = 0;
        foreach(Empire empire in empires)
        {
            if(empire.IsAlive() == true)
            {
                empiresAlive += 1;
            }
        }
        if (empiresAlive <= 1)
        {
            SceneManager.LoadScene(0);
        }
    }
    public delegate void OnDayChanged(int days); // declare new delegate type
    public event OnDayChanged onDayChanged; // instantiate an observer set
}
