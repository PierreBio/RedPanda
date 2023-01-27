using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRSBonneSante : FSMState<PRStateInfo>
{
    public override void doState(ref PRStateInfo infos)
    {
        if (infos.Controller.HasSeenTarget)
            addAndActivateSubState<PRSAgressif>();
        else
            addAndActivateSubState<PRSTranquille>();
    }
}