using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private float timer;
    private float idleTime = 3f;
    
    public IdleState(EnemyAI enemy) : base (enemy)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        timer = 0f;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        timer += Time.deltaTime;

        // Switch to patrol state after idling for pre-assigned idleTime
        if (timer >= idleTime)
            enemy.ChangeState(new PatrolState(enemy));
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
