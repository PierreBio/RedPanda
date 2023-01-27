using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHitbox : MonoBehaviour
{
    [SerializeField] MeleeWeapon weapon;
    [SerializeField] int Damagemelee;


    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if ((weapon.isPlayerWeapon && other.CompareTag("Enemy"))||
            (!weapon.isPlayerWeapon && other.CompareTag("Player")))
        {
            Body playerBody = other.GetComponent<Body>();
            weapon.HitEnemy(playerBody);
            playerBody.HitMelee(Damagemelee);
        }
    }
}
