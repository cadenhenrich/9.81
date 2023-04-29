using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughObstacleBehavior : MonoBehaviour
{
    [SerializeField]
    private bool isPassthrough = true;
    [SerializeField]
    private float passthroughCooldown;

    private Collider2D col;
    private List<Collider2D> ignoredColliders;

    void Start()
    {
        col = GetComponent<Collider2D>();
        ignoredColliders = new List<Collider2D>();
    }

    public void StartBlocking()
    {
        StartCoroutine(Block());
    }

    public void SetPassthrough(bool value)
    {
        isPassthrough = value;
        foreach (Collider2D c in ignoredColliders)
        {
            Physics2D.IgnoreCollision(c, col, isPassthrough);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPassthrough && (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, col, true);
            ignoredColliders.Add(collision.collider);
        }
    }

    private IEnumerator Block()
    {
        SetPassthrough(false);
        yield return new WaitForSeconds(passthroughCooldown);
        SetPassthrough(true);
    }
}
