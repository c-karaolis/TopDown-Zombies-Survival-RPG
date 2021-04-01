using System.Collections;
using UnityEngine;
using Foxlair.PlayerInput;
using Foxlair.Enemies;

namespace Foxlair.Weapons
{
    public class RayTracingWeapon : MonoBehaviour
    {

        public int gunDamage = 1;                                            // Set the number of hitpoints that this gun will take away from shot objects with a health script
        public float fireRate = 0.25f;                                        // Number in seconds which controls how often the player can fire
        public float weaponRange = 50f;                                        // Distance in Unity units over which the player can fire
        public float hitForce = 100f;                                        // Amount of force which will be added to objects with a rigidbody shot by the player
        public Transform gunEnd;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun

        private Camera _camera;                                                // Holds a reference to the first person camera
        private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible
        private AudioSource gunAudio;                                        // Reference to the audio source which will play our shooting sound effect
        private LineRenderer laserLine;                                        // Reference to the LineRenderer component which will display our laserline
        private float nextFire;                                                // Float to store the time the player will be allowed to fire again, after firing
        private bool isCoolingDown;

        InputHandler _input;

        void Start()
        {
            _input = InputHandler.Instance;

            // Get and store a reference to our LineRenderer component
            laserLine = GetComponent<LineRenderer>();

            // Get and store a reference to our AudioSource component
            gunAudio = GetComponent<AudioSource>();

            // Get and store a reference to our Camera by searching this GameObject and its parents
            _camera = Camera.main;
        }


        void Update()
        {
            isCoolingDown = !(Time.time > nextFire);

            Debug.DrawRay(gunEnd.position, gunEnd.forward, Color.yellow);

            if ( _input.isFiringButtonDown && !isCoolingDown )
            {
                Shoot();
            }
        }


        private void Shoot()
        {
            nextFire = Time.time + fireRate;

            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = gunEnd.position;

            RaycastHit hit;

            // Set the start position for our visual effect for our laser to the position of gunEnd
            laserLine.SetPosition(0, gunEnd.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast(rayOrigin, gunEnd.TransformDirection(Vector3.forward), out hit, weaponRange))
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
                laserLine.SetPosition(1, rayOrigin + (_camera.transform.forward * weaponRange));
            }
        }

        private IEnumerator ShotEffect()
        {
            // Play the shooting sound effect
            gunAudio.Play();

            // Turn on our line renderer
            laserLine.enabled = true;

            //Wait for .07 seconds
            yield return shotDuration;

            // Deactivate our line renderer after waiting
            laserLine.enabled = false;
        }
    }
}