using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{    
    private Body body;

    [SerializeField] private int HitDamageMelee = 30;

    [SerializeField] private bool Player;

    void OnTriggerEnter (Collider other)
    {
        if (Player)
        {
            if (other.CompareTag ("Enemy"))
            {
            body = other.GetComponent<Body> ();
            body.HitMelee (HitDamageMelee);                       
            }       
        } 
        else 
        {
            if (other.CompareTag ("Player"))
            {
            body = other.GetComponent<Body> ();
            body.HitMelee (HitDamageMelee);                       
            }  
        }
    }
}
