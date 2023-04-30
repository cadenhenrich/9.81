using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicalDamager : MonoBehaviour, Damager
{
    [Header("Damage")]
    [SerializeField]
    private float maxDamage;
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float minVelocity;
    [SerializeField, Range(0, 1)]
    private float velocityCoefficient;

    [Space]

    [SerializeField, Range(0, 1)]
    private float bulletTimeCoefficient;
    [SerializeField]
    private float bulletTimeLength;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void DealDamage(Damageable damageable)
    {
        if (rb.velocity.magnitude < minVelocity)
        {
            return;
        }

        float damage = Mathf.Clamp(maxDamage * (rb.velocity.magnitude * velocityCoefficient / maxVelocity), 0, maxDamage);
        damageable.TakeDamage(damage);
        StartCoroutine(BulletTime(damage));
    }

    public float GetDamage()
    {
        return maxDamage;
    }

    public bool CanDamagePlayer()
    {
        return false;
    }

    IEnumerator BulletTime(float damage)
    {
        Time.timeScale = Mathf.Lerp(1.0f, bulletTimeCoefficient, damage / maxDamage);
        Time.fixedDeltaTime = Mathf.Lerp(0.02f, 0.02f * bulletTimeCoefficient, damage / maxDamage);

        yield return new WaitForSeconds(bulletTimeLength);

        Time.fixedDeltaTime = 0.02f;
        Time.timeScale = 1.0f;
    }
}
