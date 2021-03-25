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

    // Suspicion bar variables
    public float minSuspicionDistance;
    public SuspicionBar suspicionBar;
    private float playerDistance;
    private static float currentSuspicionValue;

    // For going through the waitpoints sequentially
    private int waitPointIterator;

    // Reference to PlayerStatus script to be used in individual state classes
    public PlayerStatus psScript;
    // Reference to TopDownMovement script to be used in individual state classes
    public TopDownMovement tdmScript;

    // Start is called before the first frame update
    void Start()
    {
        psScript = GameObject.Find("TopDownPlayer").GetComponent<PlayerStatus>();
        tdmScript = GameObject.Find("TopDownPlayer").GetComponent<TopDownMovement>();

        waitPointIterator = 0;
        playerDistance = Vector3.Distance(this.transform.position, player.transform.position);

        // Default values for suspicion bar
        currentSuspicionValue = 0f;
        suspicionBar.SetMinSuspicion(GetMinSuspicionValue());

        // Set default start state as Patrol
        ChangeState(new PatrolState(this));
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
        //Debug.DrawRay(this.transform.position, this.transform.forward * 6f, Color.yellow);

        // Calculate distance between the player and the enemy
        playerDistance = Vector3.Distance(this.transform.position, player.transform.position);

        // If raycast from enemy to the player is successful
        if(IsRaycastToPlayerSuccess())
        {
            switch (this.tag)
            {
                // If Agro then update suspicion bar
                case "Agro":
                    UpdateSuspicionBar();
                    break;
                // If Passive and player has recipe, then update suspicion bar
                case "Passive":
                    if (this.psScript.GetHasRecipe())
                    {
                        UpdateSuspicionBar();
                    }
                    break;
                default:
                    break;
            }
        }
        // Reset suspicion bar if player is behind any object
        if (IsRaycastToPlayerFailed())
            suspicionBar.SetMinSuspicion(GetMinSuspicionValue());
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

    // Function to check if raycast is NOT possible from enemy's location to player's location
    public bool IsRaycastToPlayerFailed()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, player.transform.position - this.transform.position, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject != player)
            {
                return true;
            }
        }
        return false;
    }

    // Function to calculate suspicion bar value
    public void UpdateSuspicionBar()
    {
        // Detemine the value of suspicion bar
        if (playerDistance <= minSuspicionDistance)
        {
            currentSuspicionValue = 1 - ((playerDistance - sightRange) / (minSuspicionDistance - sightRange));
            suspicionBar.SetSuspicion(currentSuspicionValue);
        }
    }

    // Getter and Updaters for waitPointIterator variable
    public int GetWaitPointIterator() { return waitPointIterator; }
    public void IncrementWaitPointIterator() { waitPointIterator++; }
    public void ResetWaitPointIterator() { waitPointIterator = 0; }

    // Getter for the minimum value of the suspicion bar
    public float GetMinSuspicionValue() { return suspicionBar.slider.minValue; }
}
