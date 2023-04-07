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
  [SerializeField]
  private string movementAxis;
  [SerializeField]
  private float maxSpeed;
  [SerializeField]
  private float acceleration;
  [SerializeField]
  private float friction;

  [Header("Jump")]
  [SerializeField]
  private string jumpAxis;
  [SerializeField]
  private float jumpSpeed;
  [SerializeField]
  private float groundCheckDistance;
  [SerializeField]
  private float gravity;

  private bool isGrounded;

  private Vector2 velocity;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    col = GetComponent<Collider2D>();
  }

  void FixedUpdate()
  {
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
      velocity.x += axis * acceleration * Time.fixedDeltaTime;
    }
  }

  private void Fall()
  {
    velocity.y = rb.velocity.y;

    if (!isGrounded)
    {
      velocity.y -= gravity * gravity * Time.fixedDeltaTime;
    }
    else
    {
      isGrounded = Physics2D.Raycast(new Vector2(col.bounds.center.x -
          col.bounds.extents.x, col.bounds.min.y), Vector2.down,
          groundCheckDistance, groundLayer) ||
        Physics2D.Raycast(new Vector2(col.bounds.center.x +
          col.bounds.extents.x, col.bounds.min.y), Vector2.down,
          groundCheckDistance, groundLayer);
    }
  }

  private void Jump()
  {
    if (isGrounded && Input.GetAxisRaw(jumpAxis) > 0)
    {
      isGrounded = false;
      velocity.y = jumpSpeed;
    }
  }

  private void ApplyFriction()
  {
    velocity.x = rb.velocity.x;
    velocity.x = Mathf.MoveTowards(velocity.x, 0, friction * Time.fixedDeltaTime);
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
      if (contact.point.y <= col.bounds.center.y - col.bounds.extents.y)
      {
        isGrounded = true;
        return;
      }
    }
  }
}
