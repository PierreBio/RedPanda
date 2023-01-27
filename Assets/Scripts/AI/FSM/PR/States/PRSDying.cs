using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRSDying : FSMState<PRStateInfo>
{
    private bool alreadyDying = false;

    public override void doState(ref PRStateInfo infos)
    {
        if (!alreadyDying)
        {
            alreadyDying = true;
            infos.Controller.Stop();
            infos.Controller.Animator.SetTrigger("Death");
        }

        KeepMeAlive = true;
    }
}