using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawWeapon : MeleeWeapon
{
    [SerializeField] private int _damage = 30;
    protected override int damage { get { return _damage; } }

    [SerializeField] Animator pawAnim;

    [SerializeField] GameObject clawUI;

    [SerializeField] float animDuration = 0.5f;

    private void Start()
    {
        
        clawUI.GetComponent<ClawUI>().SetDuration(animDuration);
    }

    public override IEnumerator Fire()
    {
        if (!isAbleToAttack)
            yield break;
        StartCoroutine(base.Fire());
        clawUI.SetActive(true);
        pawAnim.Play("claw");
        yield return null;
    }

    public override void AttackStarted()
    {
        base.AttackStarted();
        
        AudioManager.GetInstance().Play("Claw",this.gameObject);
    }
}
