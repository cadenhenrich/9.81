using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorController : MonoBehaviour
{
    [SerializeField, Tooltip("The distance to the player at which the door will open")]
    private float detectionDistance;

    private Transform playerTransform;
    private Animator anim;
    private Collider2D col;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null)
        {
            playerTransform = transform;
        }
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x-playerTransform.position.x) <= detectionDistance && 
            Mathf.Abs(transform.position.y-playerTransform.position.y) <= detectionDistance)
        {
            anim.SetBool("IsOpen", true);
            col.enabled = false;
        }
    }
}
