using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    State currentState;
    public NavMeshAgent nmAgent;
    public GameObject player;
    public float sightRange;
    public float sightAngle;
    public float idleTime;
    public GameObject[] waitPoints;

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(new PatrolState(this));
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    // Function to change the state
    public void ChangeState(State state)
    {
        // If old state valid, then exit it
        if (currentState != null)
            currentState.OnExit();
        // Update to new state
        currentState = state;
        // If new state valid, then enter it
        if (currentState != null)
            currentState.OnEnter();
    }

    public bool IsPlayerInSightRange()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) <= sightRange)
        {
            return true;
        }
        return false;
    }

    public bool IsPlayerInSightAngle()
    {
        if (Vector3.Angle(this.transform.forward, player.transform.position - this.transform.position) <= sightAngle)
        {
            return true;
        }
        return false;
    }

    public bool IsRaycastToPlayerSuccess()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, player.transform.position - this.transform.position, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == player)
            {
                return true;
            }
        }
        return false;
    }
}
