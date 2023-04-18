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
        pushAction.started += OnTarget;
        pullAction.started += OnTarget;

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
        return Vector3.ClampMagnitude(point - transform.position, radius) + transform.position;
    }

    void OnTarget(InputAction.CallbackContext context)
    {
        if (state == PushPullState.Idle)
        {
            targetInstance = Instantiate(targetPrefab,
                new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                    Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                    0),
                Quaternion.identity);
            state = PushPullState.Targeting;
        }
    }

    void OnPushCancel(InputAction.CallbackContext context)
    {
        if (state == PushPullState.Targeting)
        {
            state = PushPullState.Pushing;
            Push(targetInstance.transform.position);
            Destroy(targetInstance);
        }
    }

    void OnPullCancel(InputAction.CallbackContext context)
    {
        if (state == PushPullState.Targeting)
        {
            state = PushPullState.Pulling;
            Pull(targetInstance.transform.position);
            Destroy(targetInstance);
        }
    }

    void Update()
    {
        switch (state)
        {
            case PushPullState.Targeting:
                targetInstance.transform.position =
                    new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                        Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                        0);
                break;
            default:
                break;
        }
    }

    void Push(Vector2 coordinates)
    {
        if (pushUseRadius != 0)
        {
            coordinates = Vector2.ClampMagnitude(coordinates, pushUseRadius);
        }

        ApplyGravityEffect(coordinates, pushForce, pushUseRadius);
        StartCoroutine(Cooldown(pushCooldown));
    }

    void Pull(Vector2 coordinates)
    {
        if (pullUseRadius != 0)
        {
            coordinates = Vector2.ClampMagnitude(coordinates, pullUseRadius);
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
