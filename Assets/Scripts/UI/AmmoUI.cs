using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{

    [SerializeField] WeaponBearer weaponBearer;
    Weapon weapon;
    [SerializeField] TextMeshProUGUI currentAmmoText;
    [SerializeField] TextMeshProUGUI ammoLeftText;
    string infinityString = "\u221E";

    // Update is called once per frame
    void Update()
    {
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        //weapon = weaponBearer.GetCurrentWeapon();
        //if (weapon.isMelee)
        //{
        //    currentAmmoText.text = infinityString;
        //    ammoLeftText.text = infinityString;
        //}
        //else
        //{
        //    var shootableWeapon = weapon as ShootableWeapon;
        //    currentAmmoText.text = shootableWeapon.currentAmmo.ToString();
        //    ammoLeftText.text = shootableWeapon.GetAmmoLeft().ToString();
        //}

    }

    //private bool IsMeleeWeapon()
    //{
    //    return playerControllerWeapon.currentWeapon.maximumAmmo < 0; //Melee weapons have -1 maximumAmmo
    //}
}
