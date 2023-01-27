using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticulecolor : MonoBehaviour
{
    [SerializeField] private GameObject GreenCross, RedCross;

    void FixedUpdate ()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 500))
        {
            if(hit.transform.tag == "Enemy") 
            {
            RedCross.SetActive (true);
            GreenCross.SetActive (false);
            }

             else
        {
            RedCross.SetActive (false);
            GreenCross.SetActive (true);
        }
        }

       
    }
}
