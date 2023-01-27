using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    private float totalDistance = 0;
    public float deltaFootSteps = 1;

    private Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        totalDistance += (lastPosition - transform.position).magnitude;
        if(totalDistance > deltaFootSteps)
        {
            totalDistance = 0;
            //Play sound
            AudioManager.GetInstance().Play("FootSteps", this.gameObject);
            
        }
        lastPosition = transform.position;
    }
}
