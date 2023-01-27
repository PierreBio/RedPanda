using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamera : MonoBehaviour
{
    private float xAngleTarget;
    private float xAngleRotate;

    public float speed = 500f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
    }

    public void AddRotation(float deltaAngle)
    {
        xAngleTarget += deltaAngle;
        xAngleTarget = Mathf.Clamp(xAngleTarget, -60, 60);
    }

    public void Rotate()
    {
        float delta = xAngleTarget - xAngleRotate;
        float angleToAdd = speed * Time.fixedDeltaTime;
        angleToAdd = Mathf.Min(angleToAdd, Mathf.Abs(delta));
        xAngleRotate += Mathf.Sign(delta) * angleToAdd;
        transform.localRotation = Quaternion.AngleAxis(xAngleRotate, Vector3.right);
    }
}
