using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Script that manages updating the values of the suspicion bar */

public class SuspicionBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    
    // Resetting suspicion bar value
    public void SetMinSuspicion(float minSuspicionValue)
    {
        slider.value = minSuspicionValue;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    // Set the current value of the suspicion bar
    public void SetSuspicion(float suspicionValue)
    {
        slider.value = suspicionValue;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
