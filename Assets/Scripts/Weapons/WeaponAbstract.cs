using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool isMelee;

    public bool isAbleToAttack = true;

    public float delayBetweenAttacks;

    public string animBoolean;

    [SerializeField] GameObject[] weaponDisplay; // Everything that needs to appear/disappear with the weapon
    public abstract IEnumerator Fire();
    public IEnumerator RestCo()
    {
        isAbleToAttack = false;
        yield return new WaitForSeconds(delayBetweenAttacks);
        isAbleToAttack = true;
    }

    // Used in animation to activate/disable hitboxes (especially for melee weapons)
    public virtual void AttackStarted() { }
    public virtual void AttackEnded() { }

    private void OnEnable()
    {
        foreach (GameObject obj in weaponDisplay)
        {
            obj.SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject obj in weaponDisplay)
        {
            obj.SetActive(false);
        }
    }

}
