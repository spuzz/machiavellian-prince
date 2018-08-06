using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName =("Ability/Bribe"))]
    public class DefaultConfig : AbilityConfig
    {
        [Header("Bribe Specific")]
        [SerializeField]
        float influence = 100f;

        public override AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo)
        {
            return gameObjectToAttachTo.AddComponent<DefaultBehaviour>();
        }

        public float GetInfluence()
        {
            return influence;
        }
    }

}
