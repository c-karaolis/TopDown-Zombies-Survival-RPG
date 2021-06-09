using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foxlair.Tools.Events;
using Foxlair.Enemies;

namespace Foxlair.Achievements
{
    public class BrainSurgeon : Achievement
    {
        protected override void SubscribeToEvents()
        {
            FoxlairEventManager.Instance.EnemyHealthSystem_OnDeath_Event += CheckIfKilledZombie;
        }

        protected override void UnsubscribeFromEvents()
        {
            FoxlairEventManager.Instance.EnemyHealthSystem_OnDeath_Event -= CheckIfKilledZombie;
        }
        private void CheckIfKilledZombie(EnemyCharacter enemyCharacter)
        {
            if (enemyCharacter is ZombieCharacter)
            {
                Achieve();
            }
        }
    }
}
