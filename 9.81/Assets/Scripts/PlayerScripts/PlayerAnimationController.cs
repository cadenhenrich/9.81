using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerState currentState = PlayerState.Idle;

    public PlayerState GetCurrentState()
    {
        return currentState;
    }

    public void SetCurrentState(PlayerState newState)
    {
        currentState = newState;
    }
}

[System.Serializable]
public enum PlayerState
{
    Idle,
    Run,
    GravCharge,
    GravHold,
    GravRelease,
    Damage,
}
