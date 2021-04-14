using Foxlair.Character;
using Foxlair.Character.Targeting;
using Foxlair.Enemies;
using Foxlair.PlayerInput;
using System;
using System.Collections;
using UnityEngine;


namespace Foxlair.Weapons
{
    public class Weapon : MonoBehaviour
    {
        //protected WeaponRarity weaponRarity;
        public float _weaponDamage;
        public float _armorPenetration;

        protected float _durability = 10f;
        protected float _durabilityLossPerShot = 1f;

        public bool _isCoolingDown;

        public AudioSource _weaponAudioSource;

        public float _fireRate = 0.25f;
        public float _weaponRange = 50f;
        public Transform _weaponEnd;
        public float _nextFire;
        public WaitForSeconds _weaponShotDuration = new WaitForSeconds(0.07f);



        public CharacterTargetingHandler _characterTargetingHandler;
        public InputHandler _input;


        public virtual void Awake()
        {

        }

        public virtual void Start()
        {
            _input = InputHandler.Instance;
            _characterTargetingHandler = CharacterTargetingHandler.Instance;
            _weaponAudioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            _isCoolingDown = !(Time.time > _nextFire);

            Debug.DrawRay(_weaponEnd.position, _weaponEnd.forward, Color.yellow);
        }

        public void DetermineAttack()
        {
            if ( !_isCoolingDown)
            {
                Attack();
            }
        }

        public bool InRangeToAttack()
        {
            if (Vector3.Distance(PlayerManager.Instance.PlayerTargetEnemy.transform.position, PlayerManager.Instance.MainPlayerCharacter.transform.position) <= _weaponRange)
            {
                Debug.Log("weapon in range to attack");
                return true;
            }
            else
            {
                return false;
            }
        }


        public virtual void Attack()
        {
            if (PlayerManager.Instance.PlayerTargetEnemy == null) { return; }

            Debug.Log("parent weapon");

            //TODO: this is fire delay not fire rate. 
            //find a way to normalise firerate for humans. e.g. thisfirerate = humanfirerate * (1/100)
            _nextFire = Time.time + _fireRate;
            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(AttackEffect());
            PlayerManager.Instance.PlayerTargetEnemy.Damage(_weaponDamage);

            _durability -= _durabilityLossPerShot;

        }

        public virtual IEnumerator AttackEffect()
        {
            // Play the shooting sound effect
            _weaponAudioSource.Play();
            //Wait for .07 seconds
            yield return _weaponShotDuration;
        }

        public virtual void HandleWeaponDurability()
        {
            if ((_durability -= _durabilityLossPerShot) <= 0)
            {
                Debug.Log("Durability Depleted");
                DestroyWeapon();
            }
        }

        private void DestroyWeapon()
        {
            Destroy(this, 0.3f);
        }
    }
}