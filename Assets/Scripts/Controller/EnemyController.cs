using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // public
    [Header("Group")]
    public int GroupId = 0;
    [Header("Speeds")]
    public float PatrolSpeed = 5f;
    public float PoursuiteSpeed = 10f;
    public float FleeSpeed = 10f;
    [Header("Comportment")]
    public float AttackDistance = 4;
    public float TimeIdle = 5f;
    [Header("Debug")]
    public bool ShowDebug = false;
    [Header("Not used")]
    public float CurrentSpeed = 1f; // should be set by States that make it move
    public float PcentLife = 100f;

    [Header("Alert")]
    [SerializeField] private GameObject alertAoE = null;
    [SerializeField] private float minAlertSize = 2f;
    [SerializeField] private float maxAlertSize = 20f;
    [SerializeField] private float alertGrowSpeed = 1f;

    [Header("References")]
    [SerializeField] private Body myBody = null;
    [SerializeField] private Animator myAnimator = null;
    [SerializeField] private EnemyLifebar lifebar = null;
    [SerializeField] private WeaponBearer weaponBearer = null;

    public Animator Animator => myAnimator;
    public WeaponBearer WeaponBearer => weaponBearer;
 
    // private
    private FSMachine<PRSBase, PRStateInfo> FSM = new FSMachine<PRSBase, PRStateInfo>();
    PRStateInfo FSMInfos = new PRStateInfo();

    private bool hasSeenTarget = false;
    private bool isFleeing = false;
    private bool isDying = false;
    private Transform myPlayer = null;

    public Transform Player => myPlayer;
    public bool HasSeenTarget { get => hasSeenTarget; set => hasSeenTarget = value; }
    public bool IsFleeing { get => isFleeing; set => isFleeing = value; }
    public bool IsDying { get => isDying; set => isDying = value; }

    void Start()
    {
        lifebar.gameObject.SetActive(PcentLife + Mathf.Epsilon < 100f);

        myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().baseCharacter.transform;
        GetComponent<AISenseSight>().AddSenseHandler(new AISense<SightStimulus>.SenseEventHandler(HandleSight));
        GetComponent<AISenseSight>().AddObjectToTrack(myPlayer);

        myBody.OnLifeChangeEvent += OnLifeChange;
    }

    private void OnDestroy()
    {
        myBody.OnLifeChangeEvent -= OnLifeChange;
    }

    private void OnLifeChange()
    {
        BecomeAggressive();

        PcentLife = Mathf.Max(0, myBody.CurrentLife / myBody.InitialLife * 100f);

        lifebar.gameObject.SetActive(PcentLife + Mathf.Epsilon < 100f);
        lifebar.transform.localScale = new Vector3(PcentLife / 100f, lifebar.transform.localScale.y, lifebar.transform.localScale.z);

        isDying = myBody.CurrentLife <= 0;
    }

    void HandleSight(SightStimulus sti, AISense<SightStimulus>.Status evt)
    {
        if (!hasSeenTarget)
        {
            bool tempSeenTarget = evt == AISense<SightStimulus>.Status.Enter || evt == AISense<SightStimulus>.Status.Stay;
            if (tempSeenTarget)
            {
                BecomeAggressive();
            }
        }

        // if (evt == AISense<SightStimulus>.Status.Enter)
            // Debug.Log("Objet " + evt + " vue en " + sti.position);
    }

    void Update()
    {
        FSM.ShowDebug = this.ShowDebug;
        FSMInfos.Controller = this;
        FSMInfos.weaponBearer = GetComponent<WeaponBearer>();
        if (hasSeenTarget && !isFleeing)
        {
            if (alertAoE.activeInHierarchy == false)
            {
                alertAoE.SetActive(true);
                alertAoE.GetComponent<SphereCollider>().radius = minAlertSize;
            }
            else if (alertAoE.GetComponent<SphereCollider>().radius < maxAlertSize)
            {
                alertAoE.GetComponent<SphereCollider>().radius += alertGrowSpeed * Time.deltaTime;
            }

            FSMInfos.LastPlayerPosition = myPlayer.position;
        }

        FSM.Update(FSMInfos);
    }

    private void LateUpdate()
    {
        if (hasSeenTarget && !isFleeing)
        {
            var lookAtPos = new Vector3(myPlayer.position.x, transform.position.y, myPlayer.position.z);
            transform.LookAt(lookAtPos);
        }
    }

    public void FindPathTo(Vector3 dest)
    {
        GetComponent<NavMeshAgent>().isStopped = false;
        GetComponent<NavMeshAgent>().speed = CurrentSpeed;
        GetComponent<NavMeshAgent>().SetDestination(dest);
    }

    public void Stop()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
    }

    public void OnDrawGizmos()
    {
        float height = GetComponent<NavMeshAgent>().height;
        if (GetComponent<NavMeshAgent>().hasPath)
        {
            Vector3[] corners = GetComponent<NavMeshAgent>().path.corners;
            if (corners.Length >= 2)
            {
                Gizmos.color = Color.red;
                for (int i = 1; i < corners.Length; i++)
                {
                    Gizmos.DrawLine(corners[i - 1] + Vector3.up * height / 2, corners[i] + Vector3.up * height / 2);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemyController = other.GetComponentInParent<EnemyController>();
        if (other.tag == "Alert" && enemyController != null && enemyController.GroupId == GroupId)
        {
            BecomeAggressive();
        }
    }

    private void BecomeAggressive()
    {
        if(!hasSeenTarget)
        {
            AudioManager.GetInstance().Play("Aggressive", this.gameObject);
            AudioManager.GetInstance().FightLaunch();
            hasSeenTarget = true;
        }
    }
}
