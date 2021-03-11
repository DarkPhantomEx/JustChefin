using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroWarning : MonoBehaviour
{

    RecipeSystem Recipe;
    // Start is called before the first frame update
    void Start()
    {
        Recipe = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RecipeSystem>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "MainPlayer")
        {
            Debug.Log("AGRO");
            Recipe.setObjective("Watch out! The manager doesn't like uninvited guests.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
