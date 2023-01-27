using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannonBall : MonoBehaviour
{
    public float amplitude = 0.5f;

    private Rigidbody parent;
    private Vector3 parentVelocity;

    private Vector3 scale;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.GetComponent<Rigidbody>();
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        parentVelocity = parent.velocity;

        if (parentVelocity == Vector3.zero) {
            parentVelocity = new Vector3(0, -1, 0);
        }


        transform.rotation = Quaternion.LookRotation(- parentVelocity.normalized);
        transform.localScale = new Vector3(scale.x, scale.y, (0.5f + amplitude * Mathf.Sqrt(parentVelocity.magnitude)) * scale.z);
    }
}
