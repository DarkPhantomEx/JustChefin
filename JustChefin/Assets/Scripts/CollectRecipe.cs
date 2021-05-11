 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script that handles collection/stealing of the signature recipe collectible
 */

public class CollectRecipe : MonoBehaviour
{
    // Reference to PlayerStatus script to be used for handling player recipe possession
    PlayerStatus psScript;
    EditHUD hudEditor;
    GameObject signatureRecipeRenderer;
    ParticleSystem recipeParticle;

    // Boolean to check if player is standing on the recipe collect trigger
    [SerializeField]
    bool canCollect;

    // Used for Raycasting and ParticleSystem emission
    private GameObject playerHead;
    private ParticleSystem.EmissionModule recipeParticleEmission;

    // Start is called before the first frame update
    void Start()
    {
        playerHead = GameObject.Find("/Iris/Head");
        psScript = GameObject.Find("Iris").GetComponent<PlayerStatus>();
        hudEditor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditHUD>();
        signatureRecipeRenderer = GameObject.Find("SignatureRecipe");
        recipeParticle = GameObject.Find("SignatureRecipe").GetComponentInChildren<ParticleSystem>();

        // Used to control emission state of the particle system
        recipeParticleEmission = recipeParticle.emission;
    }

    // Update is called once per frame
    void Update()
    {
        if (RaycastToPlayer() && Vector3.Distance(this.transform.position, playerHead.transform.position) <= 9)
            recipeParticleEmission.enabled = true;
        else
            recipeParticleEmission.enabled = false;

        if (GetCanCollect() && Input.GetKeyDown(KeyCode.E))
        {
            psScript.SetHasRecipe(true);
            hudEditor.setHUD("ObjC", "You've got the recipe! Get back to your station ASAP.");
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

    private bool RaycastToPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, playerHead.transform.position - this.transform.position, out hit, Mathf.Infinity))
        {
            //Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "MainPlayerHead")
                return true;
        }
        return false;
    }

    // Getter and Setter for ability to collect recipe
    public bool GetCanCollect() { return canCollect; }
    public void SetCanCollect(bool canCollect) { this.canCollect = canCollect; }

    // Quick methods to enable and disable mesh of the signature recipe
    public void EnableSignatureRecipeMesh() { signatureRecipeRenderer.SetActive(true); }
    public void DisableSignatureRecipeMesh() { signatureRecipeRenderer.SetActive(false); }

    // Quick methods to play and stop particle effect
    public void startSignatureRecipeParticle() { recipeParticle.Play(); }
    public void stopSignatureRecipeParticle() { recipeParticle.Stop(); }
}
