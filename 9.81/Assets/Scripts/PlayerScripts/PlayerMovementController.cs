using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovementController : MonoBehaviour
{
  private Rigidbody2D rb;
  private Collider2D col;

  [Header("Ground")]
  [SerializeField, Tooltip("Layer mask for ground")]
  private LayerMask groundLayer;

  [Header("Movement")]
  // Horizontal movement controls
  [SerializeField, Tooltip("Axis to move horizontally")]
  private string movementAxis = "Horizontal";
  [SerializeField, Tooltip("Maximum walk speed")]
  private float maxSpeed;
  [SerializeField, Tooltip("Acceleration")]
  private float acceleration;
  [SerializeField, Tooltip("Friction (slow down)")]
  private float friction;

  [Header("Jump")]
  // Jump controls
  [SerializeField, Tooltip("Axis to jump")]
  private string jumpAxis = "Jump";
  [SerializeField, Tooltip("Jump speed/height")]
  private float jumpSpeed;

  [Space]

  // Grounding checks
  [SerializeField, Tooltip("Distance to check for ground")]
  private float groundCheckDistance = 0.05f;

  [Space]

  // Basic falling
  [SerializeField, Tooltip("Gravity")]
  private float gravity;
  [SerializeField, Tooltip("Maximum fall speed")]
  private float maxFallSpeed;

  [Space]

  // Early fall if player releases jump
  [SerializeField, Tooltip("Early fall onset (player releases jump)")]
  private float earlyFallOnset;
  [SerializeField, Tooltip("Early fall velocity")]
  private float earlyFallVelocity;

  [Space]

  // Apex extension
  [SerializeField, Tooltip("Apex threshold/air sustain")]
  private float apexThreshold;

  [Space]

  // Coyote time
  [SerializeField, Tooltip("Coyote time")]
  private float coyoteTime;
  private bool inCoyoteTime = false;

  [Space]

  // Bump the player up an edge if they just barely don't make it
  [SerializeField, Tooltip("Bump-up ledge threshold")]
  private float bumpThreshold;

  [Space]

  // If the player presses jump just before they hit the ground
  // they should still jump when they are grounded
  [SerializeField, Tooltip("Jump queue time")]
  private float jumpQueueThreshold;
  private bool jumpQueued;

  private bool isGrounded;

  private Animator anim;
  private float initialXScale;

  private Vector2 velocity;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    col = GetComponent<Collider2D>();
    anim = GetComponentInChildren<Animator>();
    initialXScale = transform.localScale.x;
  }

  // Update is called every frame
  void Update()
  {
    // Check if player is grounded
    CheckGrounded();

    // Decrease velocity
    Fall();
    ApplyFriction();

    // Increase velocity
    Walk();
    Jump();

    // Apply changes
    ApplyVelocity();

    if (anim != null)
    {
      if (rb.velocity.magnitude > 0.01f)
      {
        anim.SetBool("IsMoving", true);
        if (rb.velocity.x < 0)
        {
          transform.localScale = new Vector3(-initialXScale, transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x > 0)
        {
          transform.localScale = new Vector3(initialXScale, transform.localScale.y, transform.localScale.z);
        }
      }
      else
      {
        anim.SetBool("IsMoving", false);
      }
    }
  }

  private void Walk()
  {
    float axis = Input.GetAxisRaw(movementAxis);

    if (axis != 0)
    {
      velocity.x += axis * acceleration * Time.deltaTime;
    }
  }

  private void CoyoteTime()
  {
    inCoyoteTime = false;
  }

  private void CheckGrounded()
  {
    if (Physics2D.Raycast(new Vector2(col.bounds.min.x, col.bounds.min.y),
          Vector2.down, groundCheckDistance) ||
        Physics2D.Raycast(new Vector2(col.bounds.max.x, col.bounds.min.y),
          Vector2.down, groundCheckDistance) ||
        Physics2D.Raycast(new Vector2(col.bounds.center.x,
                col.bounds.min.y), Vector2.down,
              groundCheckDistance))
    {
      isGrounded = true;
      inCoyoteTime = false;
    }
  }

  private void Fall()
  {
    if (!isGrounded)
    {
      if (-apexThreshold < velocity.y && velocity.y < apexThreshold)
      {
        velocity.y -= gravity * Time.deltaTime;
      }
      else
      {
        velocity.y -= gravity * gravity * Time.deltaTime;
      }
      velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);
    }
    else
    {
      velocity.y = rb.velocity.y;
      if (!(Physics2D.Raycast(new Vector2(col.bounds.min.x, col.bounds.min.y),
              Vector2.down, groundCheckDistance, groundLayer) ||
            Physics2D.Raycast(new Vector2(col.bounds.max.x, col.bounds.min.y),
              Vector2.down, groundCheckDistance, groundLayer)))
      {
        isGrounded = false;
        inCoyoteTime = true;
        Invoke("CoyoteTime", coyoteTime);
      }
      else
      {
        isGrounded = true;
      }
    }
  }

  private void FallEarly()
  {
    velocity.y = -earlyFallVelocity;
  }

  private void ResetJumpQueue()
  {
    jumpQueued = false;
  }

  private void Jump()
  {
    if (jumpQueued || Input.GetAxisRaw(jumpAxis) > 0)
    {
      if (isGrounded || inCoyoteTime)
      {
        isGrounded = false;
        inCoyoteTime = false;
        jumpQueued = false;
        velocity.y = jumpSpeed;
        return;
      }
      else if (!(jumpQueued || isGrounded))
      {
        jumpQueued = true;
        Invoke("ResetJumpQueue", jumpQueueThreshold);
      }
    }

    // Fall early if jump is released before the peak
    if (velocity.y > 0 && Input.GetAxisRaw(jumpAxis) <= 0)
    {
      Invoke("FallEarly", earlyFallOnset);
    }
  }

  private void ApplyFriction()
  {
    if (!col.IsTouchingLayers(groundLayer))
    {
      velocity.x = rb.velocity.x;
    }
    velocity.x = Mathf.MoveTowards(velocity.x, 0, friction * Time.deltaTime);
  }

  private void ApplyVelocity()
  {
    velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);

    rb.velocity = velocity;
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    foreach (ContactPoint2D contact in collision.contacts)
    {
      if (contact.point.x > col.bounds.min.x && contact.point.x < col.bounds.max.x &&
          contact.point.y <= col.bounds.min.y)
      {
        isGrounded = true;
        return;
      }
      else if (velocity.y > 0 &&
          contact.point.x <= col.bounds.min.x && contact.point.x >= col.bounds.max.x &&
          contact.point.y <= col.bounds.min.y + bumpThreshold)
      {
        transform.Translate(new Vector2(0f,contact.point.y - col.bounds.min.y));
      }
      else if (contact.point.y >= col.bounds.max.y)
      {
        velocity.y = rb.velocity.y;
      }
    }
  }
}
