using System.Collections;
using UnityEngine;
using Foxlair.PlayerInput;
using Foxlair.Enemies;

namespace Foxlair.Weapons
{
    public class RayTracingWeapon : Weapon
    {

        public int gunDamage = 1;                                            // Set the number of hitpoints that this gun will take away from shot objects with a health script
                                     // Distance in Unity units over which the player can fire
        public float hitForce = 100f;                                        // Amount of force which will be added to objects with a rigidbody shot by the player

        private Camera _camera;                                                // Holds a reference to the first person camera
        private LineRenderer laserLine;                                        // Reference to the LineRenderer component which will display our laserline


        

        public override void Start()
        {
            _input = InputHandler.Instance;

            // Get and store a reference to our LineRenderer component
            laserLine = GetComponent<LineRenderer>();

            // Get and store a reference to our AudioSource component
            _weaponAudioSource = GetComponent<AudioSource>();

            // Get and store a reference to our Camera by searching this GameObject and its parents
            _camera = Camera.main;
        }


        public override void Update()
        {
            base.Update();
        }


        public override void Attack()
        {
            _nextFire = Time.time + _fireRate;

            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(AttackEffect());

            Vector3 rayOrigin = _weaponEnd.position;

            RaycastHit hit;

            // Set the start position for our visual effect for our laser to the position of gunEnd
            laserLine.SetPosition(0, _weaponEnd.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast(rayOrigin, _weaponEnd.TransformDirection(Vector3.forward), out hit, _weaponRange))
            {
                // Set the end position for our laser line 
                laserLine.SetPosition(1, hit.point);

                // Get a reference to a health script attached to the collider we hit
                Enemy enemy = hit.collider.GetComponent<Enemy>();

                // If there was a health script attached
                if (enemy != null)
                {
                    // Call the damage function of that script, passing in our gunDamage variable
                    enemy.Damage(gunDamage);
                }
                else
                {
                    Debug.Log("Enemy was null");
                }
                // Check if the object we hit has a rigidbody attached
                if (hit.rigidbody != null)
                {
                    // Add force to the rigidbody we hit, in the direction from which it was hit
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                laserLine.SetPosition(1, rayOrigin + (_camera.transform.forward * _weaponRange));
            }
        }

     
    }
}