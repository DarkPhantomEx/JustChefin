using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    //import FMOD sounds
    [FMODUnity.EventRef]
    public string TickingEventPath, CaughtEventPath, SizzlingEventPath, LootingEventPath, WalkingEventPath, CrouchingEventPath;
    public EventInstance Ticking, Caught, Sizzling, Looting, Walking, Crouching;

    [FMODUnity.EventRef]
    public string PausePath;
    public EventInstance pause;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(transform.gameObject);
        Ticking = FMODUnity.RuntimeManager.CreateInstance(TickingEventPath);                     //initialize FMOD Sounds
        Caught = FMODUnity.RuntimeManager.CreateInstance(CaughtEventPath);
        Sizzling = FMODUnity.RuntimeManager.CreateInstance(SizzlingEventPath);
        Looting = FMODUnity.RuntimeManager.CreateInstance(LootingEventPath);
        Walking = FMODUnity.RuntimeManager.CreateInstance(WalkingEventPath);
        Crouching = FMODUnity.RuntimeManager.CreateInstance(CrouchingEventPath);

        pause = FMODUnity.RuntimeManager.CreateInstance(PausePath);
    }
    public void StopAllLoop()
    {
        Ticking.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Sizzling.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
