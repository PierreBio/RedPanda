public class PRSPoursuite : FSMState<PRStateInfo>
{
    public override void doState(ref PRStateInfo infos)
    {
        infos.Controller.Animator.SetBool("Run", true);
        infos.Controller.Animator.SetBool("Walk", false);
        infos.Controller.CurrentSpeed = infos.Controller.PoursuiteSpeed;
        infos.Controller.FindPathTo(infos.LastPlayerPosition);
    }
}