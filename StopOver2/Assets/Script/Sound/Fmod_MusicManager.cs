using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Fmod_MusicManager : MonoBehaviour
{
    FMOD.Studio.EventInstance musicIntance;

    [BankRef] public string musicEvent;

    [Range(0f, 100f)] public float intensity;
    [Range(0, 1)] public int boost;
    [Range(0f, 100f)] public float ressources;
    [Range(0, 1)] public int pause;

    private void Start()
    {
        musicIntance = RuntimeManager.CreateInstance(musicEvent);
        musicIntance.start();
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
        musicIntance.setParameterByName("Ressources", ressources);
    }
}
