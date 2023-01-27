using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAbleAnimal : MonoBehaviour
{
    public Transform animalHolder;
    public GameObject animalLauncher;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(animalLauncher.activeSelf && collision.gameObject.tag == "Animal")
        {
            if(animalHolder.childCount == 0)
            {
                
                collision.transform.SetParent(animalHolder);
                collision.transform.localPosition = Vector3.zero;
                collision.transform.localScale = Vector3.one;
                SetLayerRecursively(collision.gameObject, 
                    collision.transform.parent.gameObject.layer);
                //collision.gameObject.GetComponentInChildren<Collider>().enabled = false;
                //collision.transform.GetChild(0).transform.SetParent(animalHolder);
                //Destroy(collision.transform.parent.gameObject);
            }

        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

}
