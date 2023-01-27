using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : DefaultBullet
{
    [SerializeField] float lifetime = 5f;

    override protected void BulletDestruction() {
        StartCoroutine(AutoDestroyCoCannon());
    }

    private IEnumerator AutoDestroyCoCannon()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
