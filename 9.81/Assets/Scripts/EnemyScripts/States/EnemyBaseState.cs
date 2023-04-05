
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void OnEnterState(EnemyStateManager stateManager);

    public abstract void UpdateState(EnemyStateManager stateManager);

    public abstract void OnCollisionEnter(EnemyStateManager stateManager);
}
