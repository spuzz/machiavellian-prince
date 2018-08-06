using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DefaultBehaviour : PersonalityBehaviour
{

    public override void MakeDecisions(Empire empire, EmpireController empireController)
    {
        CheckState(empire, empireController);
        currentState.RunArmyBehaviour(empire, empireController);
        currentState.RunBuildBehaviour(empire, empireController);

    }

    private void CheckState(Empire empire, EmpireController empireController)
    {
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
                    UpdateState((config as DefaultConfig).GetAttack());
                }
            }
            else
            {
                if (currentState.GetStateName() != "Grow")
                {
                    UpdateState((config as DefaultConfig).GetGrow());
                }
            }

        }
    }
}


