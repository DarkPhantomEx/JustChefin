using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditHUD : MonoBehaviour
{
    //All the HUD text elements can be edited through this script
    [SerializeField]
    Text ObjectiveBody;
    [SerializeField]
    GameObject Objective;
    [SerializeField]
    Text Instr;
    [SerializeField]
    Text RecipeName;
    [SerializeField]
    Text Timer;
    //[SerializeField]
    //int objCount;


    private IEnumerator coroutine;

    public void setHUD(string HUDBox, string toPrint)
    {
        

        switch (HUDBox)
        {
            case "ObjC":
                //++objCount;
                Objective.SetActive(true);
                ObjectiveBody.text = toPrint;
                coroutine = fadeObj(4);
                StartCoroutine(coroutine);
                Debug.Log("Coroutine Begins");
                //if (objCount == 0)
                //{
                //    Objective.SetActive(false);
                //}
                break;

            case "Obj":
                Objective.SetActive(true);
                ObjectiveBody.text = toPrint;
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

    IEnumerator fadeObj(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //objCount--;
        Objective.SetActive(false);
        Debug.Log("Coroutine complete");
    }

    void Start()
    {
        //objCount = 1;
        Objective.SetActive(false);
    }
}
