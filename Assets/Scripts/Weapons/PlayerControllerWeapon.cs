using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerWeapon : MonoBehaviour
{
    [SerializeField] WeaponBearer weaponBearer;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float upSpeed;

    [SerializeField] Transform playerCamera;

    [SerializeField] Animator debugAnim;

    Vector2 movementVector;
    Vector2 lookVector;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnFire()
    {
        weaponBearer.Attack();
        }

    void OnRecharge()
    {
        debugAnim.Play("Attack");
        weaponBearer.Recharge();

    }

    void OnMove(InputValue movementVariable)
    {
        movementVector = movementVariable.Get<Vector2>();
    }

    void OnLook(InputValue movementVariable)
    {
        lookVector = movementVariable.Get<Vector2>();
    }

    void OnPickUp()
    {
        weaponBearer.Switch();
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.right * movementVector.x * Time.deltaTime * speed);
        transform.Translate(Vector3.forward * movementVector.y * Time.deltaTime * rotationSpeed);


        transform.Rotate(Vector3.up, lookVector.x * Time.deltaTime * rotationSpeed, Space.World);
        playerCamera.Rotate(Vector3.right, - lookVector.y * Time.deltaTime * upSpeed, Space.Self);
    }
}
