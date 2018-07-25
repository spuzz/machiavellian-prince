using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class BribeBehaviour : AbilityBehaviour
    {

        public override void Use(GameObject target = null, Agent agent = null)
        {
            Leader leader = target.GetComponent<Leader>();
            if (leader != null && agent)
            {
                leader.IncreaseInfluence(agent.GetPlayer(), (config as BribeConfig).GetInfluence());
            }
            
            PlayParticleEffect();
            PlayAbilitySound();
            PlayAnimation();
        }
    }
}

