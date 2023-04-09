
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void OnEnterState(EnemyStateManager stateManager);

    public abstract void UpdateState(EnemyStateManager stateManager);
    public abstract void FixedUpdateState(EnemyStateManager stateManager);

    public abstract void OnCollisionEnter(EnemyStateManager stateManager, Collision collision);
}
