using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SystemPathNode
{
    public SolarSystem system;
    public SystemPathNode parent;
    public float travelDistanceFromStart;
}

public class Navigation {

    public static SolarSystem GetNearestSystem(List<Empire> empireToFind, SolarSystem startSystem)
    {
        if(empireToFind.Count == 0)
        {
            return null;
        }
        SystemPathNode currentNode = new SystemPathNode();
        List<SystemPathNode> nodes = new List<SystemPathNode>();
        List<SystemPathNode> closedNodes = new List<SystemPathNode>();
        //if (startSystem == null)
        //{
        //    if (systemLocation == null)
        //    {
        //        currentNode.system = destinationSystem;
        //    }
        //    else
        //    {
        //        currentNode.system = systemLocation;
        //    }

        //}
        //else
        //{
        //    currentNode.system = startSystem;
        //}
        currentNode.system = startSystem;
        currentNode.parent = null;
        currentNode.travelDistanceFromStart = 0;

        nodes.Add(currentNode);

        while (!empireToFind.Contains(currentNode.system.GetEmpire()))
        {
            foreach (TravelRoute route in currentNode.system.GetTravelRoutes())
            {
                SystemPathNode newNode = new SystemPathNode();
                newNode.system = route.GetDestination(currentNode.system);
                newNode.parent = currentNode;
                newNode.travelDistanceFromStart = currentNode.travelDistanceFromStart + route.GetDistance();
                SystemPathNode inList = nodes.Find(c => c.system == newNode.system);
                if (inList == null || closedNodes.Exists(c => c.system == newNode.system) == false)
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
            nodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            nodes.Sort((l, r) => l.travelDistanceFromStart.CompareTo(r.travelDistanceFromStart));
            currentNode = nodes[0];

        }
        return currentNode.system;
    }

    public static bool RouteAvailable(SolarSystem systemLocation, SolarSystem targetSystem,Empire empire)
    {
        foreach(TravelRoute route in systemLocation.GetTravelRoutes())
        {
            if (route.ContainsSystem(targetSystem))
            {
                return route.IsBlocked(empire);
            }

        }
        return false;
    }
}
