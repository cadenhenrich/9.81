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
  [SerializeField]
  private float earlyFallOnset;

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

  private void Fall()
  {
    if (!isGrounded)
    {
      velocity.y -= gravity * gravity * Time.deltaTime;
    }
    else
    {
      velocity.y = rb.velocity.y;
      isGrounded = Physics2D.Raycast(new Vector2(col.bounds.center.x -
          col.bounds.extents.x, col.bounds.min.y), Vector2.down,
          groundCheckDistance, groundLayer) ||
        Physics2D.Raycast(new Vector2(col.bounds.center.x +
          col.bounds.extents.x, col.bounds.min.y), Vector2.down,
          groundCheckDistance, groundLayer);
    }
  }

  private void FallEarly()
  {
    velocity.y = 0;
  }

  private void Jump()
  {
    if (isGrounded && Input.GetAxisRaw(jumpAxis) > 0)
    {
      isGrounded = false;
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
      if (contact.point.y <= col.bounds.center.y - col.bounds.extents.y)
      {
        isGrounded = true;
        return;
      }
      else if (contact.point.y >= col.bounds.center.y + col.bounds.extents.y)
      {
        velocity.y = rb.velocity.y;
      }
    }
  }
}
