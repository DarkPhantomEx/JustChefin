using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    // Patrol points defined
    private Vector3[] points = { new Vector3(3.5f, 2.7f, -33f),
                                 new Vector3(3.5f, 2.7f, -4f),
                                 new Vector3(32f, 2.7f, -4f),
                                 new Vector3(32f, 0f, -38f) };

    public PatrolState(EnemyAI enemy) : base(enemy)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        MoveToNext();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (!enemy.nmAgent.pathPending && enemy.nmAgent.remainingDistance == 0f)
            enemy.ChangeState(new IdleState(enemy));
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    void MoveToNext()
    {
        enemy.nmAgent.destination = points[Random.Range(0, 3)];
    }
}
