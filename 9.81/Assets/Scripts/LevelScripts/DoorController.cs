using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField, Tooltip("The Y offset the door will have when open")]
    private float offset;
    [SerializeField, Tooltip("How fast the door will open and close")]
    private float speed;
    [SerializeField, Tooltip("The distance to the player at which the door will open")]
    private int distance;

    private Vector3 initialPosition;

    private Transform playerTransform;

    void Start()
    {
        initialPosition = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null)
        {
            playerTransform = transform;
        }
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x-playerTransform.position.x) <= distance && 
            Mathf.Abs(transform.position.y-playerTransform.position.y) <= distance)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition + new Vector3(0, offset, 0), speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
        }
    }
}
