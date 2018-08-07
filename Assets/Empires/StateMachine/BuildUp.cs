using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = ("State/BuildUp"))]
public class BuildUp : State
{

    public BuildUp()
    {
        
        buildFunctions.Add(CheckArmies);
        buildFunctions.Add(CheckMinDefence);
        buildFunctions.Add(CheckMinOffence);
        defaultBuildFunctions.Add(BuildInfrastructure);
        defaultBuildFunctions.Add(TrainArmy);
    }
    public override void RunArmyBehaviour(Empire empire, EmpireController empireController)
    {
        List<Army> armies = empire.GetArmies();
        List<SolarSystem> enemyBorderSystems = empireController.GetEnemyBorderSystems();
        enemyBorderSystems.OrderByDescending(c => c.GetOffence());
        DefendBorders(empire, armies, enemyBorderSystems);
        ColonisePlanets(empire, empireController);
        return;
    }


    public override void RunBuildBehaviour(Empire empire, EmpireController empireController)
    {
        bool status = true;
        while (status == true)
        {
            status = Build(empire);
        }
    }
}
