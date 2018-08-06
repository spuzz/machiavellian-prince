using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("State/Grow"))]
public class Grow : State
{

    public override void RunArmyBehaviour(Empire empire, EmpireController empireController)
    {
        return;
    }

    public override void RunBuildBehaviour(Empire empire, EmpireController empireController)
    {
        bool status = true;
        while(status == true)
        {
            status = Build(empire);
        }
    }

    private bool Build(Empire empire)
    {
        if ((float)empire.GetPredictedNetIncome() / (float)empire.GetPredictedGrossIncome() > GetEconomyAvailable())
        {
            return false;
        }

        if(CheckArmies(empire))
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

        if (BuildInfrastructure(empire))
        {
            return true;
        }

        if (BuildColonyShip(empire))
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
