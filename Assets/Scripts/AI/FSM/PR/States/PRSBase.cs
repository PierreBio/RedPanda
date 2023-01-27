public class PRSBase : FSMState<PRStateInfo>
{
    public float SeuilBonneSante = 30;

    public override void doState(ref PRStateInfo infos)
    {
        if (infos.Controller.IsDying)
            addAndActivateSubState<PRSDying>();
        else if (infos.Controller.PcentLife > SeuilBonneSante)
            addAndActivateSubState<PRSBonneSante>();
        else
            addAndActivateSubState<PRSFuite>();

        KeepMeAlive = true;
    }
}