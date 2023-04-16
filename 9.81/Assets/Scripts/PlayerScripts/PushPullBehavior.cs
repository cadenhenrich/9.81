using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullBehavior : MonoBehaviour
{
    private enum PushPullState
    {
        Targeting,
        Pushing,
        Pulling,
        Recharging,
        Idle
    };

    private PushPullState state = PushPullState.Idle;

    [Header("Push")]
    [SerializeField]
    private float pushForce;
    [SerializeField, Tooltip("The radius in which objects will be pushed")]
    private float pushEffectRadius;
    [SerializeField, Tooltip("The radius around the player where the effect can be used (set to 0 for infinite radius)")]
    private float pushUseRadius;
    [SerializeField]
    private float pushCooldown;

    [Header("Pull")]
    [SerializeField]
    private float pullForce;
    [SerializeField, Tooltip("The radius in which objects will be pulled")]
    private float pullEffectRadius;
    [SerializeField, Tooltip("The radius around the player where the effect can be used (set to 0 for infinite radius)")]
    private float pullUseRadius;
    [SerializeField]
    private float pullCooldown;

    [Header("VFX")]
    [SerializeField]
    private GameObject targetPrefab;
    private GameObject targetInstance;
    [SerializeField]
    private GameObject pushEffectPrefab;
    [SerializeField]
    private GameObject pullEffectPrefab;

    private Transform playerTransform;

    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            playerTransform = transform;
        }
        else
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        switch (state)
        {
            case PushPullState.Idle:
                if (Input.GetAxisRaw("Fire1") > 0 || Input.GetAxisRaw("Fire2") > 0)
                {
                    state = PushPullState.Targeting;
                    targetInstance = Instantiate(targetPrefab,
                        Camera.main.ScreenToWorldPoint(Input.mousePosition),
                        Quaternion.identity);
                }

                break;
            case PushPullState.Targeting:
                targetInstance.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (Input.GetAxisRaw("Fire1") <= 0)
                {
                    Destroy(targetInstance);
                    state = PushPullState.Pushing;
                    Push(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }
                else if (Input.GetAxisRaw("Fire2") <= 0)
                {
                    Destroy(targetInstance);
                    state = PushPullState.Pulling;
                    Pull(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }

                break;
            case PushPullState.Pushing:
            case PushPullState.Pulling:
            case PushPullState.Recharging:
            default:
                break;
        }
    }

    void Push(Vector3 coordinates)
    {
        if (pushUseRadius != 0)
        {
            coordinates = Vector3.ClampMagnitude(coordinates, pushUseRadius);
        }

        ApplyGravityEffect(coordinates, pushForce, pushUseRadius);
        StartCoroutine(Cooldown(pushCooldown));
    }

    void Pull(Vector3 coordinates)
    {
        if (pullUseRadius != 0)
        {
            coordinates = Vector3.ClampMagnitude(coordinates, pullUseRadius);
        }

        ApplyGravityEffect(coordinates, -pullForce, pullEffectRadius);
        StartCoroutine(Cooldown(pullCooldown));
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

    IEnumerator Cooldown(float seconds)
    {
        state = PushPullState.Recharging;
        yield return new WaitForSeconds(seconds);
        state = PushPullState.Idle;
    }
}
