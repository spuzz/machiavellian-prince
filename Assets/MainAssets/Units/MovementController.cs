using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {


    [SerializeField] float stoppingDistance = 1f;
    [SerializeField] SolarSystem systemLocation;
    [SerializeField] SolarSystem destinationSystem;
    [SerializeField] SolarSystem systemTarget;
    
    Universe universe;
    SpeedUI speedUI;


    [SerializeField] TravelRoute currentRoute;
    [SerializeField] float movementSpeed = 10f;
    Queue<SolarSystem> path = new Queue<SolarSystem>();
    Empire currentEmpire;
    bool blocking;
    bool internalMovementOnly = true;

    // New Delegates
    public delegate void OnReachedSystem(SolarSystem system); // declare new delegate type
    public event OnReachedSystem onReachedSystem; // instantiate an observer set

    public delegate void OnLeaveSystem(SolarSystem system); // declare new delegate type
    public event OnLeaveSystem onLeaveSystem; // instantiate an observer set

    void Start()
    {

        if (systemLocation)
        {
            transform.position = systemLocation.transform.position;
        }
        currentEmpire = gameObject.GetComponentInParent<Empire>();
        universe = FindObjectOfType<Universe>();
        speedUI = FindObjectOfType<SpeedUI>();
        blocking = false;
    }

    void Update()
    {

        if (currentRoute)
        {
            CheckReachedTarget();
        }
        else
        {
            FindNextMove();
        }

    }

    public void OnDestroy()
    {
        if(IsMoving())
        {
            if(blocking)
            {
                currentRoute.finishUsingRoute(currentEmpire);
            }
        }
    }


    public void SetLocation(SolarSystem system)
    {
        if(systemLocation != system)
        {
            this.systemLocation = system;
            transform.position = systemLocation.transform.position;
            if (currentRoute && blocking)
            {
                currentRoute.finishUsingRoute(currentEmpire);
            }
            currentRoute = null;
            destinationSystem = null;
        }

    }
    public SolarSystem GetSystemLocation()
    {
        return systemLocation;
    }

    public SolarSystem GetSystemTarget()
    {
        return systemTarget;
    }

    public SolarSystem GetSystemNextDesination()
    {
        return destinationSystem;
    }

    public void SetBlocking(bool block)
    {
        blocking = block;
    }

    public void SetInternalMovementOnly(bool internalMovement)
    {
        internalMovementOnly = internalMovement;
    }

    public bool MoveTo(SolarSystem system)
    {

        if(systemTarget != system)
        {
            systemTarget = system;
            return true;  
        }

        return false;
    }

    public bool IsMoving()
    {
        if(systemTarget != systemLocation || currentRoute)
        {
            return true;
        }
        return false;
    }



    private void CreatePath()
    {
        path.Clear();
        SystemPathNode currentNode = new SystemPathNode();
        List<SystemPathNode> nodes = new List<SystemPathNode>();
        List<SystemPathNode> closedNodes = new List<SystemPathNode>();
        currentNode.system = systemLocation;
        currentNode.parent = null;
        currentNode.travelDistanceFromStart = 0;

        nodes.Add(currentNode);

        while (currentNode.system != systemTarget)
        {
            foreach (TravelRoute route in currentNode.system.GetTravelRoutes())
            {
                SolarSystem neighbour = route.GetDestination(currentNode.system);
                if (closedNodes.Exists(c => c.system == neighbour) == false && !route.IsBlocked(currentEmpire))
                {
                    if(neighbour == systemTarget || !currentEmpire || !neighbour.GetEmpire() || currentEmpire == neighbour.GetEmpire())
                    {
                        SystemPathNode newNode = new SystemPathNode();
                        newNode.system = neighbour;
                        newNode.parent = currentNode;
                        newNode.travelDistanceFromStart = currentNode.travelDistanceFromStart + route.GetDistance();
                        SystemPathNode inList = nodes.Find(c => c.system == newNode.system);
                        if (inList == null)
                        {
                            nodes.Add(newNode);
                        }
                        else
                        {
                            if (newNode.travelDistanceFromStart < inList.travelDistanceFromStart)
                            {
                                nodes.Remove(inList);
                                nodes.Add(newNode);
                            }
                        }
                    }

                }
            }
            nodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            if (nodes.Count == 0)
            {
                break;
            }
            nodes.Sort((l, r) => l.travelDistanceFromStart.CompareTo(r.travelDistanceFromStart));
            currentNode = nodes[0];
            
        }

        if (currentNode.system != systemTarget)
        {
            return;
        }

        List<SolarSystem> finalPath = new List<SolarSystem>();

        while(currentNode.parent != null)
        {
            finalPath.Add(currentNode.system);
            currentNode = currentNode.parent;
        }

        finalPath.Reverse();
        foreach(SolarSystem system in finalPath)
        {
            path.Enqueue(system);
        }

        
    }



    private void FindNextMove()
    {
        if (systemLocation != systemTarget)
        {
            CreatePath();
        }
        else
        {
            path.Clear();
        }

        if (path.Count == 0)
        {
            systemTarget = systemLocation;
        }
        else
        {
            if (CheckNextSystemStillViable(path.Peek()))
            {

                if (blocking)
                {
                    currentRoute.UseRoute(currentEmpire);
                }
                destinationSystem = path.Dequeue();
                onLeaveSystem(systemLocation);
                systemLocation = null;
            }
            else
            {
                CreatePath();
            }
        }
    }

    private void CheckReachedTarget()
    {
        if (Vector3.Distance(transform.position, destinationSystem.transform.position) < stoppingDistance)
        {
            onReachedSystem(destinationSystem);
            SetLocation(destinationSystem);
        }
        else
        {
            if(internalMovementOnly && destinationSystem.GetEmpire() && destinationSystem.GetEmpire() != currentEmpire)
            {
                destinationSystem = currentRoute.GetDestination(destinationSystem);
                path.Clear();
                systemTarget = destinationSystem;
            }
            Vector3 direction = (destinationSystem.transform.position - transform.position).normalized;
            transform.position = transform.position + (direction * (movementSpeed * (Time.deltaTime * speedUI.GetSpeed())));
        }
    }

    private bool CheckNextSystemStillViable(SolarSystem system)
    {
        if(path.Count == 1 || !currentEmpire || !system.GetEmpire() || currentEmpire == system.GetEmpire())
        {
            currentRoute = systemLocation.GetTravelRoutes().Find(c => c.GetDestination(systemLocation) == path.Peek());
            if (currentRoute.IsBlocked(currentEmpire))
            {
                currentRoute = null;
                return false;
            }


            return true;
        }
        return false;
    }
}
