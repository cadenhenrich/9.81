using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : AbstractDamageable
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Damager damager = collision.gameObject.GetComponent<Damager>();
        if (damager != null && damager.CanDamagePlayer())
        {
            damager.DealDamage(this);
        }
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Player died");
    }
}
