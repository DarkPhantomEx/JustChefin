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
    [SerializeField]
    GameObject WinCon;
    [SerializeField]
    GameObject LoseCon;

    EditHUD hudEditor;
    ManageScene sceneManager;
        
    //Timer Script object
    [SerializeField]
    public Timer recipeTimer;

    //Recipe Data: Num of Instructions, Location ID, Steps, Timer for each step, Total number of recipes
    public List<int> numInstr;
    public List<int> locID;    
    public List<string> Instr;    
    public List<string> ingInstr;    
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

    PlayerStatus playerStats;
    //Serializable data class object for FileIO
    RecipeData Recipe;
    //Bool to check if a save file exists 
    public bool FileStatus;
    //Bool to check if player is in a cookingStation area - and hence can cook
    public bool canCook;
    //Bool to check if cooking is currently taking place
    public bool isCooking;
    //Bool to check if player is in the kitchen
    public bool inKitchen;

    public bool isEndScreenOpen;

    //Bool to check if this is the first cooking interaction
    //public bool firstCook;
    int cookNo;


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
        cookNo = 0;
        inKitchen = false;
        isEndScreenOpen = false;

        //Attaches Timer Script component to object.
        recipeTimer = this.gameObject.GetComponent<Timer>();
        hudEditor = this.gameObject.GetComponent<EditHUD>();
        sceneManager = this.gameObject.GetComponent<ManageScene>();

        //Attaches EmptyCollider GameObject
        CookingStation = GameObject.FindGameObjectWithTag("CookingStation");
        //Attaches KitchenDoor collider, and makes it so that the player can't leave the kitchen.
        KitchenDoor = GameObject.FindGameObjectWithTag("KitchenDoor");
        KitchenDoor.GetComponent<Collider>().isTrigger = false;
        //Gets access to PlayerStatus from the TopDownPlayer gameobject
        playerStats = GameObject.FindGameObjectWithTag("MainPlayer").GetComponent<PlayerStatus>();

        hudEditor.setHUD("ObjC", "Get to your station!");

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
        //level number should correspond to the scene number
        switch (levelNumber)
        {
            case 1:
                //Recipe 1 - Chicken Burger
                recName.Add("Big Max Burger\n");
                Instr.Add("Sautee Onions - 15s\n");
                ingInstr.Add("Sauteeing Onions - 15s\n");
                timer.Add(15);
                locID.Add(0);
                Instr.Add("Grill chicken patty - 1min\n");
                ingInstr.Add("Grilling chicken patty - 1min\n");
                timer.Add(60);
                locID.Add(0);
                Instr.Add("Heat Buns - 20s\n");
                ingInstr.Add("Heating Buns - 20s\n");
                timer.Add(20);
                locID.Add(0);
                    //num of Steps = 3
                numInstr.Add(3);
                //Recipe 2 - Veg Burger
                recName.Add("Gaia's Bounty Burger\n");
                Instr.Add("Sautee Onions - 15s\n");
                ingInstr.Add("Sauteeing Onions - 15s\n");
                timer.Add(15);
                locID.Add(0);
                Instr.Add("Prepare veg patty- 1min 10s\n");
                ingInstr.Add("Preparing veg patty- 1min 10s\n");
                timer.Add(70);
                locID.Add(0);
                Instr.Add("Heat Buns- 20s\n");
                ingInstr.Add("Heating Buns- 20s\n");
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

    }

    void StartGame()
    {

        ChooseRecipe(false);

    }

    // Update is called once per frame
    void Update()
    {
        //If the timer isn't counting down, that means the player isn't coooking
        if (!recipeTimer.GetTimerState() && cookNo > 0)
        {
            AudioManager.instance.Sizzling.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);   //Sizzling SFX Stop
            isCooking = false;
            //Subtracting by 1, as currInstr was incremented when cooking started
            if(currInstr >= numInstr[currRec])
            {
                hudEditor.setHUD("Ins","Order fulfilled.\nYour next order awaits!");
            }
            else
            {
                hudEditor.setHUD("Ins", Instr[currRecStart + currInstr]);
            }
        }
        
        if(!isCooking && canCook)
        {
            hudEditor.setHUD("Obj","Press 'E' to Cook!");            
        }

        
        //If the Player is at a Cooking Station, and the timer isn't counting down, by pressing E they start the timer
        if(canCook && !isCooking && Input.GetKeyDown(KeyCode.E))
        {
            cookNo++;
            hudEditor.setHUD("ObjC", "Good Job! You can now search the Restaurant for the Hidden Recipe");
            //hudEditor.setHUD("Ins", ingInstr[currRec + currInstr]);

            //Player can leave the kitchen
            KitchenDoor.GetComponent<Collider>().isTrigger = true;

            //IF the Recipe is complete, Select the next one
            if (currInstr > numInstr[currRec] - 1)
            {
                Debug.Log("Time for the next recipe!");
                ChooseRecipe(false);
            }
            //Start the timer for the instruction
            recipeTimer.StartTimer(timer[currRecStart + currInstr]);
            hudEditor.setHUD("Ins", ingInstr[currRecStart + currInstr]);
            isCooking = true;
            currInstr++;
            AudioManager.instance.Sizzling.start();                       //Sizzling SFX Start
                //recipeTimer.StartTimer(timer[currRecStart + currInstr]);
        }
               

        //CheckLoseCondition - Player is Dead
        if (!playerStats.isAlive())
        {
            if (isEndScreenOpen == false)
            {
                isEndScreenOpen = true;

                sceneManager.EndScreen(0, "You were considered suspicious, and were fired. Try again, Agent Iris!");
                Time.timeScale = 0f;
                
            }
        }

        //CheckWinCondition - Player Has Recipe and is back at the kitchen
        if(playerStats.GetHasRecipe() && canCook)
        {
            if (isEndScreenOpen == false)
            {
                isEndScreenOpen = true; 

                sceneManager.EndScreen(1, "Congratulations on a mission well done, Agent Iris!");
                Time.timeScale = 0f;
            }
        }
        //If the timer is 5s or lower, print message
        if(recipeTimer.GetTimerState() && recipeTimer.GetTime() <=5)
        {
            hudEditor.setHUD("Obj", "Get back to your cooking station!");
        }

    }

    //Loads Next Recipe, if bool is true, print recipe burnt
    public void ChooseRecipe(bool lifeLost)
    {
        if (lifeLost)
        {
            //Enter code to deal with burnt food
        }

        currInstr = 0;
        //Randomly chooses recipe out of list
        currRec = Random.Range(0, numRec);
        Debug.Log("The chosen recipe is Recipe# " + currRec);

        //Finds the Starting point of current recipe's instructions
        GetCurrRecStart();
        //Sets RecipeUI with the current recipe
        hudEditor.setHUD("Ins", null);
        hudEditor.setHUD("Rec", recName[currRec]);

        hudEditor.setHUD("Ins", Instr[currRecStart]);
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
        this.ingInstr = new List<string>(Recette.ingInstr);
        this.timer = new List<int>(Recette.timer);
        this.numInstr = new List<int>(Recette.numInstr);
        this.numRec = Recette.numRec;
        this.recName = new List<string>(Recette.recName);
    }

}
