using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Universe : MonoBehaviour {

    enum Speed
    {
        PAUSE,
        SLOW,
        NORMAL,
        FAST,
        SKIP
    }

    [SerializeField] Speed speed = Speed.NORMAL;
    [SerializeField] float timePerDay = 1.0f;
    [SerializeField] float maxTravelDistance = 1000.0f;
    [SerializeField] float borderDistance = 20.0f;
    [SerializeField] Dictionary<Speed, float> speedValues = new Dictionary<Speed, float>(); 
    public int mainPlayer = 1;
    int currentDay;
    public delegate void OnSystemOwnerChanged(SolarSystem system); // declare new delegate type
    public event OnSystemOwnerChanged onSystemOwnerChanged; // instantiate an observer set
    private int xSize = 100;
    private int ySize = 100;
    float timeSinceLastDay;

    private GameObject selected;
    private Shader shaderOutline;
    private Shader shaderNoOutline;

    private float currentSpeed;
    SolarSystem[] systems;
    int maxSpeed = Enum.GetValues(typeof(Speed)).Cast<int>().Max();

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
        speedValues.Add(Speed.PAUSE, 0);
        speedValues.Add(Speed.SLOW, 0.5f);
        speedValues.Add(Speed.NORMAL, 1);
        speedValues.Add(Speed.FAST, 2);
        speedValues.Add(Speed.SKIP, 5);
        
        CreateUniverse();
        currentSpeed = speedValues[speed];
        timeSinceLastDay = 0;
    }

    public float GetSpeed()
    {
        return currentSpeed;
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

        currentSpeed = speedValues[speed];
        timeSinceLastDay += Time.deltaTime * currentSpeed;
        int daysPassed = 0;
        while(timeSinceLastDay > timePerDay)
        {
            daysPassed++;
            currentDay++;
            timeSinceLastDay -= timePerDay;
        }

        if(daysPassed > 0)
        {
            onDayChanged(daysPassed);
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

    public void IncreaseSpeed()
    {
        
        if (Enum.IsDefined(typeof(Speed),speed+1))
        {
            speed += 1;
        }
    }

    public void DecreaseSpeed()
    {
        if(speed > 0)
        {
            speed -= 1;
        }
    }

    public delegate void OnDayChanged(int days); // declare new delegate type
    public event OnDayChanged onDayChanged; // instantiate an observer set
}
