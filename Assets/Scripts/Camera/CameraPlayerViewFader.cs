using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Foxlair.CustomCameras
{
    public class CameraPlayerViewFader : MonoBehaviour
    {

        public IsometricCamera mainCamera;
        Transform player;
        private void Awake()
        {
            mainCamera = GetComponent<IsometricCamera>();
            player = mainCamera.targetTransform;
        }
        void Update()
        {
            Vector3 pos = transform.position;
            Vector3 dir = (player.position - transform.position).normalized;
            Debug.DrawLine(pos, pos + dir * 10, Color.red, Mathf.Infinity);

            float dist = Vector3.Distance(transform.position, player.position);
            RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, dist);



            foreach (RaycastHit hit in hits)
            {
                Debug.Log(hit.transform.gameObject.name);
                
                
                    Color color = hit.transform.gameObject.GetComponentInChildren<Renderer>().material.color;
                    color.a = 0.5f;
                    hit.transform.gameObject.GetComponentInChildren<Renderer>().material.color = color;
                

                //Change the opacity of the of each object to semitransparent.
            }
        }
    }
}