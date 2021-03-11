using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class State
{
    // EnemyAI script reference
    protected EnemyAI enemy;

    public State(EnemyAI enemy)
    {
        this.enemy = enemy;
    }

    // Methods to be overridden individual states
    public virtual void OnEnter() { }

    public virtual void UpdateState() { }

    public virtual void OnExit() { }
}
