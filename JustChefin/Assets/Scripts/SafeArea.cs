using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("MainPlayer"))
        {
            Debug.Log(collider.name + "has entered the Kitchen Area");
            GameObject.FindWithTag("GameManager").GetComponent<RecipeSystem>().inKitchen = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("MainPlayer"))
        {
            Debug.Log(collider.name + "has exited the Cooking Area");
            GameObject.FindWithTag("GameManager").GetComponent<RecipeSystem>().inKitchen = false;
        }
    }
}

