using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriviaInteraction : MonoBehaviour
{
    [SerializeField]
    bool canCollectTrivia;

    EditHUD hudEditor;
    ParticleSystem triviaParticle;

    // Start is called before the first frame update
    void Start()
    {
        hudEditor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditHUD>();
        triviaParticle = this.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetCanCollectTrivia()) // And if E is pressed
        {
            // Show the Trvia card UI
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the collider belongs to the player
        if (other.tag == "MainPlayer")
        {
            // Player can collect recipe
            SetCanCollectTrivia(true);
            hudEditor.setHUD("Obj", "Press 'E' to pick item!");
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
