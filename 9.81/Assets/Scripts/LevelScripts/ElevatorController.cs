using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElevatorController : MonoBehaviour
{
    [SerializeField, Tooltip("The range the player must be in to use the elevator")]
    private float range;
    [SerializeField, Tooltip("Can this elevator be used?")]
    private bool usable;
    [SerializeField, Tooltip("The elevator's destination")]
    private Transform destination;

    [Space]

    [SerializeField, Tooltip("The input for using the elevator")]
    private InputAction useAction;
    [SerializeField, Tooltip("The prompt to show when the player is in range")]
    private GameObject usePrompt;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null)
        {
            playerTransform = transform;
        }

        useAction.started += OnUse;
    }

    private void Update()
    {
        if (Vector3.Distance(playerTransform.position, transform.position) < range)
        {
            usePrompt.SetActive(true);
        }
        else
        {
            usePrompt.SetActive(false);
        }
    }

    private void OnUse(InputAction.CallbackContext context)
    {
        if (usePrompt.activeSelf)
        {
            playerTransform.position = destination.position;
        }
    }
}
