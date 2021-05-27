using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Tools.StateMachine
{
    public class ZombieStateMachine : EnemyStateMachine
    {
        void Start()
        {

        }

        public override void Update()
        {
            base.Update();
            HandleCommonStateTransitions();
        }

        void HandleCommonStateTransitions()
        {
            //RunningStateTransition();
            //AttackingStateTransition();
            //HarvestingStateTransition();
        }




    }
}