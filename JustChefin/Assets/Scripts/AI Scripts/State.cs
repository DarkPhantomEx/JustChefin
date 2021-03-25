using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class State
{
    // EnemyAI script reference
    protected EnemyAI enemy;
    protected string name;
    public State(EnemyAI enemy, string name)
    {
        this.enemy = enemy;
        this.name = name;
    }

    // Methods to be overridden individual states
    public virtual void OnEnter() { }

    public virtual void UpdateState() { }

    public virtual void OnExit() { }

    public string GetName()
    {
        return (name);
    }
}
