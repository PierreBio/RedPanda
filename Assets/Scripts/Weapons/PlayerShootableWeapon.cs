using System.Collections;
using UnityEngine;


public class PlayerShootableWeapon : GenericShootableWeapon
{
    private Camera mainCamera;
    [SerializeField]
    protected GameObject bulletPrefab;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    protected override bool GetProjectileDestination(Vector3 startPoint, Vector3 weaponForward, out Vector3 destination)
    {
        RaycastHit hitData;
        destination = Vector3.zero;

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hitData, 1000))
        {
            destination = hitData.point;
            Vector3 dir = destination - startPoint;
            if (Vector3.Dot(dir.normalized, weaponForward) < 0)
                return false;
            if (Mathf.Abs(Vector3.Dot(dir.normalized, Vector3.up)) > 0.05f)
                destination = ray.GetPoint(10000);
        }
        else
        {
            // worldPosition = transform.position + transform.forward * 1000f; //A CHANGER EN FONCTION DE LA CAMERA ?
            destination = ray.GetPoint(1000);
        }

        return true;
    }
    protected override void Shoot()
    {
        Vector3 targetPosition;
        if (GetProjectileDestination(bulletStartingPoint.position, Camera.main.transform.forward, out targetPosition))
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletStartingPoint.position, Quaternion.identity);
            //Play sound
           
            bullet.GetComponent<Bullets>().startPosition = bulletStartingPoint.position;
            bullet.GetComponent<Bullets>().targetPosition = targetPosition;
        }
    }
}
