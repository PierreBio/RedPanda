using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Body : MonoBehaviour
{
    public bool isVulnerableMelee = true;

    [SerializeField] private Slider Healthbar;
    [SerializeField] private bool Player;
    [SerializeField] private float initialLife = 100f;

    private float currentLife;

    public float CurrentLife => currentLife;
    public float InitialLife => initialLife;
    public string sceneName;

    public delegate void OnLifeChangeEventHandler();
    public event OnLifeChangeEventHandler OnLifeChangeEvent;

    void Start()
    {
        currentLife = initialLife; 
    }

    void Update()
    {
        if (Healthbar != null)
            Healthbar.value = currentLife;

        if (currentLife <= 0)
        {
            if(Player == false)
            {
                //animator.SetTrigger("Death");
                
            }
            else
            {
                AudioManager.GetInstance().FightStop();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void AfterDeath ()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    public void RecoveryHealth(int RecoveryHP)
    {
        currentLife += RecoveryHP;
        OnLifeChangeEvent?.Invoke();
    }

    public void TakeDamage(int HitDamage)
    {
        Debug.Log(HitDamage);
        currentLife -= HitDamage;
        OnLifeChangeEvent?.Invoke();
    }

    public void HitMelee(int HitDamageMelee)
    {
        if (isVulnerableMelee)
        {
            currentLife -= HitDamageMelee;
            isVulnerableMelee = false;
            OnLifeChangeEvent?.Invoke();
        }
    }

    public void TakeDamageExplode(int HitDamageExplode)
    {
        if(!Player)
        {
             Debug.Log("HitEnnemy");

             AudioManager.GetInstance().Play("EnnemyDamage", this.gameObject);
             AudioManager.GetInstance().Play("HitEnnemy", this.gameObject);
        }
        else
        {

        }
        currentLife -= HitDamageExplode;
        OnLifeChangeEvent?.Invoke();
    }

}
