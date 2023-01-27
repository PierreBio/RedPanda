using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropelledBullet : ExplodingBullet
{
    [SerializeField] float thrust = 100;
    [SerializeField] float initialThrust = 1;
    float maxDistance = 200;
    [SerializeField] AnimationCurve thrustCurve;
    float timeAlive = 0;

    Rigidbody weaponBearerRb;

    protected override void CustomStart()
    {
        weaponBearerRb = FindObjectOfType<BaseCharacter>().gameObject.GetComponent<Rigidbody>();
        AudioManager.GetInstance().Play("Sarbacane", this.gameObject);
        Vector3 dir = (targetPosition - transform.position).normalized;
        //targetPosition = transform.position + dir * maxDistance;// + Vector3.up * maxDistance/10 ;
        rb.AddForce(Vector3.up * initialThrust, ForceMode.Impulse);
        rb.AddForce(dir * initialThrust, ForceMode.Impulse);
        transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
    }

    private void FixedUpdate()
    {
        Vector3 dir = (targetPosition - transform.position).normalized;
        rb.AddForce(thrust * dir * thrustCurve.Evaluate(timeAlive));

        // Compensating gravity
        rb.AddForce(-rb.mass * Physics.gravity * Mathf.Clamp01(thrustCurve.Evaluate(timeAlive)));
        
        
        if (rb.velocity.magnitude > speed)
            rb.velocity = rb.velocity.normalized * speed;
        timeAlive += Time.fixedDeltaTime;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 5);
        Gizmos.DrawLine(transform.position, targetPosition);

    }

    public float GetThrustCurve() {
        return thrustCurve.Evaluate(timeAlive);
    }
}
