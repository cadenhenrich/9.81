using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullBehavior : MonoBehaviour
{
    [Header("Push")]
    [SerializeField]
    private float pushForce;
    [SerializeField, Tooltip("The radius in which objects will be pushed")]
    private float pushEffectRadius;
    [SerializeField, Tooltip("The radius around the player where the effect can be used (set to 0 for infinite radius)")]
    private float pushUseRadius;
    [SerializeField]
    private float pushCooldown;

    private bool hasPushed;

    [Header("Pull")]
    [SerializeField]
    private float pullForce;
    [SerializeField, Tooltip("The radius in which objects will be pulled")]
    private float pullEffectRadius;
    [SerializeField, Tooltip("The radius around the player where the effect can be used (set to 0 for infinite radius)")]
    private float pullUseRadius;
    [SerializeField]
    private float pullCooldown;

    private bool hasPulled;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Fire1") > 0 && !hasPushed)
        {
            Push(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if (Input.GetAxisRaw("Fire2") > 0 && !hasPulled)
        {
            Pull(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    void Push(Vector2 coordinates)
    {
        if (pushUseRadius != 0 && Vector2.Distance(playerTransform.position, coordinates) <= pushUseRadius)
        {
            hasPushed = true;
            ApplyGravityEffect(coordinates, pushForce, pushUseRadius);
            StartCoroutine(PushCooldown());
        }
    }

    void Pull(Vector2 coordinates)
    {
        if (pullUseRadius != 0 && Vector2.Distance(playerTransform.position, coordinates) <= pullUseRadius)
        {
            hasPulled = true;
            ApplyGravityEffect(coordinates, -pullForce, pullEffectRadius);
            StartCoroutine(PullCooldown());
        }
    }

    void ApplyGravityEffect(Vector2 coordinates, float force, float radius)
    {
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(coordinates, radius);
        foreach (Collider2D collider in overlaps)
        {
            Rigidbody2D rigidbody = collider.GetComponent<Rigidbody2D>();
            if (rigidbody != null && !rigidbody.CompareTag("Player"))
            {
                Vector2 delta = rigidbody.position - coordinates;
                rigidbody.AddForce(delta.normalized * force + Vector2.up * force / 2f, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator PushCooldown()
    {
        yield return new WaitForSeconds(pushCooldown);

        hasPushed = false;
    }

    IEnumerator PullCooldown()
    {
        yield return new WaitForSeconds(pullCooldown);

        hasPulled = false;
    }
}
