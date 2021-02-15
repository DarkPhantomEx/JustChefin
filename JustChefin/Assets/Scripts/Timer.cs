using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
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
        time = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0.0f) //If the timer is above 0, it counts down
            isCountingDown = true;

        if ((isCountingDown) && (time == 0.0f)) //if the timer is 0, and it was counting down, stop countdown
            isCountingDown = false;

        if (time == 0.0f) //Stops timer at 0.0
            isCountingDown = false;
        
        time -= Time.deltaTime;

        var min = (int)time / 60;
        var sec = (int)time % 60;  

        timerDisp.text = string.Format("{0:00} : {1:00}", min, sec);
    }
}
