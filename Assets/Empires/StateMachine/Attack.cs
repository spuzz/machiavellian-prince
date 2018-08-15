using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("State/Attack"))]
public class Attack : State
{
    public Attack()
    {
        buildFunctions = new List<BuildFunction>();
        buildFunctions.Add(CheckArmies);
        buildFunctions.Add(CheckMinDefence);
        buildFunctions.Add(CheckMinOffence);
        defaultBuildFunctions.Add(TrainArmy);
    }
    public override void RunArmyBehaviour(Empire empire, EmpireController empireController)
    {
        AttackEnemy();
        foreach (Army army in empire.GetArmies().FindAll(c => c.GetArmyType() == Army.ArmyType.Offensive 
                && c.GetArmyStatus() != Army.ArmyStatus.Training
                && c.GetArmyStatus() != Army.ArmyStatus.Attacking))
        {
            army.MoveToNearestEnemy();
        }

        
        return;
    }

    private void AttackEnemy()
    {
    //    foreach (Army army in empire.GetArmies().FindAll(c => c.GetArmyType() == Army.ArmyType.Offensive
    //            && c.GetArmyStatus() != Army.ArmyStatus.Training
    //            && c.GetArmyStatus() != Army.ArmyStatus.Attacking))
    //    {
    //        army.MoveToNearestEnemy();
    //    }
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
