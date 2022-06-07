using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMOD_SoundCallerSingle : FMOD_SoundCaller
{
    private EventInstance soundInstance;

    [Header("Parameters")]
    [SerializeField] private EventReference soundEvent;

    private void Awake()
    {
        soundInstance = RuntimeManager.CreateInstance(soundEvent);
    }

    public override void SoundStart()
    {
        soundInstance.start();
    }

    public override void SoundStop()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
