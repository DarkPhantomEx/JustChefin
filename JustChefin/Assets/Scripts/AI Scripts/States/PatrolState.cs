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

    public PatrolState(EnemyAI enemy) : base(enemy, "Patrol")
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

        if (enemy.tag == "Agro")
            enemy.GetComponentInChildren<Light>().color = Color.yellow;
        else if (enemy.tag == "Passive")
        {
            if (!enemy.psScript.GetHasRecipe())
                enemy.GetComponentInChildren<Light>().color = Color.blue;
            else
                enemy.GetComponentInChildren<Light>().color = Color.yellow;
        }

        if (enemy.GetWaitPointIterator() < enemy.waitPoints.Length)   
                
        {
            MoveToNext();
            enemy.IncrementWaitPointIterator();
            if (enemy.GetWaitPointIterator() >= enemy.waitPoints.Length)
                enemy.ResetWaitPointIterator();

        }               
    }

    public override void UpdateState()
    {
        base.UpdateState();
        //if (enemy.transform.name == "Waiter (1)")
           // Debug.Log(enemy.GetWaitPointIterator());
        //Debug.DrawRay(enemyT.position, playerT.position - enemyT.position, Color.green);

        // If it reaches the patrol point, change state to idle
        if (!enemy.nmAgent.pathPending && enemy.nmAgent.remainingDistance == 0f)
            enemy.ChangeState(new IdleState(enemy));
        
        // If the suspicion bar gets completely filled
        if (enemy.GetSuspicionValue() == 1)
        {            
            enemy.ChangeState(new ChaseState(enemy));
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
