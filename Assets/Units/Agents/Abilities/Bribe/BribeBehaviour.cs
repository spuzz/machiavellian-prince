using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class DefaultBehaviour : AbilityBehaviour
    {

        public override void Use(GameObject target = null, Agent agent = null)
        {
            Leader leader = target.GetComponent<Leader>();
            if (leader != null && agent)
            {
                leader.IncreaseInfluence(agent.GetPlayer(), (config as DefaultConfig).GetInfluence());
            }
            
            PlayParticleEffect();
            PlayAbilitySound();
            PlayAnimation();
        }
    }
}

