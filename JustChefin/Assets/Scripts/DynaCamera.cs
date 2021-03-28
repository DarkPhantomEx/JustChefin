using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynaCamera : MonoBehaviour
{
    [SerializeField]
    List<string> roomNames;
    [SerializeField]
    List<Vector3> CameraLoc;
    [SerializeField]
    int curRoom;
    [SerializeField]
    int numRooms;
    [SerializeField]
    GameObject Cam;


    // Start is called before the first frame update
    void Start()
    {
        defCameraDetails();
        curRoom = -1;
        Cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void defCameraDetails()
    {
        roomNames.Add("Kitchen");
        CameraLoc.Add(new Vector3(0f, 0f, 0f));
        roomNames.Add("DiningRoom");
        CameraLoc.Add(new Vector3(1000f, 0f, 0f));
        roomNames.Add("LockerRoom");
        CameraLoc.Add(new Vector3(-1000f, 0f, 0f));
        roomNames.Add("ManagersOffice");
        CameraLoc.Add(new Vector3(15000f, 0f, 0f));

        numRooms = 4;
    }


    public void onTriggerEnter(Collider Room)
    {
        Debug.Log("The Trigger is Room " + Room.name);

        for(int i =0; i< numRooms; i++)
        {
            if(Room.name == roomNames[i])
            {
                Cam.transform.position = CameraLoc[i];
                curRoom = i;
            }
        }
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
