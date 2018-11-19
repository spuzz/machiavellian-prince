using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class CorporationBehaviour : AbilityBehaviour
    {

        public override void Use(GameObject target = null, Agent agent = null)
        {
            if(target == null || agent == null)
            {
                throw new ArgumentException();
            }
            Leader leader;
            SolarSystem system = target.GetComponent<SolarSystem>();
            if(system)
            {
                //system.GetComponent<PlayerBuildingController>().BuildPlayerBuilding((config as CorporationConfig).GetCorporationConfig(),agent.GetPlayer());

            }
             
            
            PlayParticleEffect(agent.gameObject);
            PlayAbilitySound(agent.gameObject);
            PlayAnimation(agent.gameObject);
        }
    }
}

