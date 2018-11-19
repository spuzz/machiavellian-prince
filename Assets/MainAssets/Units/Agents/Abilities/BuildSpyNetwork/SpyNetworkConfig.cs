using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName =("Ability/SpyNetwork"))]
    public class SpyNetworkConfig : AbilityConfig
    {
        [Header("Spy Network")]
        [SerializeField] PlayerBuildingConfig spyNetworkConfig;

        public override AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo)
        {
            return gameObjectToAttachTo.AddComponent<SpyNetworkBehaviour>();
        }

        public PlayerBuildingConfig GetSpyNetworkConfig()
        {
            return spyNetworkConfig;
        }
    }

}
