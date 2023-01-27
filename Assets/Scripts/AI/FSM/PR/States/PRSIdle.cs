using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRSIdle : FSMState<PRStateInfo>
{
    public override void doState(ref PRStateInfo infos)
    {
        infos.Controller.Animator.SetBool("Walk", false);
        infos.Controller.Animator.SetBool("Run", false);
    }
}