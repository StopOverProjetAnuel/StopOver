using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fmod_MusicManager : MonoBehaviour
{
    FMOD.Studio.EventInstance musicIntance;

    [Tooltip("Sound bank from fmod")]
    public FMODUnity.EventReference musicEvent;

    [Header("Parameters from the music bank")]
    [Range(0f, 100f)] public float intensity = 0f; //Called in C_CharacterManager.cs
    [Range(0, 1)] public int boost = 0; //Called in C_CharacterBoost.cs
    [SerializeField] [Range(0, 1)] private int ressources = 0;
    [Range(0, 1)] public int pause = 0; //Call in GMMenu.cs

    [Header("Parameters")]
    [SerializeField] private float timeBeforeActiveResources = 1f;
    [SerializeField] private float timeMaxBeforeUnactiveResources = 3f;
    private float currentTimeActive;
    [HideInInspector] public bool isCollecting = false; //Called in NudgeBars.cs
    private float continueTime = 0f;
    private bool continueCollecting = false;
    private float waitResources = 0f;



    private void Start()
    {
        musicIntance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicIntance.start();
        currentTimeActive = timeBeforeActiveResources;
    }

    private void Update()
    {
        musicIntance.setParameterByName("Boost", boost);
        musicIntance.setParameterByName("Pause", pause);

        ResourcesMusic();
        IntansityLevel();
    }

    private void IntansityLevel()
    {
        if (pause == 1)
        {
            musicIntance.setParameterByName("Intensity", 45);
            return;
        }

        musicIntance.setParameterByName("Intensity", intensity);
    }

    private void ResourcesMusic()
    {
        continueTime = (isCollecting) ? 1f : Mathf.Clamp(continueTime - Time.deltaTime, 0f, 1f); //Set max value when collecting things
        continueCollecting = (continueTime > 0f) ? true : false; //Will "say" the player continue collecting if he collect thing in less than 1sec

        waitResources = (continueCollecting) ? Mathf.Clamp(waitResources + Time.deltaTime, 0f, currentTimeActive) : Mathf.Clamp(waitResources - Time.deltaTime, 0f, currentTimeActive); //Increase and decrease a timer to play or stop the bank

        if (waitResources >= timeBeforeActiveResources)
        {
            currentTimeActive = timeMaxBeforeUnactiveResources;
            ressources = 2;
        }
        else
        {
            currentTimeActive = timeBeforeActiveResources;
            ressources = 0;
        }

        musicIntance.setParameterByName("Ressources", ressources);
    }

    public void StopMusic()
    {
        musicIntance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
