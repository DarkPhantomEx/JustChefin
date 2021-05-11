using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriviaInteraction : MonoBehaviour
{
    [SerializeField]
    bool canCollectTrivia;
    [SerializeField]
    bool isTimePaused;
    
    //For Trivia Card closing/opening purposes - helped bypass an issue with using the same key.
    [SerializeField]
    bool sameIteration;

    //To keep track of if the player is in a Trivia Card
    [SerializeField]
    public static bool isInTrivia;    

    EditHUD hudEditor;
    ParticleSystem triviaParticle;

    //GameObject that contains the trivia card corresponding to this object
    [SerializeField]
    GameObject TriviaCard;

    // Used for Raycasting and ParticleSystem emission
    private GameObject playerHead;
    ParticleSystem.EmissionModule triviaParticleEmission;
    public float triggerDistance;

    // Start is called before the first frame update
    void Start()
    {
        playerHead = GameObject.Find("/Iris/Head");
        hudEditor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditHUD>();
        triviaParticle = this.GetComponent<ParticleSystem>();
        TriviaCard.SetActive(false);

        isTimePaused = false;
        isInTrivia = false;
        sameIteration = false;

        // Used to control emission state of the particle system
        triviaParticleEmission = triviaParticle.emission;
    }

    // Update is called once per frame
    void Update()
    {
        if (RaycastToPlayer() && Vector3.Distance(this.transform.position, playerHead.transform.position) <= triggerDistance)
            triviaParticleEmission.enabled = true;
        else
            triviaParticleEmission.enabled = false;

        if (GetCanCollectTrivia() && Input.GetKeyDown(KeyCode.E)) // And if E is pressed
        {
            AudioManager.instance.StopLoop('W');//Stop Walking SFX
            AudioManager.instance.pause.start();
           //It is no longer the sameIteration as when this variable was set- time to make it false.
            if (sameIteration)
                sameIteration = false;

            if(!isTimePaused) //if Time is not paused
            {
                AudioManager.instance.Looting.start();
                isInTrivia = true;
                isTimePaused = true;
                //Time is paused, so that the game isn't running while the Trivia card is up
                Time.timeScale = 0f;
                // Show the Trvia card UI
                TriviaCard.SetActive(true);
                //A bool to check if Update is in the current iteration - when closing
                sameIteration = true;
            }

            //If Time is Paused, that means you're in a Trivia moment.
            if (isTimePaused && !sameIteration)
            {
                //Triva Card is hidden
                TriviaCard.SetActive(false);
                //Time is returned to the usual
                isTimePaused = false;
                isInTrivia = false;
                Time.timeScale = 1;
                AudioManager.instance.pause.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the collider belongs to the player
        if (other.tag == "MainPlayer")
        {
            // Player can collect recipe
            SetCanCollectTrivia(true);
            hudEditor.setHUD("ObjC", "Press 'E' to examine!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the collider belongs to the player
        if (other.tag == "MainPlayer")
        {
            // Player cannot collect recipe
            SetCanCollectTrivia(false);
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

    // Getter and Setter for ability to collect trivia card
    public bool GetCanCollectTrivia() { return canCollectTrivia; }
    public void SetCanCollectTrivia(bool canCollect) { this.canCollectTrivia = canCollect; }
}
