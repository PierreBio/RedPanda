using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField, Range (0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range (0, 30)] private float frequency = 10.0f;
    private Vector3 startPos;
    [SerializeField] private GameObject Player;
    bool moving;

    private void Awake ()
    {
        startPos = transform.position;
        
        
    }

    void Update ()
    {
        moving = Player.GetComponent<BaseCharacter>().IsMoving;
        if (moving)
        {
        transform.localPosition += new Vector3 (Mathf.Cos (Time.time * frequency) * amplitude, Mathf.Sin (Time.time * frequency/2) * amplitude*2,0);
        }

        else
        {
             transform.localPosition += new Vector3 (Mathf.Cos (Time.time * frequency/4) * amplitude/4, Mathf.Sin (Time.time * frequency/8) * ((amplitude*2) / 4),0);
        }
    }
}
