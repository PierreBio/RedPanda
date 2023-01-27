using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAmmo : MonoBehaviour
{
    public int nbRecharge = 1;

    public float floatingSpeed = 1f;
    public float floatingAmp = 0.5f;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
        Vector3 offsetY = new Vector3(0, floatingAmp * Mathf.Sin(2 * Mathf.PI * Time.time * floatingSpeed));
        transform.position = startPosition + offsetY;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponBearer wb = other.GetComponent<WeaponBearer>();
            wb.PickupAmmo(nbRecharge);
        }
    }
}
