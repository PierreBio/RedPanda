using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public GameObject destructibleFence;
    public GameObject destructedFence;
    public AnimalController animal;

    public List<GameObject> fences;
 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Animal"))
        {
            return;
        }

        if (collision.impulse.sqrMagnitude > 1)
        {
            this.destructedFence.SetActive(true);
            if (this.animal != null)
            {
                this.animal.IsFree = true;
            }
            Destroy(this.gameObject);
        }
    }
}
