using Foxlair.Character.Targeting;
using Foxlair.Enemies;
using Foxlair.Harvesting;
using Foxlair.Tools;
using UnityEngine;

namespace Foxlair.Character
{

    public class PlayerManager : PersistentSingletonMonoBehaviour<PlayerManager>
    {
        public GameObject punchWeapon;


        [System.NonSerialized]
        public Enemy PlayerTargetEnemy;
        [System.NonSerialized]
        public ResourceNode PlayerTargetResourceNode;
    }
}