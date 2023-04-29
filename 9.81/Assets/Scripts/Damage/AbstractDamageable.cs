using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDamageable : MonoBehaviour, Damageable
{
    protected float maxHealth;

    [SerializeField]
    protected AudioClip hitSound;
    protected AudioSource audioSource;

    protected float health;
    protected bool isAlive = true;

    public float GetHealth()
    {
        return health;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public virtual void TakeDamage(float damage)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    protected virtual void Die()
    {
        isAlive = false;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

}
