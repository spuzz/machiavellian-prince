using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class BribeBehaviour : AbilityBehaviour
    {

        public override void Use(GameObject target = null, Agent agent = null)
        {
            if(target == null || agent == null)
            {
                throw new ArgumentException();
            }
            Leader leader;
            SolarSystem system = target.GetComponent<SolarSystem>();
            if(system && system.GetEmpire())
            {
                leader = system.GetEmpire().GetLeader();
            }
            else
            {
                throw new ArgumentException("No Leader Target Found");
            }
             
            Player player = agent.GetPlayer();
            if (leader && player.UseGold((int)config.GetCost()))
            {
                leader.IncreaseInfluence(agent.GetPlayer(), (config as BribeConfig).GetInfluence());
            }
            
            PlayParticleEffect();
            PlayAbilitySound();
            PlayAnimation();
        }
    }
}

