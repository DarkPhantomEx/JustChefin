using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroWarning : MonoBehaviour
{

    EditHUD hudEditor;
    // Start is called before the first frame update
    void Start()
    {
        hudEditor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditHUD>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "MainPlayer")
        {
            Debug.Log("AGRO");
            hudEditor.setHUD("Obj","Watch out! The manager doesn't like uninvited guests.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
