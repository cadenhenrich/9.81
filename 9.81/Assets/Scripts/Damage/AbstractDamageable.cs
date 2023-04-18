using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDamageable : MonoBehaviour, Damageable
{
    [SerializeField]
    private float maxHealth;

    private float health;
    private bool isAlive = true;

    public float GetHealth()
    {
        return health;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    void Die()
    {
        isAlive = false;
    }

    void Start()
    {
        health = maxHealth;
    }
}
