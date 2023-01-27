using System.Collections;
using UnityEngine;


public abstract class GenericShootableWeapon : Weapon
{

    [SerializeField]
    protected Transform bulletStartingPoint;

    public GameObject MuzzleFlash;
    private void Start()
    {
        isAbleToAttack = true;
    }

    void StartShootingProcess()
    {
        Shoot();
       
    }

    public override IEnumerator Fire()
    {
        if (!isAbleToAttack)
            yield break;

        StartCoroutine(RestCo());
        StartShootingProcess();
    }

    protected IEnumerator PlayMuzzle()
    {
        if (MuzzleFlash == null)
            yield break;

        MuzzleFlash.SetActive(true);
        yield return new WaitForSeconds(delayBetweenAttacks);
        MuzzleFlash.SetActive(false);
    }

    protected abstract bool GetProjectileDestination(Vector3 startPoint, Vector3 weaponForward, out Vector3 destination);
    protected abstract void Shoot();
}
