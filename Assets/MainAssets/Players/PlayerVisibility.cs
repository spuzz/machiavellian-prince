using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisibility : MonoBehaviour {

    Dictionary<GameObject, int> visibleObjects = new Dictionary<GameObject, int>();

    FogCamera fogCamera;

    private void Awake()
    {
        fogCamera = FindObjectOfType<FogCamera>();
    }
    public void AddEmpire(Empire empire)
    {
        empire.onVisibleObjectupdated += OnObjectVisibilityUpdate;
        foreach(SolarSystem solarSystem in empire.GetSystems())
        {
            AddObject(solarSystem.gameObject);
        }

        foreach (Army army in empire.GetArmies())
        {
            AddObject(army.gameObject);
        }

        foreach (ColonyShip colonyShip in empire.GetColonyShips())
        {
            AddObject(colonyShip.gameObject);
        }
    }

    public void RemoveEmpire(Empire empire)
    {
        empire.onVisibleObjectupdated -= OnObjectVisibilityUpdate;
        foreach (SolarSystem solarSystem in empire.GetSystems())
        {
            RemoveObject(solarSystem.gameObject);
        }

        foreach (Army army in empire.GetArmies())
        {
            RemoveObject(army.gameObject);
        }

        foreach (ColonyShip colonyShip in empire.GetColonyShips())
        {
            RemoveObject(colonyShip.gameObject);
        }
    }

    public void AddObject(GameObject visibleObject)
    {
        if(!visibleObjects.ContainsKey(visibleObject))
        {
            visibleObjects.Add(visibleObject, 1);
        }
        else
        {
            visibleObjects[visibleObject]++;
        }

        fogCamera.AddObject(visibleObject.transform);
    }

    public void RemoveObject(GameObject visibleObject)
    {
        if (visibleObjects.ContainsKey(visibleObject))
        {
            visibleObjects[visibleObject]--;
            if (visibleObjects[visibleObject] <= 0)
            {
                fogCamera.RemoveObject(visibleObject.transform);
                visibleObjects.Remove(visibleObject);
            }
        }

    }

    public void OnObjectVisibilityUpdate(GameObject gameObject, bool isVisible)
    {
        if(isVisible)
        {
            AddObject(gameObject);
        }
        else
        {
            RemoveObject(gameObject);
        }
    }
}
