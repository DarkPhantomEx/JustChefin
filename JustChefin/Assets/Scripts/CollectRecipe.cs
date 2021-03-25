 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectRecipe : MonoBehaviour
{
    // Reference to PlayerStatus script to be used for handling player recipe possession
    PlayerStatus psScript;
    RecipeSystem Recipe;
    // Start is called before the first frame update
    void Start()
    {
        psScript = GameObject.Find("TopDownPlayer").GetComponent<PlayerStatus>();
        Recipe = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RecipeSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the collider belongs to the player
        if(other.tag == "MainPlayer")
        {
            // Player can collect recipe
            psScript.SetCanCollect(true);
            Recipe.setObjective("Press 'E' to steal the recipe!");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the collider belongs to the player
        if (other.tag == "MainPlayer")
        {
            // Player cannot collect recipe
            psScript.SetCanCollect(false);
        }
    }
}
