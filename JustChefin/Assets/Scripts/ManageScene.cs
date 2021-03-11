using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScene : MonoBehaviour
{

    bool isPaused;

    [SerializeField]
    GameObject PauseUI;

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

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        PauseUI = GameObject.FindGameObjectWithTag("PauseScreen");
        PauseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                isPaused = true;
                Time.timeScale = 0;
                PauseUI.SetActive(true);
            }
            else
            {
                isPaused = false;
                Time.timeScale = 1;
                PauseUI.SetActive(false);
            }



        }
    }
}
