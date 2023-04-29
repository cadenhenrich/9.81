using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingObstacleBehavior : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void Detatch()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
