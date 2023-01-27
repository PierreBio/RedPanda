using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoActivation : MonoBehaviour
{
    [SerializeField] GameObject[] toActivate;

    private void OnEnable()
    {
        foreach (GameObject obj in toActivate)
            obj.SetActive(true);
    }


}
