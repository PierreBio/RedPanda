using UnityEngine;

public class PRSAttaque : FSMState<PRStateInfo>
{
    public bool isAttacking = false;

    public override void doState(ref PRStateInfo infos)
    {
        if (infos.Controller.WeaponBearer.IsAbleToAttack)
        {
            Debug.Log("I CAN ATTACK");
            isAttacking = false;
        }

        if (!isAttacking)
        {
            isAttacking = true;
            infos.Controller.Stop();
            infos.weaponBearer.Attack();
        }

        KeepMeAlive = true;
    }
}
