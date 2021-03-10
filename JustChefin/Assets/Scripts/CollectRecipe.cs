 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectRecipe : MonoBehaviour
{
    PlayerStatus psScript;

    // Start is called before the first frame update
    void Start()
    {
        psScript = GameObject.Find("TopDownPlayer").GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "MainPlayer" && Input.GetKeyDown(KeyCode.E))
        {
            psScript.SetRecipeBool(true);
        }
    }
}
