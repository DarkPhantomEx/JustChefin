using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InLocation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("MainPlayer"))
        {
            Debug.Log(collider.name + "has entered the Cooking Area");
            GameObject.FindWithTag("GameManager").GetComponent<RecipeSystem>().canCook = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("MainPlayer"))
        {
            Debug.Log(collider.name + "has exited the Cooking Area");
            GameObject.FindWithTag("GameManager").GetComponent<RecipeSystem>().canCook = false;
        }
    }
}
