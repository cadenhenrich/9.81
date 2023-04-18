using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicalDamager : MonoBehaviour, Damager
{
    [SerializeField]
    private float maxDamage;
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float velocityCoefficient;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void DealDamage(Damageable damageable)
    {
        damageable.TakeDamage(maxDamage * (rb.velocity.magnitude * velocityCoefficient / maxVelocity));
    }

    public float GetDamage()
    {
        return maxDamage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        if (damageable != null)
        {
            DealDamage(damageable);
        }
    }
}
