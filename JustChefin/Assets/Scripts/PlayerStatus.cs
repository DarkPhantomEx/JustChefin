using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    bool HasRecipe;
    [SerializeField]
    bool canCollect;

    public EnemyAI[] enemy;

    // Start is called before the first frame update
    void Start()
    {
        Spawn = GameObject.FindGameObjectWithTag("LevelSpawn").transform;
        PlayerSpawn = new Vector3(Spawn.position.x, transform.position.y, Spawn.position.z);
        
        //Disabling Strikes UI
        Strike.enabled = false;
        Strike2.enabled = false;
        Strike3.enabled = false;

        isDead = false;
        strikes = 3;

        HasRecipe = false;
    }


    public int getStrike()
    {
        return strikes;
    }

    public void LoseLife()
    {
        //Reduces a life, and teleports player to spawn
        GameObject.FindGameObjectWithTag("Objective").GetComponent<Text>().text = "They're suspicious! Get back to work.";
        strikes--;
        this.gameObject.transform.position = PlayerSpawn;
        SetHasRecipe(false);

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

        if(strikes <= 0)
        {
            isDead = true;
        }

        for(int i = 0; i < enemy.Length; i++)
        {
            enemy[i].ChangeState(new PatrolState(enemy[i]));
        }
    }

    public bool isAlive()
    {
        return !isDead;
    }
    // Update is called once per frame
    void Update()
    {
        if(GetCanCollect() && Input.GetKeyDown(KeyCode.E))
        {
            SetHasRecipe(true);
        }
    }

    public bool GetHasRecipe()
    {
        return HasRecipe;
    }

    public void SetHasRecipe(bool HasRecipe)
    {
        this.HasRecipe = HasRecipe;
    }

    public bool GetCanCollect()
    {
        return canCollect;
    }

    public void SetCanCollect(bool canCollect)
    {
        this.canCollect = canCollect;
    }
}
