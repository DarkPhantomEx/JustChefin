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

    // Start is called before the first frame update
    void Start()
    {
        hudEditor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditHUD>();
        triviaParticle = this.GetComponentInChildren<ParticleSystem>();
        TriviaCard.SetActive(false);

        isTimePaused = false;
        isInTrivia = false;
        sameIteration = false;
    }

    // Update is called once per frame
    void Update()
    {        
        if (GetCanCollectTrivia() && Input.GetKeyDown(KeyCode.E)) // And if E is pressed
        {
           //It is no longer the sameIteration as when this variable was set- time to make it false.
            if (sameIteration)
                sameIteration = false;

            if(!isTimePaused) //if Time is not paused
            {
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
            startTriviaParticle();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the collider belongs to the player
        if (other.tag == "MainPlayer")
        {
            // Player cannot collect recipe
            SetCanCollectTrivia(false);
            stopTriviaParticle();
        }
    }

    // Getter and Setter for ability to collect trivia card
    public bool GetCanCollectTrivia() { return canCollectTrivia; }
    public void SetCanCollectTrivia(bool canCollect) { this.canCollectTrivia = canCollect; }

    // Quick methods to play and stop particle effect
    public void startTriviaParticle() { triviaParticle.Play(); }
    public void stopTriviaParticle() { triviaParticle.Stop(); }
}
