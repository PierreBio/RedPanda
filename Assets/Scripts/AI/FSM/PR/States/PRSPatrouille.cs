using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PRSPatrouille : FSMState<PRStateInfo>
{
    bool hasPatrolDestination = false;
    private Vector3 randomPatrolPosition;
    private float patrolDistance = 5f;
    private EnemyController controller;
    private float DistPointReached = 2;

    public override void doState(ref PRStateInfo infos)
    {
        controller = infos.Controller;

        controller.Animator.SetBool("Walk", true);

        this.controller = infos.Controller;
        if (!hasPatrolDestination)
        {
            if (createValidPatrolPoint(ref randomPatrolPosition))
            {
                hasPatrolDestination = true;
                controller.CurrentSpeed = infos.Controller.PatrolSpeed;
                controller.FindPathTo(randomPatrolPosition);
            }
        }
        else
        {
            if ((controller.transform.position - randomPatrolPosition).sqrMagnitude < DistPointReached * DistPointReached)
            {
                hasPatrolDestination = false;
            }
        }

        KeepMeAlive = true;
    }

    public bool createValidPatrolPoint(ref Vector3 dest)
    {
        for (int i = 0; i < 10; i++)
        {

            if (sampleRandomDestination(controller.transform.position, patrolDistance, ref dest))
            {
                if (canBeReached(dest))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool sampleRandomDestination(Vector3 curentPosition, float distance, ref Vector3 destination)
    {
        Vector3 offset = Random.onUnitSphere;
        offset.y = 0;
        offset = offset.normalized * distance;
        offset.y = 1000;
        RaycastHit hitInfo;
        if (Physics.Raycast(curentPosition + offset, Vector3.down, out hitInfo))
        {
            destination = hitInfo.point;
            return true;
        }

        return false;
    }

    public bool canBeReached(Vector3 destination)
    {
        NavMeshAgent agent = controller.transform.GetComponent<NavMeshAgent>();
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(destination, path);
        if (path.status != NavMeshPathStatus.PathInvalid)
        {
            return true;
        }
        return false;
    }
}



