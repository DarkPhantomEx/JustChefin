using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public float distance = 10f;
    private GameObject player;
    private Transform playerT;
    private Transform enemyT;
    private Vector3 previousPoint;

    public PatrolState(EnemyAI enemy) : base(enemy)
    {
        previousPoint = new Vector3(0.0f, 0.0f, 0.0f);
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
        if (enemy.IsPlayerInSightRange() && enemy.IsPlayerInSightAngle() && enemy.IsRaycastToPlayerSuccess())
        {
            switch (enemy.tag)
            {
                case "Agro":
                    enemy.ChangeState(new ChaseState(enemy));
                    break;
                case "Passive":
                    if (enemy.psScript.GetRecipeBool())
                    {
                        enemy.ChangeState(new ChaseState(enemy));
                    }
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

    // Function to calculate the next wait point for the AI
    void MoveToNext()
    {
        Vector3 currentPoint;
        do
        {
            currentPoint = enemy.waitPoints[Random.Range(0, enemy.waitPoints.Length)].transform.position;
            Debug.Log("CurrentPoint: " + currentPoint);
        }
        while (currentPoint == previousPoint);
        
        //if (currentPoint != previousPoint)
        //{
            enemy.nmAgent.destination = currentPoint;
            previousPoint = currentPoint;
            Debug.Log("PreviousPoint: " + previousPoint);
        //}
    }
}
