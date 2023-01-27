using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRSTranquille : FSMState<PRStateInfo>
{ 
    private float TempoIdle = 0;
    private bool Init = true;

    public override void doState(ref PRStateInfo infos)
    {
        TempoIdle += infos.PeriodUpdate;

        if (TempoIdle > infos.Controller.TimeIdle || Init)
        {
            TempoIdle = 0;
            Init = false;
            if (isActiveSubstate<PRSIdle>())
            {
                addAndActivateSubState<PRSPatrouille>();
            }
            else
            {
                addAndActivateSubState<PRSIdle>();
            }
        }

        KeepMeAlive = true; //Sinon on perds la tempo, l'init etc...
    }
}
