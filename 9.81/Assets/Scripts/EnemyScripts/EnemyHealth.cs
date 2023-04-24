using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHealth : AbstractDamageable
{
    private float xKnockback;
    private float yKnockback;
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
            
            
        }
    }

    protected override void Die()
    {
        base.Die();
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
}
