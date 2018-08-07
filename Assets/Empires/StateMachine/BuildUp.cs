using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = ("State/BuildUp"))]
public class BuildUp : State
{

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

    private bool Build(Empire empire)
    {
        isInDefaultBuild = false;
        if ((float)empire.GetPredictedNetIncome() / (float)empire.GetPredictedGrossIncome() > GetEconomyAvailable())
        {
            return false;
        }

        if (CheckArmies(empire))
        {
            return true;
        }

        if (CheckMinDefence(empire))
        {
            return true;
        }

        if (CheckMinOffence(empire))
        {
            return true;
        }

        isInDefaultBuild = true;

        if (BuildInfrastructure(empire))
        {
            return true;
        }

        if (TrainArmy(empire))
        {
            return true;
        }

        return false;
    }
}
