using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullets : MonoBehaviour
{
    public Vector3 startPosition { get; set; }
    public Vector3 targetPosition { get; set; }

    public float speed;

    [SerializeField] private int HeadDamage = 40, ChessDamage = 20, LegDamage = 10;

    protected Rigidbody rb;

   
    protected virtual void Start()
    {
        Physics.IgnoreLayerCollision(6, 7);
        Physics.IgnoreLayerCollision(11, 7);
        rb = GetComponent<Rigidbody>();
        StartCoroutine(AutoDestroyCo());
    }

    private IEnumerator AutoDestroyCo()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        var other = col;
        if (col.rigidbody)
        {
            var parent = col.rigidbody.gameObject;
            var bodyPart = col.collider.gameObject;
            var body = parent.GetComponentInChildren<Body>();

            if (body == null)
            {
                body = parent.GetComponent<Body>();
            }

            if (bodyPart.CompareTag("Player"))
            {
                if (body != null)
                {
                    body.TakeDamage(ChessDamage);
                }
            }

            if (bodyPart.CompareTag("Head"))
            {
                if (body != null)
                {
                    body.TakeDamage(HeadDamage);
                }
            }

            if (bodyPart.CompareTag("Chess"))
            {
                if (body != null)
                {
                    body.TakeDamage(ChessDamage);
                }
            }

            if (bodyPart.CompareTag("Leg"))
            {
                if (body != null)
                {
                    body.TakeDamage(LegDamage);
                }
            }
        }
  
        if (!col.gameObject.CompareTag("Player"))
            BulletDestruction();
    }

    protected virtual void BulletDestruction() {
        Destroy(gameObject);
    }
}
