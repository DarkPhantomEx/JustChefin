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
    //public void setObjective(string toPrint)
    //{
    //    Objective.text = toPrint;
    //}

    //public void setRecipeName(string toPrint)
    //{
    //    RecipeName.text = toPrint;
    //}

    //public void setInstr(string toPrint)
    //{
    //    Instr.text = toPrint;
    //}

    //public void setInstr(char add, string toPrint)
    //{
    //    if (add == '+')
    //    {
    //        Instr.text += toPrint;
    //    }
    //    else
    //    {
    //        Debug.LogError("Invalid operator, sorry");
    //    }
        
    //}

    //public void setTimerText(string toPrint)
    //{
    //    Timer.text = toPrint;
    //}

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
