using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private PushPullState state;

    [Header("Push")]
    [SerializeField]
    private InputAction pushAction;
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
    private InputAction pullAction;
    [SerializeField]
    private float pullForce;
    [SerializeField, Tooltip("The radius in which objects will be pulled")]
    private float pullEffectRadius;
    [SerializeField, Tooltip("The radius around the player where the effect can be used (set to 0 for infinite radius)")]
    private float pullUseRadius;
    [SerializeField]
    private float pullCooldown;

    [Header("Misc")]
    [SerializeField, Range(0, 1)]
    private float bulletTimeCoefficient;

    [Header("VFX")]
    [SerializeField]
    private GameObject targetPrefab;
    private GameObject targetInstance;
    [SerializeField]
    private GameObject pushEffectPrefab;
    [SerializeField]
    private GameObject pullEffectPrefab;

    private Transform playerTransform;

    void Awake()
    {
        pushAction.started += OnPushTarget;
        pullAction.started += OnPullTarget;

        pushAction.canceled += OnPushCancel;
        pullAction.canceled += OnPullCancel;
    }

    void OnEnable()
    {
        pushAction.Enable();
        pullAction.Enable();
    }

    void OnDisable()
    {
        pushAction.Disable();
        pullAction.Disable();
    }

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

        state = PushPullState.Idle;
    }

    Vector3 GetPointInTargetRange(Vector3 point, float radius)
    {
        Vector2 point2 = new Vector2(point.x, point.y);
        Vector2 player2 = new Vector2(playerTransform.position.x, playerTransform.position.y);

        Vector2 offset = point2 - player2;
        float distance = Vector2.Distance(point2, player2);
        if (distance > radius)
        {
            offset = Vector2.ClampMagnitude(offset, radius);
        }

        return transform.position + new Vector3(offset.x, offset.y, 0.0f);
    }

    Vector3 GetMouseScreenPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnTarget(float radius)
    {
        if (state == PushPullState.Idle)
        {
            targetInstance = Instantiate(targetPrefab,
                GetPointInTargetRange(GetMouseScreenPosition(), radius),
                Quaternion.identity);
            Time.timeScale = bulletTimeCoefficient;
            state = PushPullState.Targeting;
        }
    }

    void OnPushTarget(InputAction.CallbackContext context)
    {
        OnTarget(pushUseRadius);
    }

    void OnPullTarget(InputAction.CallbackContext context)
    {
        OnTarget(pullUseRadius);
    }

    void OnPushCancel(InputAction.CallbackContext context)
    {
        if (state == PushPullState.Targeting)
        {
            Time.timeScale = 1.0f;
            Destroy(targetInstance);
            state = PushPullState.Pushing;
            Push(GetPointInTargetRange(GetMouseScreenPosition(), pushEffectRadius));
        }
    }

    void OnPullCancel(InputAction.CallbackContext context)
    {
        if (state == PushPullState.Targeting)
        {
            Time.timeScale = 1.0f;
            Destroy(targetInstance);
            state = PushPullState.Pulling;
            Pull(GetPointInTargetRange(GetMouseScreenPosition(), pullEffectRadius));
        }
    }

    void Update()
    {
        switch (state)
        {
            case PushPullState.Targeting:
                targetInstance.transform.position = GetPointInTargetRange(GetMouseScreenPosition(), pushEffectRadius);
                break;
            default:
                break;
        }
    }

    void Push(Vector2 coordinates)
    {
        ApplyGravityEffect(coordinates, pushForce, pushUseRadius);
        StartCoroutine(Cooldown(pushCooldown));
    }

    void Pull(Vector2 coordinates)
    {
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
