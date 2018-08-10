using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Universe : MonoBehaviour {

    [SerializeField] float timePerDay = 1.0f;
    [SerializeField] float maxTravelDistance = 1000.0f;
    [SerializeField] float borderDistance = 20.0f;
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
    SolarSystem[] systems;

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

        CreateUniverse();
    }

    private void CreateUniverse()
    {
        GenerateSystems();
        GenerateBorders();
    }


    // TODO: temp code to get systems from demo scene
    private void GenerateSystems()
    {
        systems = FindObjectsOfType<SolarSystem>();


    }
    private void GenerateBorders()
    {
        List<BorderController> borders = new List<BorderController>();
        foreach (SolarSystem system in systems)
        {
            BorderController border = system.transform.Find("Border").GetComponent<BorderController>();
            border.SetBorderDistance(borderDistance);
            foreach (SolarSystem possibleNeighbour in systems)
            {
                if (possibleNeighbour != system && Vector3.Distance(possibleNeighbour.transform.position, system.transform.position) <= borderDistance * 2)
                {
                    border.AddNearbySystem(possibleNeighbour);
                }
            }

            borders.Add(border);
        }

        foreach (BorderController border in borders)
        {
            bool result = true;
            while (result)
            {
                result = border.GrowBorder();
            }
            border.CreateBorderMesh();
        }
        foreach (SolarSystem system in systems)
        {
            system.UpdateBorders();
        }
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
