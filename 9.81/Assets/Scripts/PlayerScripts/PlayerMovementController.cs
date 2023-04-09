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
  [SerializeField]
  private LayerMask groundLayer;

  [Header("Movement")]
  // Horizontal movement controls
  [SerializeField]
  private string movementAxis;
  [SerializeField]
  private float maxSpeed;
  [SerializeField]
  private float acceleration;
  [SerializeField]
  private float friction;

  [Header("Jump")]
  // Jump controls
  [SerializeField]
  private string jumpAxis;
  [SerializeField]
  private float jumpSpeed;

  [Space]

  // Grounding checks
  [SerializeField]
  private float groundCheckDistance;

  [Space]

  // Basic falling
  [SerializeField]
  private float gravity;
  [SerializeField]
  private float maxFallSpeed;

  [Space]

  // Early fall if player releases jump
  [SerializeField]
  private float earlyFallOnset;
  [SerializeField]
  private float earlyFallVelocity;

  [Space]

  // Apex extension
  [SerializeField]
  private float apexThreshold;

  [Space]

  // Coyote time
  [SerializeField]
  private float coyoteTime;
  private bool inCoyoteTime;

  [Space]

  // Bump the player up an edge if they just barely don't make it
  [SerializeField]
  private float bumpThreshold;

  private bool isGrounded;

  private Vector2 velocity;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    col = GetComponent<Collider2D>();
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
          Vector2.down, groundCheckDistance, groundLayer) ||
        Physics2D.Raycast(new Vector2(col.bounds.max.x, col.bounds.min.y),
          Vector2.down, groundCheckDistance, groundLayer) ||
        Physics2D.Raycast(new Vector2(col.bounds.center.x,
                col.bounds.min.y), Vector2.down,
              groundCheckDistance, groundLayer))
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

  private void Jump()
  {
    if ((isGrounded || inCoyoteTime) && Input.GetAxisRaw(jumpAxis) > 0)
    {
      isGrounded = false;
      inCoyoteTime = false;
      velocity.y = jumpSpeed;
      return;
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
