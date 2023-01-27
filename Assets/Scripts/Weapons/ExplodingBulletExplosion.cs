using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBulletExplosion : MonoBehaviour
{

    [SerializeField] ParticleSystem explosionParticles;

    private void OnEnable()
    {


        explosionParticles.Play();

    }
}
