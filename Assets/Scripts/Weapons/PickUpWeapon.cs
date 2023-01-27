using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    [SerializeField]
    private Weapon weapon;

    public float floatingSpeed = 1f;
    public float floatingAmp = 0.5f;

    private Vector3 startPosition;

    private void Start() {
        startPosition = transform.position;
    }

    private void Update() {
        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
        Vector3 offsetY = new Vector3(0, floatingAmp * Mathf.Sin(2 * Mathf.PI * Time.time * floatingSpeed));
        transform.position = startPosition + offsetY;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter");

        if (other.transform.parent != null && other.transform.parent.tag == "Player")
        {
            GameObject player = GameObject.FindWithTag("Player");
            WeaponBearer wb = player.GetComponent<WeaponBearer>();
            wb.EquipWeapon(weapon);
            Debug.Log("equipped");
            Destroy(gameObject);
        }
    }
}
