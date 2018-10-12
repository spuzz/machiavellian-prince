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
        defaultBuildFunctions.Add(BuildInfrastructure);
        defaultBuildFunctions.Add(TrainArmy);
    }
    public override void RunArmyBehaviour(Empire empire, EmpireController empireController)
    {
        DefendBorders(empire,empireController);
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
