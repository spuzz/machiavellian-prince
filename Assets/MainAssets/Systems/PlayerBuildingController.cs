using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingController : MonoBehaviour {

    [SerializeField] int maxPlayerBuildings = 5;
    Dictionary<Player, List<PlayerBuilding>> playerBuildings = new Dictionary<Player, List<PlayerBuilding>>();
    [SerializeField] GameObject playerBuildingGameObject;
    bool isBuilding = false;
    int daysLeftOnBuild = 0;
    PlayerBuildingConfig currentConfig;

    public bool IsBuilding()
    {
        return isBuilding;
    }

    public IEnumerable<PlayerBuilding> GetPlayerBuildings(Player player)
    {
        if(!playerBuildings.ContainsKey(player))
        {
            playerBuildings.Add(player,new List<PlayerBuilding>());
        }
        return playerBuildings[player];
    }

    public bool BuildPlayerBuilding(PlayerBuildingConfig playerBuildingConfig, Player player, int buildingNumber)
    {
        if(isBuilding == true)
        {
            return false;
        }

        if(!playerBuildings.ContainsKey(player))
        {
            playerBuildings.Add(player, new List<PlayerBuilding>());
        }

        if(playerBuildings[player].Count >= maxPlayerBuildings)
        {
            return false;
        }


        PlayerBuilding playerBuilding = Instantiate(playerBuildingGameObject, transform.Find("PlayerBuildings")).GetComponent<PlayerBuilding>();
        playerBuilding.SetConfig(playerBuildingConfig);
        playerBuilding.SetSystem(GetComponent<SolarSystem>());
        playerBuilding.Build(buildingNumber);
        playerBuildings[player].Add(playerBuilding);

        return true;

    }

    public void OnDayChange(int days)
    {
        if (IsBuilding())
        {
            daysLeftOnBuild -= days;
            if (daysLeftOnBuild <= 0)
            {
                isBuilding = false;
               
            }
        }

    }
}
