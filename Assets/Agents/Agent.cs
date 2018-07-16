using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{

    [SerializeField] Player player;
    [SerializeField] List<AbilityConfig> abilities;

    public Player GetPlayer()
    {
        return player;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }
}

