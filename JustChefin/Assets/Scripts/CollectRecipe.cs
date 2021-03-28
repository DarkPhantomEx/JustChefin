 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectRecipe : MonoBehaviour
{
    // Reference to PlayerStatus script to be used for handling player recipe possession
    PlayerStatus psScript;
    EditHUD hudEditor;
    Renderer signatureRecipeRenderer;
    ParticleSystem recipeParticle;

    // Boolean to check if player is standing on the recipe collect trigger
    [SerializeField]
    bool canCollect;

    // Start is called before the first frame update
    void Start()
    {
        psScript = GameObject.Find("TopDownPlayer").GetComponent<PlayerStatus>();
        hudEditor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditHUD>();
        signatureRecipeRenderer = GameObject.Find("SignatureRecipe").GetComponent<Renderer>();
        recipeParticle = GameObject.Find("SignatureRecipe").GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetCanCollect() && Input.GetKeyDown(KeyCode.E))
        {
            psScript.SetHasRecipe(true);
            hudEditor.setHUD("Obj", "You've got the recipe! Get back to your station ASAP.");
            DisableSignatureRecipeMesh();
            stopSignatureRecipeParticle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the collider belongs to the player
        if(other.tag == "MainPlayer")
        {
            // Player can collect recipe
            SetCanCollect(true);
            hudEditor.setHUD("Obj", "Press 'E' to steal the recipe!");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the collider belongs to the player
        if (other.tag == "MainPlayer")
        {
            // Player cannot collect recipe
            SetCanCollect(false);
        }
    }

    // Getter and Setter for ability to collect recipe
    public bool GetCanCollect() { return canCollect; }
    public void SetCanCollect(bool canCollect) { this.canCollect = canCollect; }

    public void EnableSignatureRecipeMesh() { signatureRecipeRenderer.enabled = true; }
    public void DisableSignatureRecipeMesh() { signatureRecipeRenderer.enabled = false; }

    public void startSignatureRecipeParticle() { recipeParticle.Play(); }

    public void stopSignatureRecipeParticle() { recipeParticle.Stop(); }
}
