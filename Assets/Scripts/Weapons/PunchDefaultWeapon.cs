using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Weapons
{
    public class PunchDefaultWeapon : MeleeWeapon
    {

        public override void HandleWeaponDurability() { return; }

        public override void DestroyWeapon() { return; }
    }
}