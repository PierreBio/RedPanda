using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{    
    private Body body;

    [SerializeField] private int HitDamageExplode = 30;

    bool active = true;

    void OnTriggerEnter (Collider other)
    {
        if (active && (other.CompareTag ("Enemy") || other.CompareTag("Player")))
            {
            body = other.GetComponent<Body> ();
            body.TakeDamageExplode (HitDamageExplode);                       
            }       
    }       

    public void ExplosionEnded()
    {
        active = false;
    }
}
