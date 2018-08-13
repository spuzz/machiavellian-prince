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
        foreach(Army army in empire.GetArmies().FindAll(c => c.GetArmyType() == Army.ArmyType.Offensive))
        {
            army.AttackNearestEnemy();
        }
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
