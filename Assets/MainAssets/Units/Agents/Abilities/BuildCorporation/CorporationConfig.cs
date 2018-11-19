using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName =("Ability/Corporation"))]
    public class CorporationConfig : AbilityConfig
    {
        [Header("Corporation")]
        [SerializeField] PlayerBuildingConfig corporationConfig;

        public override AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo)
        {
            return gameObjectToAttachTo.AddComponent<CorporationBehaviour>();
        }

        public PlayerBuildingConfig GetCorporationConfig()
        {
            return corporationConfig;
        }
    }

}
