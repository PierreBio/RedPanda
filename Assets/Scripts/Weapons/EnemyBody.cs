using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Claw };

public class EnemyBody : MonoBehaviour
{
    public int health = 100;

    public bool isVulnerableMelee = true;

    public void Hit(int damage)
    {
        if (isVulnerableMelee)
        {
            health -= damage;
            isVulnerableMelee = false;
        }
    }
}
