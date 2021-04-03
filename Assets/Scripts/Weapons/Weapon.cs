using Foxlair.Character.Targeting;
using Foxlair.Enemies;
using Foxlair.PlayerInput;
using System.Collections;
using UnityEngine;


namespace Foxlair.Weapons
{
    public class Weapon : MonoBehaviour
    {
        //protected WeaponRarity weaponRarity;
        public float _weaponDamage;
        public float _armorPenetration;
        public float _durability;
        public bool _isCoolingDown;

        public AudioSource _weaponAudioSource;

        public float _fireRate = 0.25f;
        public float _weaponRange = 50f;
        public Transform _weaponEnd;
        public float _nextFire;
        public WaitForSeconds _weaponShotDuration = new WaitForSeconds(0.07f);

        CharacterTargetingHandler _characterTargetingHandler;
        InputHandler _input;


        Enemy _weaponEnemyTarget;



        public virtual void Awake()
        {
            
        }

        public virtual void Start()
        {
            _characterTargetingHandler = CharacterTargetingHandler.Instance;
            _input = InputHandler.Instance;
            _weaponAudioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            _isCoolingDown = !(Time.time > _nextFire);

            Debug.DrawRay(_weaponEnd.position, _weaponEnd.forward, Color.yellow);

            if (_input.isFiringButtonDown && !_isCoolingDown)
            {
                Shoot();
            }
        }



        public virtual void Shoot()
        {
            _weaponEnemyTarget = _characterTargetingHandler.EnemyTarget;
            _nextFire = Time.time + _fireRate;
            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(ShotEffect());
            _weaponEnemyTarget.Damage(_weaponDamage);
        }

        public virtual IEnumerator ShotEffect()
        {
            // Play the shooting sound effect
            _weaponAudioSource.Play();
            //Wait for .07 seconds
            yield return _weaponShotDuration;
        }

    }
}