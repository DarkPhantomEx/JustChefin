using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditHUD : MonoBehaviour
{
    //All the HUD text elements can be edited through this script
    [SerializeField]
    Text Objective;
    [SerializeField]
    Text Instr;
    [SerializeField]
    Text RecipeName;
    [SerializeField]
    Text Timer;

    public void setHUD(string HUDBox, string toPrint)
    {
        switch (HUDBox)
        {
            case "Obj":
                Objective.text = toPrint;
                break;

            case "Ins":
                Instr.text = toPrint;
                break;

            case "+Ins":
                Instr.text += toPrint;
                break;

            case "Rec":
                RecipeName.text = toPrint;
                break;
            case "Tim":
                Timer.text = toPrint;
                break;
            default:
                Debug.LogError("Incorrect TextBox type. Sorry");
                break;
        }
    }
    
    public string getTimerText()
    {
        return Timer.text;
    }
}
