using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {


    [SerializeField] float stoppingDistance = 1f;
    [SerializeField] SolarSystem systemLocation;
    [SerializeField] SolarSystem destinationSystem;
    [SerializeField] SolarSystem systemTarget;

    public void Remove()
    {
        if(army)
        {
            if (systemLocation)
            {
                systemLocation.RemoveArmy(army);
            }
            if(currentRoute)
            {
                currentRoute.finishUsingRoute(army);
            }
        }

    }

    [SerializeField] TravelRoute currentRoute;
    [SerializeField] float movementSpeed = 1f;
    Queue<SolarSystem> path = new Queue<SolarSystem>();
    Empire currentEmpire;
    Army army;
    public void SetLocation(SolarSystem systemLocation)
    {
        this.systemLocation = systemLocation;
        transform.position = systemLocation.transform.position;
    }
    public SolarSystem GetSystemLocation()
    {
        return systemLocation;
    }

    public SolarSystem GetSystemTarget()
    {
        return systemTarget;
    }
    // New Delegates
    public delegate void OnReachedSystem(SolarSystem system); // declare new delegate type
    public event OnReachedSystem onReachedSystem; // instantiate an observer set

    public delegate void OnLeaveSystem(SolarSystem system); // declare new delegate type
    public event OnLeaveSystem onLeaveSystem; // instantiate an observer set

    void Start () {

        if(systemLocation)
        {
            transform.position = systemLocation.transform.position;
        }
        currentEmpire = gameObject.GetComponentInParent<Empire>();
        army = GetComponent<Army>();
    }

    public void MoveTo(SolarSystem system)
    {
        if(systemTarget != system)
        {
            systemTarget = system;
            
        }
        
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

    // Update is called once per frame
    void Update () {

        if (currentRoute)
        {
            CheckReachedTarget();
        }
        else
        {
            FindNextMove();
        }

    }

    private void FindNextMove()
    {
        if (systemLocation != systemTarget)
        {
            CreatePath();
        }
        if (path.Count == 0)
        {
            systemTarget = systemLocation;
        }
        else
        {
            if (CheckNextSystemStillViable(path.Peek()))
            {

                if (army)
                {
                    currentRoute.UseRoute(army);
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
            systemLocation = destinationSystem;
            onReachedSystem(destinationSystem);
            if (army)
            {
                currentRoute.finishUsingRoute(army);
            }
            currentRoute = null;
            destinationSystem = null;
        }
        else
        {
            Vector3 direction = (destinationSystem.transform.position - transform.position).normalized;
            transform.position = transform.position + (direction * (movementSpeed / 10));
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
