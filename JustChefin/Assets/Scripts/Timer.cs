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
    //[SerializeField]
    //private Text timerDisp;

    [SerializeField]
    private float time;

    bool isCountingDown;

    //UI attributes
    //Slider for the TimerBar
    [SerializeField]
    Slider timerSlider;
    [SerializeField]
    Gradient timerBarGrad;
    [SerializeField]
    Image FillColor;
    [SerializeField]
    float lerpSpeed;

    EditHUD hudEditor;

    //Defines the max value for the slider when starting it up
    public void defSlider(int max)
    {
        //Slider goes from 0 to max value
        timerSlider.minValue = 0;
        timerSlider.maxValue = max;
        //Slider's value is set to max, to avoid an initial lerp from 0-max
        timerSlider.value = max;
        
    }

    //Updates current value of slider
    public void setSlider(int val)
    {
        //sets the color of the timer bar based on the time left
        FillColor.color = timerBarGrad.Evaluate(timerSlider.normalizedValue);
        //lerps the value of the timer from the previous second to next
        timerSlider.value = Mathf.Lerp(timerSlider.value, val, Time.deltaTime * lerpSpeed);
    }


    // Start is called before the first frame update
    void Start()
    {
        hudEditor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditHUD>();
        timerSlider = GameObject.FindGameObjectWithTag("TimerBar").GetComponent<Slider>();
        //timerDisp = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        isCountingDown = false;
    }

    //Starts the timer with the provided numebr of seconds.
    public void StartTimer(int time)
    {
        //Set's the slider's max value based on the time required for the current instruction
        defSlider(time);
        isCountingDown = true;
        this.time = time;
    }

    public void StopTimer()
    {
        //slider is set to 0
        setSlider(0);
        isCountingDown = false;
    }

    public float GetTime()
    {
        return time;
    }

    //public string GetTimer()
    //{
    //    return timerDisp.text;
    //}

    public bool GetTimerState()
    {
        return isCountingDown;
    }
    // Update is called once per frame
    void Update()
    {
        //    if(isCountingDown && time<= 7.0f)
        //    {
        //        GameObject.FindGameObjectWithTag("Objective").GetComponent<Text>().text = "Get back to the kitchen, before time runs out!";
        //    }

        if (isCountingDown && time > 0.0f)
        {
            //Converts time to mm:ss format if counting down.
            time -= Time.deltaTime;
            setSlider((int)time);
            //Debug.Log("Set timeSlider to " + time);
            var min = (int)time / 60;
            var sec = (int)time % 60;
            hudEditor.setHUD("Tim", string.Format("{0:00} : {1:00}", min, sec));
        }

        if (isCountingDown && time <= 0.0f)
        {
            StopTimer();
            //If the player is not in the kitchen
            if (this.gameObject.GetComponent<RecipeSystem>().inKitchen == false)
            {
                //Player loses a life
                GameObject.FindGameObjectWithTag("MainPlayer").GetComponent<PlayerStatus>().LoseLife();
                //Kitchen Door trigger is false, so as to prevent player from passing through it
                GameObject.FindGameObjectWithTag("KitchenDoor").GetComponent<Collider>().isTrigger = false;
            }
        }
    }
}

