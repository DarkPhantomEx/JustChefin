using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Assigns Cook Orders to the Player in the form of steps.
//Each step involves a timer
//Player must carry out the actions within the time provided

//Recipe 1 
/*
 * Boil Water - 30s
 * Simmer vegggies - 20s
 * Stir 10s
*/

//Script assigns recipe to player, starts timer based on the step assigned

public class RecipeSystem : MonoBehaviour
{
    [SerializeField]
    Timer recipeTimer;

    // Start is called before the first frame update
    void Start()
    {
        recipeTimer = this.gameObject.GetComponent<Timer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
