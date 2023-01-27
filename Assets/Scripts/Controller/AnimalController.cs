using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class AnimalController : MonoBehaviour
{
    private bool _isFree;
    private List<Animator> _animators;
    public BaseCharacter baseCharacter;    

    public bool IsFree
    {
        get
        {
            return this._isFree;
        }
        set
        {
            this._isFree = value;
            if (this._isFree == true)
            {
                Free();
            }
        }
    }

    void Awake()
    {
        this._animators = this.GetComponentsInChildren<Animator>().ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Free();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
    }    

    public void UpdateAnimation()
    {
        if (this.baseCharacter.IsMoving)
        {
            foreach (Animator animator in this._animators)
            {
                animator.Play("Walk");
            }
        }
        else
        {
            foreach (Animator animator in this._animators)
            {
                animator.Play("Idle A");
            }
        }
    }

    public void Free()
    {
        StartCoroutine(Move(1.0f));
    }
    IEnumerator Move(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.baseCharacter.SetDirection(new Vector2(0, 1));
        StartCoroutine(WaitRandom(2.0f));
    }

    IEnumerator WaitRandom(float delay)
    {        
        yield return new WaitForSeconds(delay);
        this.baseCharacter.AddRotation(Random.value * 50 + 10);
        this.baseCharacter.SetDirection(new Vector2(0, 1));
        StartCoroutine(WaitRandom(2.0f));
    }









}
