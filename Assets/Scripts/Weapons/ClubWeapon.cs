using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubWeapon : MeleeWeapon
{
    [SerializeField] private int _damage = 30;
    protected override int damage { get { return _damage; } }
}
