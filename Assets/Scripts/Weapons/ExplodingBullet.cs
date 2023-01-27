using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBullet : DefaultBullet
{
    [SerializeField] Explode explosion;
    [SerializeField] GameObject bullet;
    [SerializeField] float explosionDuration;

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            return;

        rb.constraints = RigidbodyConstraints.FreezeAll;
        explosion.gameObject.SetActive(true);
        bullet.SetActive(false);
        StartCoroutine(ExplosionCoroutine());
    }

    IEnumerator ExplosionCoroutine()
    {

        AudioManager.GetInstance().Play("ExplosionBullet", this.gameObject);
        yield return new WaitForSeconds(explosionDuration);
        explosion.GetComponent<Explode>().ExplosionEnded();
        while (explosion.GetComponent<ParticleSystem>().isPlaying)
            yield return null;
        Destroy(gameObject);
    }
}
