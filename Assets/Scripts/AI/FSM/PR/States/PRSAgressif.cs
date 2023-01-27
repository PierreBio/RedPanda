using UnityEngine;

public class PRSAgressif : FSMState<PRStateInfo>
{
    public override void doState(ref PRStateInfo infos)
    {
        bool closeToTarget = (infos.LastPlayerPosition - infos.Controller.transform.position).sqrMagnitude < Mathf.Pow(infos.Controller.AttackDistance, 2);

        if (closeToTarget)
            addAndActivateSubState<PRSAttaque>();
        else
            addAndActivateSubState<PRSPoursuite>();
    }
}
