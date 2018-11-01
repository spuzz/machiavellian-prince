using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DefaultBehaviour : PersonalityBehaviour
{

    public override void MakeDecisions(Empire empire, EmpireController empireController, ref State currentState)
    {
        CheckState(empire, empireController, ref currentState);
        currentState.RunArmyBehaviour(empire, empireController);
        currentState.RunBuildBehaviour(empire, empireController);

    }

    private void CheckState(Empire empire, EmpireController empireController, ref State currentState)
    {
        if(!currentState)
        {
            currentState = (config as DefaultConfig).GetGrow();
        }
        if (empire.IsAlive())
        {

            // TODO Simple code to set off war
            DiplomacyController diplomacy = empireController.GetDiplomacyController();
            if (empireController.GetEnemyBorderSystems().Count > 0)
            {
                foreach (SolarSystem system in empireController.GetEnemyBorderSystems())
                {
                    if (diplomacy.GetDiplomacy(system.GetEmpire()).relationship != Relationship.War)
                    {
                        diplomacy.DeclareWar(empire, system.GetEmpire());
                    }
                }
                if (currentState.GetStateName() != "Attack")
                {
                    currentState = (config as DefaultConfig).GetAttack();
                }
            }
            else
            {
                CheckGrowOrBuildUp(empire, empireController, ref currentState);
            }

        }
    }

    private void CheckGrowOrBuildUp(Empire empire, EmpireController empireController, ref State currentState)
    {
        if(currentState == (config as DefaultConfig).GetGrow())
        {
            if(empire.HasOrBuildingColonyShip())
            {
                currentState = (config as DefaultConfig).GetBuildUp();
            }
        }
        else if(currentState == (config as DefaultConfig).GetBuildUp())
        {
            if(empireController.GetNeutralBorderSystems().Count > 0  && empire.CheckSystemHealth() && currentState.GetIsInDefaultBuildMode())
            {
                currentState = (config as DefaultConfig).GetGrow();
            }
        }
    }
}


