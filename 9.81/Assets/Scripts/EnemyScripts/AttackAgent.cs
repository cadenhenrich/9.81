using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAgent : MonoBehaviour
{
    public void ExecuteMovementAttack(Rigidbody2D subjectRb, Vector2 target, AttackScriptableObject attackScriptableObject)
    {
        Vector2 direction = (target - subjectRb.position).normalized;
        Vector2 force = direction * attackScriptableObject.fireSpeed;
        subjectRb.AddForce(force, ForceMode2D.Impulse);
    }
}
