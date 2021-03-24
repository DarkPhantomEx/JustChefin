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

    EditHUD hudEditor;

    //[SerializeField]
    //private Text timerDisp;
    [SerializeField]
    private float time;
        
    bool isCountingDown;

    //UI attributes
    //Slider for the TimerBar
    Slider timerSlider;
    [SerializeField]
    Gradient timerBarGrad;
    [SerializeField]
    Image FillColor;
    [SerializeField]
    float lerpSpeed;

    //Defines the max value for the slider when starting it up
    public void defSlider(int max)
    {     
        timerSlider.minValue = 0;
        timerSlider.value = max;
        timerSlider.maxValue = max;
    }

    //Updates current value of slider
    public void setSlider(int val)
    {
        FillColor.color =  timerBarGrad.Evaluate(timerSlider.normalizedValue);
        timerSlider.value = Mathf.Lerp(timerSlider.value,val,Time.deltaTime * lerpSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        hudEditor =GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditHUD>();
        timerSlider = GameObject.FindGameObjectWithTag("TimerBar").GetComponent<Slider>();
        //timerDisp = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        isCountingDown = false;
    }

    public void StartTimer(int time)
    {
        defSlider(time);
        isCountingDown = true;
        this.time = time;
    }

    public void StopTimer()
    {
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
            var min = (int)time / 60;
            var sec = (int)time % 60;
            //timerDisp.text = string.Format("{0:00} : {1:00}", min, sec);
            hudEditor.setHUD("Tim",string.Format("{0:00} : {1:00}", min, sec));
        }

        if(isCountingDown && time <= 0.0f)
        {
            StopTimer();
            if(this.gameObject.GetComponent<RecipeSystem>().canCook == false)
            {
                GameObject.FindGameObjectWithTag("MainPlayer").GetComponent<PlayerStatus>().LoseLife();
                GameObject.FindGameObjectWithTag("KitchenDoor").GetComponent<Collider>().isTrigger = false;
            }
        }
    }
}

