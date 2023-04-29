using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(EnemyStateManager))]
public class EnemyHealth : AbstractDamageable
{
    private new float maxHealth; 
    private float xKnockback;
    private float yKnockback;

    private Animator anim;
    private EnemyStateManager stateManager;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        stateManager = GetComponent<EnemyStateManager>();
        maxHealth = stateManager.enemyScriptableObject.hitsCanTake;
        health = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Damager damager = collision.gameObject.GetComponent<Damager>();
        
        if (damager != null)
        {
            damager.DealDamage(this);
            if (collision.relativeVelocity.magnitude > 15) 
            {
                Vector2 collisionDirection = collision.contacts[0].normal;
                Vector2 forceMagnitude = new Vector2(0, yKnockback);
                collisionDirection *= xKnockback;
                collisionDirection += forceMagnitude;

                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.AddForce(collisionDirection, ForceMode2D.Impulse);
            }

            StartCoroutine(Stun());
        }
    }

    protected override void Die()
    {
        base.Die();
        GameManager.Instance.EnemyDied();
        Destroy(gameObject);
    }

    public void SetKnockBack(float xForceScale, float yForceScale)
    {
        xKnockback = xForceScale;
        yKnockback = yForceScale;
    }

    public void SetHealth(float hp)
    {
        health = hp;
    }

    private IEnumerator Stun()
    {
        anim.SetBool("IsStunned", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("IsStunned", false);
    }
}
