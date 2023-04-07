using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObject/Attack", order = 2)]
public class AttackScriptableObject : ScriptableObject
{
    public int damage;
    public float range;
    public float attackChargeUpInSeconds;
    public AttackScriptableObject[] _attackSequence;
}
