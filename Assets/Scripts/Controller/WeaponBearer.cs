using UnityEngine;
using System;
using System.Collections;

public class WeaponBearer : MonoBehaviour
{
    [SerializeField]
    private bool usingMelee;

    [SerializeField]
    private Weapon meleeWeapon;

    [SerializeField]
    private Weapon rangeWeapon;

    public bool IsAbleToAttack => usingMelee ? meleeWeapon.isAbleToAttack : rangeWeapon.isAbleToAttack;

    

    [SerializeField]
    private EnemyController myController;

    [SerializeField] private bool isSwitching = false;

    [SerializeField] Animator anim;

    private Vector3 m_AccumulatedRecoil;
    private Vector3 m_WeaponRecoilLocalPosition;

    // RECOIL
    public float MaxRecoilDistance { get; private set; } = 0.5f;
    public float RecoilSharpness { get; private set; } = 50f;
    public float RecoilRestitutionSharpness { get; private set; } = 10f;

    private Vector3 objectToRecoilOriginalPos;
    public GameObject objToRecoil;

    protected /*override*/ void Start()
    {
        if (meleeWeapon != null)
            meleeWeapon.gameObject.SetActive(usingMelee);

        if (myController == null) // player
        {
            anim.SetBool(GetCurrentWeapon().animBoolean, true);

            if (rangeWeapon != null)
            {
                rangeWeapon.gameObject.SetActive(!usingMelee);
                objectToRecoilOriginalPos = objToRecoil.transform.localPosition;
            }
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        Debug.Log("EquipWeapon");
        StartCoroutine(EquipWeaponCoroutine(weapon)); // Wait for current switching to end to change weapon
    }

    

    public void PickupAmmo(int nbRecharges)
    {
        //if (rangeWeapon != null)
        //{
        //    rangeWeapon.PickupAmmo(nbRecharges);
        //}
    }

    public void Attack()
    {
        if (myController != null) // for enemies
        {
            myController.Animator.SetTrigger("Attack");
        }
        if (!isSwitching) // always true for enemies
        {
            if (usingMelee)
                StartCoroutine(meleeWeapon.Fire());
            else
            {
                if (rangeWeapon.isAbleToAttack)
                {
                    m_AccumulatedRecoil += Vector3.back;
                    m_AccumulatedRecoil = Vector3.ClampMagnitude(m_AccumulatedRecoil, MaxRecoilDistance);
                }
                Debug.Log("Wb Attack");
                StartCoroutine(rangeWeapon.Fire());  
            }               
        }
    }

    private void Update()
    {
        if (rangeWeapon != null && myController == null) // for player only
            {
            UpdateWeaponRecoil();
            if (rangeWeapon.gameObject.activeSelf)
                objToRecoil.transform.localPosition = objectToRecoilOriginalPos + m_WeaponRecoilLocalPosition;
        }
            
    }

    void UpdateWeaponRecoil()
    {
        // if the accumulated recoil is further away from the current position, make the current position move towards the recoil target
        if (m_WeaponRecoilLocalPosition.z >= m_AccumulatedRecoil.z * 0.99f)
        {
            m_WeaponRecoilLocalPosition = Vector3.Lerp(m_WeaponRecoilLocalPosition, m_AccumulatedRecoil,
                RecoilSharpness * Time.deltaTime);
        }
        // otherwise, move recoil position to make it recover towards its resting pose
        else
        {
            m_WeaponRecoilLocalPosition = Vector3.Lerp(m_WeaponRecoilLocalPosition, Vector3.zero,
                RecoilRestitutionSharpness * Time.deltaTime);
            m_AccumulatedRecoil = m_WeaponRecoilLocalPosition;
        }
    }

    public void Recharge()
    {
        //if (!usingMelee)
        //    rangeWeapon.OnRecharge();
    }

    public void Switch() // Only applies to player, start switch
    {
        if (myController != null)
            throw new Exception("Ennemies can't switch weapon (yet?)");
        if (!isSwitching)
        {
            if ((usingMelee && rangeWeapon != null) || (!usingMelee && meleeWeapon != null))
            {
                isSwitching = true;
                anim.SetBool(GetCurrentWeapon().animBoolean, false);
                anim.SetBool(GetOtherWeapon().animBoolean, true);
            }
        }
    }

    public void SwitchEnded()
    {
        isSwitching = false;
    }


    public void AttackStarted()
    {
        GetCurrentWeapon().AttackStarted();
    }

    public void AttackEnded()
    {
        GetCurrentWeapon().AttackEnded();
    }

    public void SwitchWeapons() // Physically switch the weapons (in the middle of the switch animation)
    {
        if (usingMelee)
        {
            if (rangeWeapon != null)
            {
                rangeWeapon.gameObject.SetActive(true);
                meleeWeapon.gameObject.SetActive(false);
                usingMelee = false;
                
            }
        }
        else
        {
            if (meleeWeapon != null)
            {
                rangeWeapon.gameObject.SetActive(false);
                meleeWeapon.gameObject.SetActive(true);
                usingMelee = true;
            }
        }
    }

    public Weapon GetCurrentWeapon()
    {
        if (usingMelee)
        {
            return meleeWeapon;
        }

        return rangeWeapon;
    }

    public Weapon GetOtherWeapon()
    {
        if (usingMelee)
        {
            return rangeWeapon;
        }

        return meleeWeapon;
    }


    IEnumerator EquipWeaponCoroutine(Weapon weapon)
    {
        while (true)
        {
            if (!isSwitching)
            {
                weapon.gameObject.SetActive(true);
                if (weapon.isMelee)
                {
                    if (meleeWeapon != null)
                        meleeWeapon.gameObject.SetActive(false);
                    meleeWeapon = weapon;
                }
                else
                {
                    if (rangeWeapon != null)
                        rangeWeapon.gameObject.SetActive(false);
                    rangeWeapon = weapon;
                }
                yield break;
            }
        }
    }

    // peut �tre utile de s'inspirer de �a :
    /*
    public void Attack(Vector3 targetPosition, Vector3 targetVelocity)
    {
        //Regarde l'ennemi mais reste droit
        Vector3 lookPos = targetPosition;
        lookPos.y = transform.position.y;
        transform.LookAt(targetPosition);

        //Cr�ation du projectile
        Transform proj = GameObject.Instantiate<Transform>(projectile);
        proj.position = transform.position + Vector3.up + transform.forward; //on le lance de devant nous

        //Si on tire sans mouvement de la cible, quels sont les param�tres
        Vector3 velocityProjectile;
        getParamsShoot(targetPosition, proj.position, out velocityProjectile);

        //Si mouvement de la cible on en tient compte
        if (targetVelocity.sqrMagnitude > 0.1f)
        {
            //Duree du trajet
            float distance = (targetPosition - proj.position).magnitude;
            float dureeTrajet = distance / new Vector3(velocityProjectile.x, 0, velocityProjectile.z).magnitude;

            //Decalage estim� du perso
            Vector3 positionApresMouvement = targetPosition + targetVelocity * dureeTrajet;

            getParamsShoot(positionApresMouvement, proj.position, out velocityProjectile);
        }


        proj.GetComponent<Rigidbody>().velocity = velocityProjectile * 1.1f;
        proj.GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * 3.0f; //On fait tourner le projectile en l'air, pour le fun
    }
    */
}
