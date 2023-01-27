using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{  
    void Start()
    {
       AudioManager.GetInstance().Play("Gunshot", this.gameObject);
    }
}
