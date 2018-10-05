using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyShip : MonoBehaviour {
    Empire empire;
    MovementController movementController;

    void Awake()
    {
        movementController = GetComponent<MovementController>();
        movementController.onReachedSystem += OnReachedSystem;
        movementController.onLeaveSystem += OnLeaveSystem;
    }

    public void SetEmpire(Empire empire)
    {
        this.empire = empire;
    }
    public Empire GetEmpire()
    {
        return empire;
    }
    public void DestroyColonyShip()
    {
        empire.RemoveColonyShip(this);
        Destroy(gameObject);
    }

    public void MoveTo(SolarSystem system)
    {
        movementController.MoveTo(system);
    }

    private void OnReachedSystem(SolarSystem system)
    {
        ColoniseSystem(system);
    }

    private void ColoniseSystem(SolarSystem system)
    {
        if(!system.GetEmpire())
        {
            system.Colonised(empire);
            DestroyColonyShip();
        }
        
    }

    private void OnLeaveSystem(SolarSystem system)
    {
        // Nothing to do
    }


}
