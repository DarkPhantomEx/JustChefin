using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManageScene : MonoBehaviour
{

    bool isPaused;

    [SerializeField]
    GameObject PauseUI;
    [SerializeField]
    GameObject EndUI;
    [SerializeField]
    GameObject NextLevel;

    EditHUD hudEditor;

    public int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadLevel(int levelIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelIndex);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitLevel()
    {
        Application.Quit();
    }

    public void EndScreen(int Header, string Body)
    {
        //Loss
        if(Header == 0)
        {
            //NextLevel.SetActive(false);
            hudEditor.setHUD("EndH", "Mission Failed!");
        }
        if (Header == 1)
        {
            //NextLevel.SetActive(true);
            hudEditor.setHUD("EndH", "Mission Successful!");
        }
        hudEditor.setHUD("EndB", Body);
        EndUI.SetActive(true);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        PauseUI = GameObject.FindGameObjectWithTag("PauseScreen");
        EndUI = GameObject.FindGameObjectWithTag("EndScreen");
        hudEditor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditHUD>();        
        PauseUI.SetActive(false);
        EndUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //If the Escape button is pressed, WHEN the player is not in a Trivia card
        if (Input.GetKeyDown(KeyCode.Escape) && !TriviaInteraction.isInTrivia)
        {
            AudioManager.instance.StopAllLoop();
            if (!isPaused)
            {
                isPaused = true;
                Time.timeScale = 0;
                PauseUI.SetActive(true);
                AudioManager.instance.pause.start();
            }
            else
            {
                isPaused = false;
                AudioManager.instance.pause.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                Time.timeScale = 1;
                PauseUI.SetActive(false);
            }



        }
    }
}
