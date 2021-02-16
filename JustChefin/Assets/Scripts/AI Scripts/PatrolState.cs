using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public float distance = 10f;
    private GameObject player;
    private Transform playerT;
    private Transform enemyT;

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
        player = enemy.player;
        enemyT = enemy.transform;
        playerT = enemy.player.transform;
        enemy.GetComponent<Renderer>().material.color = Color.green;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Debug.DrawRay(enemyT.position, playerT.position - enemyT.position, Color.green);
        // If it reaches the patrol point, change state to idle
        if (!enemy.nmAgent.pathPending && enemy.nmAgent.remainingDistance == 0f)
            enemy.ChangeState(new IdleState(enemy));

        // If the player is inside the view range and view angle
        if(enemy.IsPlayerInSightRange() && enemy.IsPlayerInSightAngle())
        {
            if(enemy.IsRaycastToPlayerSuccess())
            {
                enemy.ChangeState(new ChaseState(enemy));
            }
        }
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
