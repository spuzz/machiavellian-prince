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
        DefendBorders(empire, empireController);

        // Keep going until no army has an immediate target
        while (AttackEnemy(empire, empireController) == true)
        {

        }
       
        foreach (Army army in empire.GetArmies().FindAll(c => c.GetArmyType() == Army.ArmyType.Offensive 
                && c.GetArmyStatus() != Army.ArmyStatus.Training
                && c.GetArmyStatus() != Army.ArmyStatus.Attacking))
        {
            army.MoveToNearestEnemy();
        }

        
        return;
    }

    private bool AttackEnemy(Empire empire, EmpireController empireController)
    {
        DiplomacyController diplomacy = empireController.GetDiplomacyController();
        foreach (Army army in empire.GetArmies().FindAll(c => c.GetArmyType() == Army.ArmyType.Offensive
                && c.GetArmyStatus() == Army.ArmyStatus.Idle || c.GetArmyStatus() == Army.ArmyStatus.Training))
        {
            SolarSystem system = army.GetComponent<MovementController>().GetSystemLocation();
            List<SolarSystem> systems = system.GetNearbySystems();
            foreach(SolarSystem nearbySystem in systems)
            {
                 
                if (nearbySystem.GetEmpire() && nearbySystem.GetEmpire() != empire && diplomacy.GetDiplomacy(nearbySystem.GetEmpire()).relationship == Relationship.War && Navigation.RouteAvailable(system, nearbySystem,empire))
                {

                    List<Army> combinedOffence = new List<Army>();
                    float offence = 0;
                    foreach (Army systemArmy in system.GetArmies().FindAll(c => c.GetArmyType() == Army.ArmyType.Offensive
                        && c.GetArmyStatus() == Army.ArmyStatus.Idle || c.GetArmyStatus() == Army.ArmyStatus.Training))
                    {
                        combinedOffence.Add(systemArmy);
                        offence += systemArmy.GetAttackValue();
                    }

                    if(offence > nearbySystem.GetDefence())
                    {
                        CreateFleet(combinedOffence, nearbySystem, system, empire);
                        return true;
                    }
                    
                   
                }
            }
            
        }
        return false;
    }

    private void CreateFleet(List<Army> combinedOffence, SolarSystem enemySystem, SolarSystem currentSystem, Empire empire)
    {
        Fleet fleet = Instantiate(empire.GetFleetPrefab(), empire.transform.Find("Armies").transform).GetComponent<Fleet>();
        fleet.SetEmpire(empire);
        fleet.SetLocation(currentSystem);
        foreach (Army army in combinedOffence)
        {
            army.GetComponent<ArmyTrainer>().CancelAllTraining();
            army.SetArmyStatus(Army.ArmyStatus.Attacking);
            
            fleet.AddArmy(army);
        }

        fleet.SetTarget(enemySystem);



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
