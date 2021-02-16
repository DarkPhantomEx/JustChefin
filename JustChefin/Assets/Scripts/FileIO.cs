using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class FileIO
{
    public static void SaveRecipeData(RecipeSystem Recette)
    {
        BinaryFormatter bF = new BinaryFormatter();
        //Creates a "save file" in the User's folder under the name of recette.jc
        FileStream file = new FileStream(Application.persistentDataPath + "/recette.jc", FileMode.Create);

        RecipeData data = new RecipeData(Recette);

        bF.Serialize(file, data);
        file.Close();
    }

    public static void LoadRecipeData(ref RecipeData data)
    {
        if(File.Exists(Application.persistentDataPath + "/recette.jc"))
        {
            BinaryFormatter bF = new BinaryFormatter();
            //Opens the save file in the User's folder under the name of recette.jc
            FileStream file = new FileStream(Application.persistentDataPath + "/recette.jc", FileMode.Open);

            //Deserializes the saved recette.jc file
            data = (RecipeData)bF.Deserialize(file);
            file.Close();
        }
        else
        {
            Debug.LogError("File Doesn't Exist, bud.");           
        }
    }

}

[SerializeField]
public class RecipeData
{
    //A class that contains all the pertinent data from RecipeSystem.cs This data will be stored in our save file "recette.jc"

    public List<int> locID;
    public List<string> Instr;
    public List<int> timer;
    public int num;


    public RecipeData(RecipeSystem Recette)
    {
        this.locID = new List<int>(Recette.locID);
        this.Instr = new List<string>(Recette.Instr);
        this.timer = new List<int>(Recette.timer);
        this.num = Recette.numInstr;
    }

}

