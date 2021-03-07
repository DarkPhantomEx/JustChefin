using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    /* timerDisp - TextBox to display the timer
     * time - time in seconds
     * isCountingDown - true if timer is active
     */
    [SerializeField]
    private Text timerDisp;
    [SerializeField]
    private float time;

    bool isCountingDown;
    
    
    // Start is called before the first frame update
    void Start()
    {
        timerDisp = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        isCountingDown = false;
    }

    public void StartTimer(int time)
    {
        isCountingDown = true;
        this.time = time;
    }

    public void StopTimer()
    {
        isCountingDown = false;
    }

    public string GetTimer()
    {
        return timerDisp.text;
    }

    public bool GetTimerState()
    {
        return isCountingDown;
    }
    // Update is called once per frame
    void Update()
    {
        if (isCountingDown && time > 0.0f)
        {
            //Converts time to mm:ss format if counting down.
            time -= Time.deltaTime;
            var min = (int)time / 60;
            var sec = (int)time % 60;
            timerDisp.text = string.Format("{0:00} : {1:00}", min, sec);
        }

        if(isCountingDown && time <= 0.0f)
        {
            StopTimer();
            if(this.gameObject.GetComponent<RecipeSystem>().canCook == false)
            {
                GameObject.FindGameObjectWithTag("MainPlayer").GetComponent<PlayerStatus>().LoseLife();
            }
        }
    }
}

