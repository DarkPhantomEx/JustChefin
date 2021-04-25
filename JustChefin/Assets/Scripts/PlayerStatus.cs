using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    //LIVES
    [SerializeField]
    int strikes;
    [SerializeField]
    bool isDead;
    [SerializeField]
    Image Strike;
    [SerializeField]
    Image Strike2;
    [SerializeField]
    Image Strike3;
    [SerializeField]
    Vector3 PlayerSpawn;
    [SerializeField]
    Transform Spawn;

    EditHUD hudEditor;
    TopDownMovement playerMove;

    // Boolean to check if player has the recipe collected
    [SerializeField]
    bool HasRecipe;

    CollectRecipe crScript;
    RecipeSystem RecipeManager;

    // Array of all the enemies in the scene (to be set in the editor)
    public EnemyAI[] enemy;

    public GameObject Sizzler;
    //Get Where sizzler is

    /*public GameObject[] AlarmLights;
    List<Animator> alarmAnimators = new List<Animator>();
    private Animator alarmAnimator;*/

    // Start is called before the first frame update
    void Start()
    {
        Spawn = GameObject.FindGameObjectWithTag("LevelSpawn").transform;
        PlayerSpawn = new Vector3(Spawn.position.x, transform.position.y, Spawn.position.z);
        playerMove = GameObject.FindGameObjectWithTag("MainPlayer").GetComponent<TopDownMovement>();
        hudEditor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditHUD>();
        if(SceneManager.GetActiveScene().name!="Test")
        crScript = GameObject.Find("SignatureRecipe").GetComponentInChildren<CollectRecipe>();
        RecipeManager = GameObject.Find("GameManager").GetComponent<RecipeSystem>();

        //alarmAnimator = GameObject.Find("FireAlarm").GetComponentInChildren<Animator>();

        //Disabling Strikes UI
        Strike.enabled = false;
        Strike2.enabled = false;
        Strike3.enabled = false;

        isDead = false;
        strikes = 3;

        // Player does not have the recipe initially
        SetHasRecipe(false);
    }

    void Update()
    {
        float SizzlerDistance;
        SizzlerDistance = Vector3.Distance(this.transform.position, Sizzler.transform.position);
        float MaxHearDistance = 30f;
        AudioManager.instance.Sizzling.setParameterByName("Distance", Mathf.Clamp(SizzlerDistance / MaxHearDistance, 0, 1));
       
    }

    public int getStrike()
    {
        return strikes;
    }

    //Reduces a life, and teleports player to spawn
    public void LoseLife()
    {
        
        hudEditor.setHUD("ObjC","They're suspicious! Get back to work.");
        strikes--;  //Life lost
        playerMove.SetCanMove(false); //Player movement is halted for a bit
        //Debug.Log(this.gameObject.transform.position);
        this.gameObject.transform.position = PlayerSpawn;
        //Debug.Log(this.gameObject.transform.position);

        /*StartCoroutine("AlarmTrigger");
        alarmAnimator.SetBool("IsFlashing", false);*/

        // Player loses collected recipe
        SetHasRecipe(false);
        if (SceneManager.GetActiveScene().name != "Test")
        {
            crScript.EnableSignatureRecipeMesh();
            crScript.startSignatureRecipeParticle();
        }        

        //UI update, based on lives lost
        switch (strikes)
        {
            //3 Strikes - Color: default -> red
            case 0: Strike.enabled = true;
                Strike.color = new Color32(229, 18, 18, 225);
                Strike2.color = new Color32(229, 18, 18, 225);
                Strike3.color = new Color32(229, 18, 18, 225);
                break;
            
            case 1:
                Strike2.enabled = true;
                break;

            case 2: Strike3.enabled = true;                      
                break;
        }

        //If life is 0 or below, player is dead.
        if(strikes <= 0)
        {
            isDead = true;
        }

        for(int i = 0; i < enemy.Length; i++)
        {
            // Reset suspicion bar back to minimum after being caught
            enemy[i].ResetSuspicionValue();
            // Reset every enemy AI's state to patrol after being aught
            enemy[i].ChangeState(new PatrolState(enemy[i]));          
           
        }
        //Since player lost a life, the recipe is restarted
        RecipeManager.ChooseRecipe(true);
        Invoke("DelaySetMove", 0.2f);
    }
    private void DelaySetMove()
    {
        playerMove.SetCanMove(true);
    }
    public bool isAlive()
    {
        return !isDead;
    }

    /*IEnumerator AlarmTrigger()
    {
        Debug.Log("Chaluuuuuuuuuuuuuuuuuuuuj");
        alarmAnimator.SetBool("IsFlashing", true);
        yield return new WaitForSeconds(3f);
        Debug.Log("juuuuuuuuuuuuuuuuuuuuulahc");
    }*/

    // Getter and Setter for signature recipe possession
    public bool GetHasRecipe() { return HasRecipe; }
    public void SetHasRecipe(bool HasRecipe) { this.HasRecipe = HasRecipe; }
}
