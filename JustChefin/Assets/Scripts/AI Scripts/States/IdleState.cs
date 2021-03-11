using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
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
        enemy.GetComponent<Renderer>().material.color = Color.blue;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        timer += Time.deltaTime;
        Debug.DrawRay(enemyT.position, playerT.position - enemyT.position, Color.blue);
        // Switch to patrol state after idling for pre-assigned idleTime
        if (timer >= enemy.idleTime)
            enemy.ChangeState(new PatrolState(enemy));

        // If the player is inside the view range and view angle
        if (enemy.IsPlayerInSightRange() && enemy.IsPlayerInSightAngle() && enemy.IsRaycastToPlayerSuccess())
        {
            switch(enemy.tag)
            {
                case "Agro":
                    enemy.ChangeState(new ChaseState(enemy));
                    break;
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
