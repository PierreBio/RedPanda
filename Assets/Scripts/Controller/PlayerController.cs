using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float sensibility = 0.5f;

    public BaseCharacter baseCharacter;
    private BaseCamera _baseCamera;
    private WeaponBearer _weaponBearer;

    private bool reset = true;
    private float _oldScroll = 0f; 
    // Start is called before the first frame update
    void Awake()
    {
        //GetComponent
        this.baseCharacter = GetComponentInChildren<BaseCharacter>();
        this._baseCamera = GetComponentInChildren<BaseCamera>();
        this._weaponBearer = GetComponentInChildren<WeaponBearer>();

        //Lock et Met le curseur invisible 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(InputValue value)
    {        
        Vector2 dir = value.Get<Vector2>();
        baseCharacter.SetDirection(dir);
    }

    public void OnJump()
    {
        this.baseCharacter.SetJump();
    }


    public void OnShoot()
    {
        //direction = Random.onUnitSphere* 10f;
        //direction.y = 0.0f;
        //_baseCharacter.SetRotationToLookAt(direction);

        _weaponBearer.Attack();
        //Debug.Log("Shoot");
    }

    public void OnMouse(InputValue value)
    {
        Vector2 mouseDelta = value.Get<Vector2>();
        baseCharacter.AddRotation(mouseDelta.x * sensibility);
        _baseCamera.AddRotation(-mouseDelta.y * sensibility);
    }

    public void OnReload(InputValue value)
    {
        _weaponBearer.Recharge();
        //Debug.Log("Reload");
    }

    public void OnSwitchWeapon(InputValue value)
    {
        float scroll = value.Get<float>();

        if(scroll > 0 && scroll != _oldScroll)
        {
            _oldScroll = scroll;
            _weaponBearer.Switch();
        }
        else if(scroll < 0 && scroll != _oldScroll)
        {
            _oldScroll = scroll;
            _weaponBearer.Switch();
        }
        
    }
    

}
