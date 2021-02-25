using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; //Reqd. for FileIO serialization
using UnityEngine;
using System; //For the [Serializable] tag

public static class FileIO
{
    //Checks if recette.jc exists. 
    //On RIT LAB PCs, it can be found under "D:\Profiles\<ritID>\AppData\LocalLow\DefaultCompany\JustChefin"
    //*AppData is a HiddenFolder
    public static bool DoesRecipeFileExist()
    {
        if (File.Exists(Application.persistentDataPath + "/recette.jc"))
        {
            return true;
        }

        else return false;
    }

    public static RecipeData SaveRecipeData(RecipeSystem Recette)
    {
        BinaryFormatter bF = new BinaryFormatter();
        //Creates a "save file" in the User's folder under the name of recette.jc
        FileStream file = new FileStream(Application.persistentDataPath + "/recette.jc", FileMode.Create);

        RecipeData data = new RecipeData(Recette);

        //For Debug purposes
        Debug.Log("=====TIME TO SAVE=====");
        Debug.Log(data.Instr[0]);
        Debug.Log(data.Instr[1]);
        Debug.Log(data.Instr[2]);
        Debug.Log(data.Instr[3]);


        bF.Serialize(file, data);
        Debug.Log("=====SAVE COMPLETED=====");
        file.Close();
        return data;
    }

    public static void LoadRecipeData(ref RecipeData data)
    {
        if(File.Exists(Application.persistentDataPath + "/recette.jc"))
        {
            Debug.Log("=====TIME TO LOAD=====");
            BinaryFormatter bF = new BinaryFormatter();
            //Opens the save file in the User's folder under the name of recette.jc
            FileStream file = new FileStream(Application.persistentDataPath + "/recette.jc", FileMode.Open);

            //Deserializes the saved recette.jc file
            data = (RecipeData)bF.Deserialize(file);
            Debug.Log("=====LOAD COMPLETED=====");
            file.Close();
        }
        else
        {
            Debug.LogError("File Doesn't Exist, bud.");           
        }
    }

}

[Serializable]
public class RecipeData
{
    //A class that contains all the pertinent data from RecipeSystem.cs This data will be stored in our save file "recette.jc"

    public List<int> locID;
    public List<string> Instr;
    public List<int> timer;
    public List<int> numInstr;
    public int numRec;


    public RecipeData(RecipeSystem Recette)
    {
        this.locID = new List<int>(Recette.locID);
        this.Instr = new List<string>(Recette.Instr);
        this.timer = new List<int>(Recette.timer);
        this.numInstr = new List<int>(Recette.numInstr);
        this.numRec = Recette.numRec;
    }

}

