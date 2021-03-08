using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    //CookingLocation Colliders
    GameObject CookingStation;
    [SerializeField]
    GameObject KitchenDoor;
    //TextBoxes
    public Text Step;
    public Text RecipeName;
    [SerializeField]
    Text Objective;
    //public Text Step1;
    //public Text Step2;
    //public Text Step3;
    
    //Timer Script object
    [SerializeField]
    public Timer recipeTimer;

    //Recipe Data: Num of Instructions, Location ID, Steps, Timer for each step, Total number of recipes
    public List<int> numInstr;
    public List<int> locID;    
    public List<string> Instr;    
    public List<int> timer;
    public List<string> recName;
    public int numRec;

    //Variable to keep track of the step currently being worked on
    [SerializeField]
    private int currInstr;
    [SerializeField]
    private int currRec;
    [SerializeField]
    int currRecStart;
    

    ////for randomizing steps
    //public string[] verbs;
    //public string[] food;

    //Serializable data class object for FileIO
    RecipeData Recipe;
    //Bool to check if a save file exists 
    public bool FileStatus;
    //Bool to check if player is in a cookingStation area - and hence can cook
    public bool canCook;
    //Bool to check if cooking is currently taking place
    public bool isCooking;
    //Bool to check if this is the first cooking interaction
    public bool firstCook;

    // Start is called before the first frame update
    void Start()
    {
        Initialize(); //Loads/Creates File and instantiates pertinent data.
        StartGame();
        
    }
    
    public void Initialize()
    {
        //Debug.Log(Application.persistentDataPath + " lol");
        currInstr = 0;
        currRec = 0;
        currRecStart = 0;
        canCook = false;
        isCooking = false;

        //Attaches Timer Script component to object.
        recipeTimer = this.gameObject.GetComponent<Timer>();
        
        //Attaches EmptyCollider GameObject
        CookingStation = GameObject.FindGameObjectWithTag("CookingStation");
        //Attaches KitchenDoor collider, and makes it so that the player can't leave the kitchen.
        KitchenDoor = GameObject.FindGameObjectWithTag("KitchenDoor");
        KitchenDoor.GetComponent<Collider>().isTrigger = false;

        Objective.text = "Get to your station!";

        //If Game is launched for the first time
        if (FileStatus = FileIO.DoesRecipeFileExist())
        {
            Load(Recipe);
        }
        else
        {
            //if NewGame, initializes number of Recipes to zero, and scripts new recipes. This data is saved into "recette.jc"
            numRec = 0;

            //Scripts Level Data based on the Levelnumber
            ScriptRecipeData(SceneManager.GetActiveScene().buildIndex);

            //Saves RecipeData to file, and keeps a copy of RecipeData in "Recipe"
            Recipe = Save();
        }
    }

    //This method is used to create recipes for each level. Might move this to FileIO.cs
    public void ScriptRecipeData(int levelNumber)
    {
        switch (levelNumber+1)
        {
            case 1:
                //Recipe 1 - Chicken Burger
                recName.Add("Big Max Burger\n");
                Instr.Add("Step 1: Sautee Onions - 40s\n");
                timer.Add(40);
                locID.Add(0);
                Instr.Add("Step 2: Grill chicken patty - 2min\n");
                timer.Add(120);
                locID.Add(0);
                Instr.Add("Step 3: Heat Buns - 20s\n");
                timer.Add(20);
                locID.Add(0);
                    //num of Steps = 3
                numInstr.Add(3);
                //Recipe 2 - Veg Burger
                recName.Add("Gaia's Bounty Burger\n");
                Instr.Add("Step 1: Sautee Onions - 40s\n");
                timer.Add(40);
                locID.Add(0);
                Instr.Add("Step 2: Prepare veg patty- 1min 40s\n");
                timer.Add(100);
                locID.Add(0);
                Instr.Add("Step 3: Heat Buns- 20s\n");
                timer.Add(20);
                locID.Add(0);
                    //num of Steps = 3
                numInstr.Add(3);
                    //Total num of Recipes = 2
                numRec = 2;
                //Recipes End
                break;

            default: Debug.LogError("Something went wrong. This is not scene you're looking for, bub.");
                break;             
        }

        ///*If we ever need Randomized recipes*/
        //verbs = new string[3] { "boil", "heat", "cut" };
        //food = new string[3] { "water", "vegetables", "meat" };
        //locID.Add(Random.Range(0, 5));
        //Instr.Add(verbs[Random.Range(0, 3)] + food[Random.Range(0, 3)]);
        //timer.Add(20 + Random.Range(0, 21));

    }

    void StartGame()
    {

        ChooseRecipe();

    }

    // Update is called once per frame
    void Update()
    {
        //If the timer isn't counting down, that means the player isn't coooking
        if (!recipeTimer.GetTimerState())
        {
            isCooking = false;
        }
        
        if(!isCooking && canCook)
        {
            Objective.text = "Press 'Q' to Cook!";
        }

        //If the Player is at a Cooking Station, and the timer isn't counting down, by pressing Q they start the timer
        if(canCook && !isCooking && Input.GetKeyDown(KeyCode.Q))
        {
            Objective.text = "Good Job! You can now search the Restaurant for the Hidden Recipe";

            //Player can leave the kitchen
            KitchenDoor.GetComponent<Collider>().isTrigger = true;

            //IF the Recipe is complete, Select the next one
            if (currInstr > numInstr[currRec] - 1)
            {
                Debug.Log("Time for the next recipe!");
                ChooseRecipe();
            }
            //Start the timer for the instruction
            recipeTimer.StartTimer(timer[currRecStart + currInstr]);
            isCooking = true;
            currInstr++;
                //recipeTimer.StartTimer(timer[currRecStart + currInstr]);
        }      
       

    }

    //Loads Next Recipe
    void ChooseRecipe()
    {
        currInstr = 0;
        //Randomly chooses recipe out of list
        currRec = Random.Range(0, numRec);
        Debug.Log("The chosen recipe is Recipe# " + currRec);

        //Finds the Starting point of current recipe's instructions
        GetCurrRecStart();
        //Sets RecipeUI with the current recipe
        Step.text = null;
        RecipeName.text = recName[currRec];
        
        for (int i = 0; i < numInstr[currRec]; i++)
        {
            Step.text += Instr[currRecStart + i];
        }
        //Starts Timer
            //recipeTimer.StartTimer(timer[currRecStart]);
    }

    //Finds the Starting point of the instructions for the current recipe, by adding the num of instructions of the previous ones
    public void GetCurrRecStart()
    {
        currRecStart = 0;
       //The Starting index of the current recipe, by adding the num of instructions of the previous ones
        for (int i = 0; i < currRec; i++)
        {
            currRecStart += numInstr[i];
        }
        ////The end of the recipe is starting index + number of steps - 1
        //currRecEnd = currRecStart + numInstr[currRecStart] - 1;
    }
    
    

    public RecipeData Save()
    {
        //Stores the List of recipes into recette.jc, by calling FileIO's save method
        return FileIO.SaveRecipeData(this);        
    }

    public void Load(RecipeData Recette)
    {
        //Loads the List of recipes into recette.jc, by calling FileIO's save method
        FileIO.LoadRecipeData(ref Recette);
        this.locID = new List<int>(Recette.locID);
        this.Instr = new List<string>(Recette.Instr);
        this.timer = new List<int>(Recette.timer);
        this.numInstr = new List<int>(Recette.numInstr);
        this.numRec = Recette.numRec;
        this.recName = new List<string>(Recette.recName);
    }


   
}
