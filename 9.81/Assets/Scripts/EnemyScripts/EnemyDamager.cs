using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour, Damager
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float cooldown;

    private bool canDamage = true;

    public bool CanDamagePlayer()
    {
        return canDamage;
    }

    public void DealDamage(Damageable damageable)
    {
        damageable.TakeDamage(damage);
        StartCoroutine(Cooldown());
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float damageToSet)
    {
        damage = damageToSet;
    }

    IEnumerator Cooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(cooldown);
        canDamage = true;
    }
}
