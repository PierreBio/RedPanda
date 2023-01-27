using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Les infos passées au state pour le jeu FrozenBattle
public class PRStateInfo : FSMStateInfo
{
    public EnemyController Controller;
    public WeaponBearer weaponBearer;
    public Vector3 LastPlayerPosition;
}
