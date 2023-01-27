using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    [SerializeField] protected GameObject hitBox;
    List<Body> hitEnemies = new List<Body>();

    [SerializeField] bool _isPlayerWeapon;
    public bool isPlayerWeapon { get { return _isPlayerWeapon; } }

    protected abstract int damage { get; }

    public override IEnumerator Fire()
    {
        if (!isAbleToAttack)
            yield break;
        StartCoroutine(RestCo());
    }

    public override void AttackStarted()
    {
        base.AttackStarted();
        hitBox.SetActive(true);
    }

    public override void AttackEnded()
    {
        base.AttackEnded();
        hitBox.SetActive(false);
        foreach (Body hitEnemy in hitEnemies)
        {
            hitEnemy.isVulnerableMelee = true;
        }
    }


    public void HitEnemy(Body other) {
        hitEnemies.Add(other);
        other.HitMelee(damage);
    }
}

