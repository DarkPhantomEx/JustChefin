using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterUnique : MonoBehaviour
{
    public EnemyAI waiter;
    public GameObject[] waitPoints;
    private GameObject[] temp;
    private PlayerStatus psScript;
 
    // Start is called before the first frame update
    void Start()
    {
        psScript = GameObject.Find("TopDownPlayer").GetComponent<PlayerStatus>();
        temp = waiter.waitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (psScript.GetHasRecipe() && waiter.waitPoints != waitPoints)
        {
            waiter.waitPoints = waitPoints;
            waiter.ResetWaitPointIterator();
            waiter.ChangeState(new PatrolState(waiter));            
        }
        else if(!psScript.GetHasRecipe() && waiter.waitPoints != temp)
        {
            waiter.waitPoints = temp;
            waiter.ResetWaitPointIterator();
            waiter.ChangeState(new PatrolState(waiter));
        }
    }
}
