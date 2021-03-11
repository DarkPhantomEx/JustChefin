using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    // Idle time counting variable
    private float timer;

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
        //enemy.GetComponent<Renderer>().material.color = Color.blue;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        // Idle time counter
        timer += Time.deltaTime;

        //Debug.DrawRay(enemyT.position, playerT.position - enemyT.position, Color.blue);

        // Switch to patrol state after idling for assigned idleTime
        if (timer >= enemy.idleTime)
            enemy.ChangeState(new PatrolState(enemy));

        // If the player is inside the sight distance, sight angle and it can raycast to player
        if (enemy.IsPlayerInSightDistance() && enemy.IsPlayerInSightAngle() && enemy.IsRaycastToPlayerSuccess())
        {
            switch(enemy.tag)
            {
                // If Agro then chase by default
                case "Agro":
                    enemy.ChangeState(new ChaseState(enemy));
                    break;
                // If Passive and player has recipe, then chase
                case "Passive":
                    if(enemy.psScript.GetHasRecipe())
                        enemy.ChangeState(new ChaseState(enemy));
                    break;
                default:
                    break;
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
