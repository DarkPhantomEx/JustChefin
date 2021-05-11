using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* The enemy NPC script that uses the State Pattern AI Design */

public class EnemyAI : MonoBehaviour
{
    // Reference to current state
    State currentState;
    public NavMeshAgent nmAgent;

    // Reference to player
    public GameObject player;
    // Reference to player head
    [SerializeField]
    private GameObject playerHead;
    public GameObject enemyHead;

    // Enemy AI properties
    public float sightRange;
    public float sightAngle;
    public float idleTime;
    public GameObject[] waitPoints;
    public bool Moving;
    public bool Rotating;
    private GameObject[] temp;

    // Suspicion bar variables
    public float maxSuspicionDistance;
    public float minSuspicionDistance = 3;
    public SuspicionBar suspicionBar;
    public float IncrementSpeed = 5;
    private float playerDistance;
    private float currentSuspicionValue = 0;

    // For going through the waitpoints sequentially
    private int waitPointIterator;
    // Reference to PlayerStatus script to be used in individual state classes
    public PlayerStatus psScript;
    // Reference to TopDownMovement script to be used in individual state classes
    public TopDownMovement tdmScript;

    // Start is called before the first frame update
    void Start()
    {
        
        psScript = GameObject.Find("Iris").GetComponent<PlayerStatus>();
        tdmScript = GameObject.Find("Iris").GetComponent<TopDownMovement>();
        playerHead = GameObject.Find("/Iris/Head");

        waitPointIterator = 0;
        playerDistance = Vector3.Distance(this.transform.position, player.transform.position);

        temp = waitPoints;
        
        // Set default start state as Patrol
        ChangeState(new PatrolState(this));
    }

    // Update is called once per frame
    void Update()
    {
        //Unique Movement Path for Waiter_2Chibi
        if (this.transform.name == "Waiter_2Chibi")
        {            
            if (psScript.GetHasRecipe())
            {                
                if (waitPoints.Length > 1)
                {
                    System.Array.Resize(ref waitPoints, 1);
                    this.ChangeState(new PatrolState(this));
                }
                else
                {
                    Vector3 a = this.transform.position, b = waitPoints[0].transform.position;
                    a.y = 0; b.y = 0;
                    if (Vector3.Distance(a, b) <= 0.01)
                        Moving = false;
                }
            }
            else
            {
                waitPoints = temp;
                Moving = true;
            }
        }

        //Unique Movement Path for Waiter_Chibi
        if (this.transform.name == "Waiter_Chibi")
        {
            if (psScript.GetHasRecipe())
            {
                if (waitPoints.Length != 2)
                {
                    System.Array.Resize(ref waitPoints, 2);
                    //waitPointIterator = 0;
                    this.ChangeState(new PatrolState(this));
                }
            }
            else
            {
                waitPoints = temp;
            }
        }

        currentState.UpdateState();
        //Debug.DrawRay(this.transform.position, this.transform.forward * 6f, Color.yellow);    


        // Calculate distance between the player and the enemy
        playerDistance = Vector3.Distance(this.transform.position, player.transform.position);

        // If player in suspicious range is successful
        if (IsPlayerInSuspiciousRange())
        {
            switch (this.tag)
            {
                // If Agro then calculate suspicion
                case "Agro":                    
                    CalculateSuspicion();
                    break;
                // If Passive and player has recipe, then calculate suspicion
                case "Passive":
                    if (this.psScript.GetHasRecipe())
                    {
                        CalculateSuspicion();
                    }
                    break;
                default:
                    break;
            }
        }
        // Reset suspicion bar if player is behind any object
        else
        {
            currentSuspicionValue = Mathf.Clamp(currentSuspicionValue - Time.deltaTime * 1 / 10, 0, 1);                
        }
        UpdateSuspicionBar();

    }

    // Function to change the state
    public void ChangeState(State state)
    {
        if(this.name == "Waiter_2Chibi (2)")
        Debug.Log(currentState);
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
        if (Vector3.Distance(enemyHead.transform.position, playerHead.transform.position) <= maxSuspicionDistance)
        {
            return true;
        }
        return false;
    }

    // Function to check if player falls inside enemy's sight angle
    public bool IsPlayerInSightAngle()
    {
        if (Vector3.Angle(enemyHead.transform.forward, playerHead.transform.position - enemyHead.transform.position) <= sightAngle)
        {
            return true;
        }
        return false;
    }

    // Function to check if player is in current enemy's suspicion range
    public bool IsPlayerInSuspiciousRange()
    {
        int enemyToPlayerLayerMask = 1 << 10;
        enemyToPlayerLayerMask = ~enemyToPlayerLayerMask;
        RaycastHit hit;
        if (Physics.Raycast(enemyHead.transform.position, playerHead.transform.position - enemyHead.transform.position, out hit, Mathf.Infinity, enemyToPlayerLayerMask))
        {
            
            if (hit.collider.gameObject == playerHead && IsPlayerInSightDistance() && IsPlayerInSightAngle())
            {
                return true;
            }           
        }
        return false;
    }

    // Function to calculate suspicion value
    private void CalculateSuspicion()
    {
        // Detemine the formula of suspicion bar's increment
        if (currentState.GetName() != "Chase")
        {
            if (playerDistance <= minSuspicionDistance)
                currentSuspicionValue = 1;
            else
            {
                //y=log(-9/(k^2)*(x-k)^2+10)
                currentSuspicionValue = Mathf.Clamp(currentSuspicionValue + Time.deltaTime * IncrementSpeed
                    * (1 -
                    //y=log(-9/(k^2)*(x-k)^2+10)
                    Mathf.Log10(-9 / (maxSuspicionDistance * maxSuspicionDistance) * (playerDistance - maxSuspicionDistance) * (playerDistance - maxSuspicionDistance) + 10)), 0, 1);
            }           
        }
    }
    // Function to update suspicion bar
    private void UpdateSuspicionBar()
    {
        suspicionBar.SetSuspicion(currentSuspicionValue);
    }

    public float GetSuspicionValue() { return currentSuspicionValue; }
    public void ResetSuspicionValue() { currentSuspicionValue = 0; }

    // Getter and Updaters for waitPointIterator variable
    public int GetWaitPointIterator() { return waitPointIterator; }
    public void IncrementWaitPointIterator() { waitPointIterator++; }
    public void ResetWaitPointIterator() { waitPointIterator = 0; }

    // Function to check if raycast is possible from enemy's location to player's location
    /*    public bool IsRaycastToPlayerSuccess()
        {
            RaycastHit hit;
            if (Physics.Raycast(enemyHead.transform.position, playerHead.transform.position - enemyHead.transform.position, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == playerHead)
                {
                    return true;
                }
            }
            return false;
        }*/
    
}
