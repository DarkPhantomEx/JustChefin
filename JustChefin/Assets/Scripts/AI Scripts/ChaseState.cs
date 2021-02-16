﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    private Transform playerT;
    private Transform enemyT;

    public ChaseState(EnemyAI enemy) : base(enemy)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemyT = enemy.transform;
        playerT = enemy.player.transform;
        enemy.GetComponent<Renderer>().material.color = Color.red;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Debug.DrawRay(enemyT.position, playerT.position - enemyT.position, Color.red);
        // Keep chasing the player
        enemy.nmAgent.destination = playerT.transform.position - (enemyT.forward * 1f);

        // If player is out of view
        if(Vector3.Distance(enemyT.position, playerT.position) >= enemy.sightRange)
        {
            Vector3 searchTrail = enemyT.position - playerT.position;
            enemy.nmAgent.destination = searchTrail;
            enemy.ChangeState(new PatrolState(enemy));
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
