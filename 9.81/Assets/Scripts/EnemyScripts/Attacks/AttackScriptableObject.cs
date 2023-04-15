using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObject/Attack", order = 2)]
public class AttackScriptableObject : ScriptableObject
{
    public int damage;
    public float fireRange;
    public float fireSpeed;
    public bool isMelee;
    public float attackChargeUpInSeconds;
    public float attackCooldownInSeconds;
    //public AttackScriptableObject[] _attackSequence;
}
