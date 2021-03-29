﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    private Transform playerT;
    private Transform enemyT;
    
    public ChaseState(EnemyAI enemy) : base(enemy, "Chase")
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

        //Debug.DrawRay(enemyT.position, playerT.position - enemyT.position, Color.red);

        // Move towards the player and stay away 1 unit distance
        enemy.nmAgent.destination = playerT.position - enemyT.forward;
        // Stop player movement
        enemy.tdmScript.SetCanMove(false);

        // If it reaches the patrol point, change state to idle
        //Debug.Log(enemy.nmAgent.pathPending + " " + enemy.nmAgent.remainingDistance);
        if (!enemy.nmAgent.pathPending && enemy.nmAgent.remainingDistance <= 2f)// BUG HERE
        {
            enemy.psScript.LoseLife();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
