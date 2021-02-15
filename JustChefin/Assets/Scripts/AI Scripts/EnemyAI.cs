using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    State currentState;
    public NavMeshAgent nmAgent;

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
}
