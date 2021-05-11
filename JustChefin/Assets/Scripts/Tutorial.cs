using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject GameManager, Player, Poster1, Poster2, Poster3;
    private RecipeSystem RecipeSystem;
    private PlayerStatus psScript;
    private TopDownMovement playerMove;
    public Text instruction;
    private bool firsttime = true;
  
    // Start is called before the first frame update
    void Start()
    {
        RecipeSystem = GameManager.GetComponent<RecipeSystem>();
        psScript = GameObject.Find("Iris").GetComponent<PlayerStatus>();
        playerMove = GameObject.FindGameObjectWithTag("MainPlayer").GetComponent<TopDownMovement>();
        playerMove.SetCanMove(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (instruction.text == "If you don't return on time, the dish burns.\nYou reputation lowers (lose a Life) and must start a new dish\n" && firsttime)
        {
            Player.transform.position = new Vector3(-272f, 0.75f, 251f);
            firsttime = false;
        }
        if (playerMove.GetCanMove())
        {
            Poster3.SetActive(true);
        }
        if (instruction.text == "Now go out and steal the signature recipe!\n")
        {
            Invoke("DelaySetMove", 0.2f);
        }
        if (psScript.GetHasRecipe())
        {
            Poster1.SetActive(true);
            Poster2.SetActive(true);
        }
    }
}