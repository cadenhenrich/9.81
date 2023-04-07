using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy", order = 1)]
public class EnemyScriptableObject : ScriptableObject
{
    [Header("Stats")]
    public int hitsCanTake;
    public float speed;
    [Range(0.2f, 1f)] public float reactionTime;
    public Vector2 RandomIdleTimeInSecondsWhileWandering;

    [Header("Behaviors")]
    public AttackScriptableObject[] _typesOfAttacks;

    [Header("VFX/SFX")]
    Sprite sprite;
}
