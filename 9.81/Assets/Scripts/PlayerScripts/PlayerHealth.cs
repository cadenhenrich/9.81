using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : AbstractDamageable
{
    [SerializeField]
    protected new float maxHealth;

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
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.enabled = false;
        }
        Invoke("GameManager.Instance.RestartLevel", 2f);
    }
}
