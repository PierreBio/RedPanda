using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalLauncherWeapon : PlayerShootableWeapon
{
    public Transform animalHolder;

    protected override void Shoot()
    {

        Vector3 targetPosition;
        if (GetProjectileDestination(bulletStartingPoint.position, Camera.main.transform.forward, out targetPosition))
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletStartingPoint.position, Quaternion.LookRotation(targetPosition));
            bullet.GetComponent<Bullets>().startPosition = bulletStartingPoint.position;
            bullet.GetComponent<Bullets>().targetPosition = targetPosition;

            Transform animal = animalHolder.GetChild(0);
            animal.localRotation = Quaternion.identity;
            animal.SetParent(bullet.transform);
            animal.GetComponentInChildren<Collider>().enabled = true;

            for (int i=0; i<animal.childCount;i++)
            { 
                GameObject animalTemp = animal.GetChild(i).gameObject;
                //
                animalTemp.GetComponentInChildren<Animator>().Play("Roll");
            }


        }

    }

    public override IEnumerator Fire()
    {
        if (!isAbleToAttack || animalHolder.childCount <= 0)
            yield break;

        StartCoroutine(RestCo());
        Shoot();
        
    }

}
