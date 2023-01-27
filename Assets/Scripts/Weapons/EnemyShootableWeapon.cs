using System.Collections;
using UnityEngine;

class EnemyShootableWeapon : GenericShootableWeapon
{
    [SerializeField]
    protected GameObject bulletPrefab;

    [SerializeField]
    protected float heightAdd = 1;

    [SerializeField]
    protected float waitTimebeforeShoot = 0.3f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().baseCharacter.transform;
    }

    protected override bool GetProjectileDestination(Vector3 startPoint, Vector3 weaponForward, out Vector3 destination)
    { 
        destination = player.position;

        // WARNING : specifique au canon
        destination += Vector3.up * heightAdd * Vector3.Distance(startPoint, destination);

        return true;
    }

    protected override void Shoot()
    {
        StartCoroutine(ShootAfterTime(waitTimebeforeShoot));
    }

    private IEnumerator ShootAfterTime(float timeBeforeShoot)
    {

        yield return new WaitForSeconds(timeBeforeShoot);
        //Play sound
        
        AudioManager.GetInstance().Play("CanonShoot", this.gameObject);
        Vector3 targetPosition;
        if (GetProjectileDestination(bulletStartingPoint.position, Camera.main.transform.forward, out targetPosition))
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletStartingPoint.position, Quaternion.identity);
            bullet.GetComponent<Bullets>().startPosition = bulletStartingPoint.position;
            bullet.GetComponent<Bullets>().targetPosition = targetPosition;
        }
    }
}

