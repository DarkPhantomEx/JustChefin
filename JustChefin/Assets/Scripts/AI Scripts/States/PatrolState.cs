using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    // Reference to player
    /*private GameObject player;
    private Transform playerT;
    private Transform enemyT;*/

    // Previously visited wait point
    //private Vector3 previousPoint;

    public PatrolState(EnemyAI enemy) : base(enemy)
    {
        // Set previous wait point to origin (basically something different than all the wait point values)
        //previousPoint = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        /*player = enemy.player;
        enemyT = enemy.transform;
        playerT = enemy.player.transform;*/

        if(enemy.tag == "Agro")
            enemy.GetComponent<Renderer>().material.color = Color.yellow;
        else if(enemy.tag == "Passive")
            enemy.GetComponent<Renderer>().material.color = Color.green;

        if (enemy.GetWaitPointIterator() < enemy.waitPoints.Length)
        {
            MoveToNext();
            enemy.IncrementWaitPointIterator();
        }
        else
        {
            enemy.ResetWaitPointIterator();
            MoveToNext();
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();

        //Debug.DrawRay(enemyT.position, playerT.position - enemyT.position, Color.green);

        // If it reaches the patrol point, change state to idle
        if (!enemy.nmAgent.pathPending && enemy.nmAgent.remainingDistance == 0f)
            enemy.ChangeState(new IdleState(enemy));

        // If the player is inside the sight distance, sight angle and it can raycast to player
        if (enemy.IsPlayerInSightDistance() && enemy.IsPlayerInSightAngle() && enemy.IsRaycastToPlayerSuccess())
        {
            switch (enemy.tag)
            {
                // If Agro then chase by default
                case "Agro":
                    enemy.ChangeState(new ChaseState(enemy));
                    break;
                // If Passive and player has recipe, then chase
                case "Passive":
                    if (enemy.psScript.GetHasRecipe())
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
        enemy.nmAgent.destination = enemy.waitPoints[enemy.GetWaitPointIterator()].transform.position;

        /*Vector3 currentPoint;
        do
        {
            currentPoint = enemy.waitPoints[Random.Range(0, enemy.waitPoints.Length)].transform.position;
            //Debug.Log("CurrentPoint: " + currentPoint);
        }
        while (currentPoint == previousPoint);

        enemy.nmAgent.destination = currentPoint;
        previousPoint = currentPoint;
        //Debug.Log("PreviousPoint: " + previousPoint);*/
    }
}
