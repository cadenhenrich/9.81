using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy", order = 1)]
public class EnemyScriptableObject : ScriptableObject
{
    [Header("Stats")]
    public int hitsCanTake;
    public float speed;
    public float jumpHeight;
    public float detectionRadius;
    [Tooltip("The refresh rate of enemy pathing")] [Range(0.2f, 1f)] public float reactionTime;
    [Tooltip("From/To")] public RangeData RandomIdleTimeInSecondsWhileWandering;
    public float xKnockbackScale;
    public float yKnockbackScale;

    [Header("Behaviors")]
    [Tooltip("Randomly selects an attack from this list")] public AttackScriptableObject[] _typesOfAttacks;
    [Tooltip("Case sensitive tag of the enemy's target")] public string targetTag;

    [Header("VFX/SFX")]
    Sprite sprite;
}

[System.Serializable]
public class RangeData
{
    public float minSeconds;
    public float maxSeconds;
}
