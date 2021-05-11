using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    // Idle time counting variable
    private float timer;

    private Transform playerT;
    private Transform enemyT;

    private Quaternion TargetRotation;
    private int p = 1;
    private float speed;
    public IdleState(EnemyAI enemy) : base (enemy, "Idle")
    {
    }
    int t;
    public override void OnEnter()
    {
        base.OnEnter();
        timer = 0f;
        enemyT = enemy.transform;
        playerT = enemy.player.transform;
        speed = enemy.nmAgent.angularSpeed/2; //Rotating Speed
        if (enemy.tag == "Agro")
            enemy.GetComponentInChildren<Light>().color = Color.yellow;
        else if (enemy.tag == "Passive")
        {
            if (!enemy.psScript.GetHasRecipe())
                enemy.GetComponentInChildren<Light>().color = Color.blue;
            else
                enemy.GetComponentInChildren<Light>().color = Color.yellow;
        }
        //Rotation Calculate
        if (Mathf.Abs(enemyT.transform.rotation.eulerAngles.y - 270) < Mathf.Abs(enemyT.transform.rotation.eulerAngles.y - 90))
            TargetRotation = Quaternion.LookRotation(Vector3.right);
        else
            TargetRotation = Quaternion.LookRotation(Vector3.left);       
        if (enemy.GetWaitPointIterator() == 0)
            t = enemy.waitPoints.Length - 1;
        else
            t = enemy.GetWaitPointIterator() - 1;
        
    }

    public override void UpdateState()
    {
        base.UpdateState();
       
        //Rotating/Adjust facing
        if (enemy.Moving&&enemy.Rotating)
        {           
            enemyT.transform.rotation = Quaternion.RotateTowards(this.enemyT.rotation, enemy.waitPoints[t].transform.rotation, speed * Time.deltaTime);
        }
        if (!enemy.Moving)
        {
            
            switch (p)
            {
                case 1:
                    enemyT.transform.rotation = Quaternion.RotateTowards(this.enemyT.rotation, Quaternion.LookRotation(Vector3.back), speed * Time.deltaTime);
                    if (enemyT.transform.rotation == Quaternion.LookRotation(Vector3.back))
                    {
                        p++;
                    }
                    break;
                case 2:
                    enemyT.transform.rotation = Quaternion.RotateTowards(this.enemyT.rotation, TargetRotation, speed * Time.deltaTime);
                    if (enemyT.transform.rotation == TargetRotation)
                    {
                        p++;
                    }
                    break;
                default:
                    break;
            }
        }


        // Idle time counter
        timer += Time.deltaTime;

        //Debug.DrawRay(enemyT.position, playerT.position - enemyT.position, Color.blue);

        // Switch to patrol state after idling for assigned idleTime
        if (timer >= enemy.idleTime)
            enemy.ChangeState(new PatrolState(enemy));

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
}
