using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    public class SystemPathNode
    {
        public SolarSystem system;
        public SystemPathNode  parent;
        public float travelDistanceFromStart;
    }
    [SerializeField] float stoppingDistance = 1f;
    [SerializeField] SolarSystem systemLocation;
    [SerializeField] SolarSystem systemTarget;
    [SerializeField] float movementSpeed = 1f;
    Queue<SolarSystem> path = new Queue<SolarSystem>();

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
        

	}

    public void MoveTo(SolarSystem system)
    {
        systemTarget = system;
    }
    private void CreatePath()
    {
        SystemPathNode currentNode = new SystemPathNode();
        List<SystemPathNode> nodes = new List<SystemPathNode>();
        List<SystemPathNode> closedNodes = new List<SystemPathNode>();
        currentNode.system = systemLocation;
        currentNode.parent = null;
        currentNode.travelDistanceFromStart = 0;

        nodes.Add(currentNode);

        while (currentNode.system != systemTarget)
        {
            foreach (SolarSystem neighbour in currentNode.system.GetNearbySystems().Keys)
            {
                SystemPathNode newNode = new SystemPathNode();
                newNode.system = neighbour;
                newNode.parent = currentNode;
                newNode.travelDistanceFromStart = currentNode.travelDistanceFromStart + currentNode.system.GetNearbySystems()[neighbour];
                SystemPathNode inList = nodes.Find(c => c.system == newNode.system);
                if (inList == null || closedNodes.Exists(c => c.system == newNode.system) == false)
                {
                    nodes.Add(newNode);
                }
                else
                {
                    if(newNode.travelDistanceFromStart < inList.travelDistanceFromStart)
                    {
                        nodes.Remove(inList);
                        nodes.Add(newNode);
                    }
                }
            }
            nodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            nodes.Sort((l, r) => l.travelDistanceFromStart.CompareTo(r.travelDistanceFromStart));
            currentNode = nodes[0];
            
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

        if (systemTarget && systemLocation != systemTarget && path.Count == 0)
        {
            if(systemLocation)
            {
                onLeaveSystem(systemLocation);
            }
            
            CreatePath();
        }
        if (path.Count > 0 && Vector3.Distance(transform.position, path.Peek().transform.position) < stoppingDistance)
        {
            systemLocation = path.Peek();
            path.Dequeue();
            if (path.Count == 0)
            {
                onReachedSystem(systemLocation);
            }
        }
        if(path.Count > 0)
        {
            Vector3 direction = (path.Peek().transform.position - transform.position).normalized;
            transform.position = transform.position + (direction * (movementSpeed/10));
        }

    }
}
