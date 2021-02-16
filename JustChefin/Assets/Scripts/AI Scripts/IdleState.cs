using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private float timer;
    private float idleTime = 3f;
    private Transform playerT;
    private Transform enemyT;
    
    public IdleState(EnemyAI enemy) : base (enemy)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        timer = 0f;
        enemyT = enemy.transform;
        playerT = enemy.player.transform;
        enemy.GetComponent<Renderer>().material.color = Color.blue;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        timer += Time.deltaTime;
        Debug.DrawRay(enemyT.position, playerT.position - enemyT.position, Color.blue);

        // Switch to patrol state after idling for pre-assigned idleTime
        if (timer >= idleTime)
            enemy.ChangeState(new PatrolState(enemy));
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
