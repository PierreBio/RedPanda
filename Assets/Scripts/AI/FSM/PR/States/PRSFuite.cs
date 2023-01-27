using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PRSFuite : FSMState<PRStateInfo>
{
    private const string ANIMATION_BOOL_RUN = "Run";

    private float fleeDistance = 5f;
    bool hasFleeDestination = false;
    private Vector3 randomFleePosition;
    private EnemyController controller;
    private float DistPointReached = 2;


    public override void doState(ref PRStateInfo infos)
    {
        controller = infos.Controller;
        controller.IsFleeing = true;
        controller.Animator.SetBool(ANIMATION_BOOL_RUN, true);
        controller.Animator.SetBool("Walk", false);
        controller.CurrentSpeed = infos.Controller.FleeSpeed;

        if (!hasFleeDestination)
        {
            if (createValidFleePoint(ref randomFleePosition))
            {
                hasFleeDestination = true;
                controller.FindPathTo(randomFleePosition);
            }
        }
        else
        {
            if ((controller.transform.position - randomFleePosition).sqrMagnitude < DistPointReached * DistPointReached)
            {
                hasFleeDestination = false;
            }
        }

        KeepMeAlive = true;
    }

    public bool createValidFleePoint(ref Vector3 dest)
    {
        for (int i = 0; i < 30; i++)
        {
            if (sampleRandomDestination(controller.transform.position, fleeDistance, ref dest))
            {
                if (canBeReached(dest) && IsAwayFromPlayer(dest))
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

    public bool IsAwayFromPlayer(Vector3 destination)
    {
        if (Vector3.Distance(controller.Player.position, destination) > Vector3.Distance(controller.Player.position, controller.transform.position))
        {
            return true;
        }

        return false;
    }
}
