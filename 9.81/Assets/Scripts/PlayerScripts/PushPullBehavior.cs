using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullBehavior : MonoBehaviour
{
  [Header("Push")]
  [SerializeField]
  private float pushForce;
  [SerializeField]
  private float pushRadius;
  [SerializeField]
  private float pushCooldown;

  private bool hasPushed;

  [Header("Pull")]
  [SerializeField]
  private float pullForce;
  [SerializeField]
  private float pullRadius;
  [SerializeField]
  private float pullCooldown;

  private bool hasPulled;

  void Start()
  {
    
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
    hasPushed = true;
    ApplyGravityEffect(coordinates, pushForce);
    StartCoroutine(PushCooldown());
  }

  void Pull(Vector2 coordinates)
  {
    hasPulled = true;
    ApplyGravityEffect(coordinates, -pullForce);
    StartCoroutine(PullCooldown());
  }

  void ApplyGravityEffect(Vector2 coordinates, float force)
  {
    Collider2D[] overlaps = Physics2D.OverlapCircleAll(coordinates, pushRadius);
    foreach (Collider2D collider in overlaps)
    {
      Rigidbody2D rigidbody = collider.GetComponent<Rigidbody2D>();
      if (rigidbody != null)
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
