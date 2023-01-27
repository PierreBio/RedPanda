using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private Vector2 _direction;
    private float _Yrotation;
    private float _YrotationTarget;
    private float _distToGround;
    private Vector3 _jumpDirection;
    private GameObject lastWallJumped;
    private GameObject currentGameObjectCollide;

    private List<ContactPoint> _allContactPoints;
    private Vector3 averageNormal;
    private Vector3 dirWallJump;
    private int _numFrameUpdate = 0;
    private int _averageNormalFrameUpdate = 0;
    [SerializeField]
    private int _nbCollisionNormal = 0;
    private Vector3 _collisionNormalSum = Vector3.zero;

    [SerializeField]
    private bool _isGrounded;
    [SerializeField]
    private bool _isWalled;
    [SerializeField]
    private bool _wantToJump = false;

    private float tempoGrounded =0;


    public float speed = 500.0f;
    public float angularSpeed = 10000.0f;
    public float smoothRotation = 0.01f;

    public float normalJumpForce = 10.0f;    
    public float wallJumpForce = 15.0f;

    public float gravityForce = 0.2f;

    #region Properties

    public bool IsGrounded
    {
        get
        {
            return this._isGrounded;
        }
        set
        {
            this._isGrounded = value;
            if(this._isGrounded == true)
            {
                lastWallJumped = null;
            }
            this._rigidbody.drag = (this._isGrounded == true || this._isWalled ) ? 3 : 0;
        }
       
    }

    public bool IsWalled
    {
        get
        {       
            return this._isWalled;
        }
        set
        {
            this._isWalled = value;
            this._rigidbody.drag = (this._isGrounded == true || this._isWalled) ? 3 : 0;

        }
    }

    public bool IsMoving
    {
        get
        {
            return (Mathf.Abs(this._rigidbody.velocity.x) + Mathf.Abs(this._rigidbody.velocity.z) >= 1.0f);
        }
        
    }

    #endregion

    #region DefaultMethods

    void Awake()
    {
        this._allContactPoints = new List<ContactPoint>();
        this._rigidbody = this.GetComponent<Rigidbody>();
        this._distToGround = this.GetComponentInChildren<Collider>().bounds.extents.y;

    }

    private void FixedUpdate()
    {
        this._nbCollisionNormal = 0;
        Rotate();
        Jump();
        Move();

        CheckIfGroundedAndWalled();

        tempoGrounded -= Time.fixedDeltaTime;
        IsGrounded = tempoGrounded > 0;
    }

    #endregion

    #region Setters
    public virtual void SetRotation(float yRotation)
    {
        this._YrotationTarget = yRotation;
        while (this._YrotationTarget >= 360)
        {
            this._YrotationTarget -= 360.0f;
        }
    }        

    // pour le player
    public void AddRotation(float deltaRot)
    {
        SetRotation(this._YrotationTarget + deltaRot);        
    }

    // pour les ennemis
    public virtual void LookRotation(Vector3 direction)
    {
        SetRotation(Vector3.SignedAngle(Vector3.forward, direction, Vector3.up));
    }


    public virtual void SetDirection(Vector2 keyAxis)
    {
        this._direction = keyAxis;
    }

    public void SetJump()
    {
        this._wantToJump = true;
        if (this._nbCollisionNormal > 0)
        {
            this.averageNormal = this._collisionNormalSum / this._nbCollisionNormal;
        }
        else
        {
            this.averageNormal = Vector3.zero;
        }
        this._collisionNormalSum = Vector3.zero;
        this._nbCollisionNormal = 0;
        this._jumpDirection = Vector3.zero;
        
        if (averageNormal.sqrMagnitude > float.Epsilon)
        {
            this._jumpDirection = Vector3.Lerp(this.transform.up, averageNormal.normalized, 0.5f);
        }
        
    }

    #endregion

    #region MainMethods


    public void Rotate()
    {
        float deltaRot = this._YrotationTarget - this._Yrotation;
        float rotationIncrement = this.angularSpeed * Time.fixedDeltaTime;
        rotationIncrement = Mathf.Lerp(rotationIncrement * smoothRotation, rotationIncrement, Mathf.Min(Mathf.Abs(deltaRot), 180) / 180);
        rotationIncrement = Mathf.Min(Mathf.Abs(deltaRot), rotationIncrement);
        this._Yrotation += Mathf.Sign(deltaRot) * rotationIncrement;
        this.transform.rotation = Quaternion.AngleAxis(this._Yrotation, Vector3.up);
    }    

    public void Move()
    {
        if (!IsWalled && IsGrounded && this._direction.sqrMagnitude > float.Epsilon)
        {
            Vector3 newDirection = this.transform.right * this._direction.x + this.transform.forward * this._direction.y;
            Vector3 velocity = newDirection * this.speed * Time.fixedDeltaTime + Vector3.up * this._rigidbody.velocity.y;
            this._rigidbody.velocity = velocity;
        }
        else if (!IsGrounded && !IsWalled)
        {
            Vector3 newDirection = this.transform.right * this._direction.x + this.transform.forward * this._direction.y;
            Vector3 velocity = newDirection * this.speed * Time.fixedDeltaTime + Vector3.up * this._rigidbody.velocity.y;
            this._rigidbody.velocity = Vector3.Lerp(velocity, this._rigidbody.velocity, 0.9f);
        }
        else if (IsWalled)
        {
            Vector3 newDirection = this.transform.right * this._direction.x + this.transform.forward * this._direction.y;
            Vector3 velocity = newDirection * this.speed * Time.fixedDeltaTime;
            velocity.y = this._rigidbody.velocity.y;
            
            if (!IsGrounded && this._rigidbody.velocity.y <= 0)
            {
                this._rigidbody.velocity = Vector3.Lerp(velocity, this._rigidbody.velocity, 0.9f);
                this._rigidbody.velocity = Vector3.Lerp(new Vector3(this._rigidbody.velocity.x, -1, this._rigidbody.velocity.z), this._rigidbody.velocity, 0.2f);
                //this._rigidbody.velocity = new Vector3(this._rigidbody.velocity.x, -5, this._rigidbody.velocity.z);
            }
        }

        if (!IsGrounded)
        {
            AddGravity();
        }
 
    }

    public void Jump()
    {      

        if (this._wantToJump == true)
        {
            this._wantToJump = false;
            if (IsGrounded)
            {
                NormalJump();
                IsGrounded = false;
            }
            else if (IsWalled)
            {
                WallJump();
                IsWalled = false;
            }
        }
    }

    public void NormalJump()
    {
        //Play Sound
        
        AudioManager.GetInstance().Play("JumpGround",this.gameObject);
        this._rigidbody.AddForce(Vector3.up * this.normalJumpForce, ForceMode.Impulse);
    }

    public void WallJump()
    {
        if (currentGameObjectCollide != lastWallJumped)
        {
            //Play Sound
            AudioManager.GetInstance().Play("JumpWall",this.gameObject);
            this._rigidbody.AddForce(this._jumpDirection.normalized * this.wallJumpForce, ForceMode.Impulse);
            lastWallJumped = currentGameObjectCollide;
        }
    }

    #endregion

    #region SecondaryMethods
   


    public void Stop()
    {
        this._rigidbody.velocity = Vector3.zero;
        this._rigidbody.angularVelocity = Vector3.zero;
    }

    #region Collisions

    private void OnCollisionStay(Collision collision)
    {
        ProcessCollision(collision);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ProcessCollision(collision);
    }

    private void ProcessCollision(Collision collision)
    {
        currentGameObjectCollide = collision.gameObject;
        this._allContactPoints.Clear();
        collision.GetContacts(this._allContactPoints);

        for (int i = 0; i < collision.contactCount; i++)
        {
            if (this._allContactPoints[i].normal.sqrMagnitude > float.Epsilon)
            {
                this._collisionNormalSum += this._allContactPoints[i].normal;
                this._nbCollisionNormal++;

            }
        }
        
        CheckIfGroundedAndWalled();

    }

    #endregion

    private void AddGravity()
    {
        this._rigidbody.velocity -= new Vector3(0, this.gravityForce, 0);
    }

    public void CheckIfGroundedAndWalled()
    {
        if (Physics.Raycast(this.transform.position + Vector3.up * this._distToGround, -Vector3.up, this._distToGround + 0.1f))
        {
            tempoGrounded = 0.3f;
        }
        //IsGrounded = (Mathf.Abs(this._rigidbody.velocity.y) < 0.01);
        IsWalled = !IsGrounded && this._nbCollisionNormal > 0;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + averageNormal * 10);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _jumpDirection * 10);
    }

    #endregion

}
