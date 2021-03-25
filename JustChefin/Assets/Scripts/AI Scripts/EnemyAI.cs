using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Reference to current state
    State currentState;
    public NavMeshAgent nmAgent;

    // Reference to player
    public GameObject player;

    // Enemy AI properties
    public float sightRange;
    public float sightAngle;
    public float idleTime;
    public GameObject[] waitPoints;

    // Reference to PlayerStatus script to be used in individual state classes
    public PlayerStatus psScript;
    // Reference to TopDownMovement script to be used in individual state classes
    public TopDownMovement tdmScript;

    // Start is called before the first frame update
    void Start()
    {
        psScript = GameObject.Find("TopDownPlayer").GetComponent<PlayerStatus>();
        tdmScript = GameObject.Find("TopDownPlayer").GetComponent<TopDownMovement>();

        // Set default start state as Patrol
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

    // Function to check if player falls inside enemy's sight distance
    public bool IsPlayerInSightDistance()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) <= sightRange)
        {
            return true;
        }
        return false;
    }

    // Function to check if player falls inside enemy's sight angle
    public bool IsPlayerInSightAngle()
    {
        if (Vector3.Angle(this.transform.forward, player.transform.position - this.transform.position) <= sightAngle)
        {
            return true;
        }
        return false;
    }

    // Function to check if raycast is possible from enemy's location to player's location
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
