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

    public int numInstr;
    public List<int> locID;    
    public List<string> Instr;    
    public List<int> timer;

    // Start is called before the first frame update
    void Start()
    {
        numInstr = 0;
        recipeTimer = this.gameObject.GetComponent<Timer>();
        
    }
    
    public void Save()
    {
        //Stores the List of recipes into recette.jc, by calling FileIO's save method
        FileIO.SaveRecipeData(this);
    }

    public void Load(RecipeData Recette)
    {
        //Loads the List of recipes into recette.jc, by calling FileIO's save method
        FileIO.LoadRecipeData(ref Recette);
        this.locID = new List<int>(Recette.locID);
        this.Instr = new List<string>(Recette.Instr);
        this.timer = new List<int>(Recette.timer);
        this.numInstr = Recette.num;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
