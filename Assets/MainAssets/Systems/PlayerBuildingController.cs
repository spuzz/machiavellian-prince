using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingController : MonoBehaviour {

    [SerializeField] int maxPlayerBuildings = 5;
    Dictionary<Player, List<PlayerBuilding>> playerBuildings = new Dictionary<Player, List<PlayerBuilding>>();
    Dictionary<Player,SpyNetwork> playerSpyNetworks = new Dictionary<Player, SpyNetwork>();
    [SerializeField] GameObject playerBuildingGameObject;
    [SerializeField] GameObject spyNetworkBuilding;
    int daysLeftOnBuild = 0;
    PlayerBuildingConfig currentConfig;


    public IEnumerable<PlayerBuilding> GetPlayerBuildings(Player player)
    {
        if (playerBuildings.ContainsKey(player))
        {
            return playerBuildings[player];
        }
        else
        {
            return new List<PlayerBuilding>();
        }
        
    }

    public SpyNetwork GetPlayerSpyNetwork(Player player)
    {
        if(playerSpyNetworks.ContainsKey(player))
        {
            return playerSpyNetworks[player];
        }
        return null;
    }

    public bool BuildSpyNetwork(Player player)
    {
        if(playerSpyNetworks.ContainsKey(player))
        {
            return false;
        }
        SpyNetwork spyNetwork = Instantiate(spyNetworkBuilding, transform.Find("PlayerBuildings")).GetComponent<SpyNetwork>();
        playerSpyNetworks.Add(player, spyNetwork);
        playerBuildings.Add(player, new List<PlayerBuilding>());
        player.AddSystemWithBuildings(GetComponent<SolarSystem>());
        return true;
    }

    public bool BuildPlayerBuilding(PlayerBuildingConfig playerBuildingConfig, Player player, int buildingNumber)
    {

        if(!playerSpyNetworks.ContainsKey(player))
        {
            return false;
        }

        if (playerBuildings[player].Count >= maxPlayerBuildings)
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

}
