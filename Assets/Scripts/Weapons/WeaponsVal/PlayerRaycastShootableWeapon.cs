using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Weapons.WeaponsVal
{
    public class PlayerRaycastShootableWeapon : GenericShootableWeapon
    {
        private Camera mainCamera;
        public GameObject hitParticleSystemObj;
        public GameObject hitParticleSystemMonster;

        [SerializeField] private int HeadDamage = 40, ChessDamage = 20, LegDamage = 10;

        private void Start()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        protected override bool GetProjectileDestination(Vector3 startPoint, Vector3 weaponForward, out Vector3 destination)
        {
            RaycastHit hitData;

            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out hitData, 1000))
            {
                destination = hitData.point;
                Hit(hitData);
              
            }
            else
            {
                // worldPosition = transform.position + transform.forward * 1000f; //A CHANGER EN FONCTION DE LA CAMERA ?
                destination = ray.GetPoint(75);
            }

            return true;
        }

        protected override void Shoot()
        {
            
            Vector3 targetPosition;
            StartCoroutine(PlayMuzzle());
            GetProjectileDestination(bulletStartingPoint.position, Camera.main.transform.forward, out targetPosition);
        }
       

        protected virtual void Hit(RaycastHit hitData)
        {

            var col = hitData.collider;
            if (col)
            {
                var isBodyPart = false;
                var parent = col.gameObject;
                var bodyPart = col.gameObject;
                if (bodyPart.CompareTag("Head"))
                {
                    isBodyPart = true;
                       var body = parent.GetComponentInParent<Body>();
                    if (body != null)
                    {
                        body.TakeDamage(HeadDamage);
                    }
                }

                if (bodyPart.CompareTag("Chess"))
                {
                    isBodyPart = true;
                    var body = parent.GetComponentInParent<Body>();

                    if (body != null)
                    {
                        body.TakeDamage(ChessDamage);
                    }
                }

                if (bodyPart.CompareTag("Leg"))
                {
                    isBodyPart = true;
                    var body = parent.GetComponentInParent<Body>();

                    if (body != null)
                    {
                        body.TakeDamage(LegDamage);
                    }
                }
                GameObject hitEff;
                if (isBodyPart)
                {
                    hitEff = GameObject.Instantiate(hitParticleSystemMonster, hitData.point, Quaternion.LookRotation(hitData.normal));
                }
                else
                {
                    hitEff = GameObject.Instantiate(hitParticleSystemObj, hitData.point, Quaternion.LookRotation(hitData.normal));
                }
                Destroy(hitEff, 0.5f);
            }  
        }
    }
}