using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEventHandler : MonoBehaviour
{
    [SerializeField] WeaponBearer weaponBearer;

    public void AttackStarted()
    {
        weaponBearer.AttackStarted();
    }

    public void AttackEnded()
    {
        weaponBearer.AttackEnded();
    }

}
