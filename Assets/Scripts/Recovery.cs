using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recovery : MonoBehaviour
{
    private Body body;

    [SerializeField] private int RecoveryHP = 40;

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            body = other.GetComponent<Body> ();
            body.RecoveryHealth (RecoveryHP);
            Destroy (gameObject);
        }
    }
}
