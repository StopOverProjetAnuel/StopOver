using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMOD_SoundCallerMultiple : FMOD_SoundCaller
{
    private EventInstance[] soundInstance;

    [Header("Parameters")]
    [SerializeField] private EventReference[] soundEvent;

    private void Awake()
    {
        for (int i = 0; i < soundEvent.Length; i++)
        {
            soundInstance[i] = RuntimeManager.CreateInstance(soundEvent[i]);
        }
    }

    public override void SoundStart()
    {
        for (int i = 0; i < soundInstance.Length; i++)
        { 
            soundInstance[i].start();
        }
    }

    public override void SoundStop()
    {
        for (int i = 0; i < soundInstance.Length; i++)
        {
            soundInstance[i].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
