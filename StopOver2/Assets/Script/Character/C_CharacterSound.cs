using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class C_CharacterSound : MonoBehaviour
{
    #region Event Instance
    EventInstance engineInstance;
    EventInstance boostInstance;
    EventInstance overheatInstance;
    EventInstance overheatDownInstance;
    EventInstance boostReadyInstance;
    EventInstance speedMaxInstance;
    EventInstance hitInstance;
    EventInstance getBiomassInstance;
    EventInstance turnInstance;
    EventInstance nudgeBarsInstance;
    #endregion

    #region Banks
    [Header("BANKS")]
    public EventReference engineEvent;
    public EventReference boostEvent;
    public EventReference overheatEvent;
    public EventReference overheatDownEvent;
    public EventReference boostReadyEvent;
    public EventReference speedMaxEvent;
    public EventReference hitEvent;
    public EventReference getBiomassEvent;
    public EventReference turnEvent;
    public EventReference nudgeBarsEvent;
    #endregion

    [Header("Banks Parameters")]
    [SerializeField] [Range(0, 1)] private int boostActivation = 0; 
    [Range(0, 135)] public float engineIntensity = 0f;



    public void InitiateSoundValue()
    {
        #region Generate Instances
        boostInstance = RuntimeManager.CreateInstance(boostEvent);
        engineInstance = RuntimeManager.CreateInstance(engineEvent);
        overheatInstance = RuntimeManager.CreateInstance(overheatEvent);
        overheatDownInstance = RuntimeManager.CreateInstance(overheatDownEvent);
        boostReadyInstance = RuntimeManager.CreateInstance(boostReadyEvent);
        speedMaxInstance = RuntimeManager.CreateInstance(speedMaxEvent);
        hitInstance = RuntimeManager.CreateInstance(hitEvent);
        getBiomassInstance = RuntimeManager.CreateInstance(getBiomassEvent);
        turnInstance = RuntimeManager.CreateInstance(turnEvent);
        nudgeBarsInstance = RuntimeManager.CreateInstance(nudgeBarsEvent);
        #endregion

        engineInstance.start();
    }

    public void TriggerSound()
    {
        boostInstance.setParameterByName("TimeBoost", boostActivation);
        engineInstance.setParameterByName("Speed", engineIntensity);
    }

    public void BoostNOverheatStart()
    {
        boostInstance.start();
        overheatInstance.start();
    }

    public void BoostNOverheatStop()
    {
        boostInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        overheatInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void OverheatDownStart()
    {
        overheatDownInstance.start();
    }

    public void OverheatDownStop()
    {
        overheatInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void BoostReadyTrigger()
    {
        boostReadyInstance.start();
    }

    public void maxSpeedTrigger()
    {
        speedMaxInstance.start();
    }

    public void hitTrigger()
    {
        hitInstance.start();
    }

    public void GetBiomassTrigger()
    {
        nudgeBarsInstance.start();
        getBiomassInstance.start();
    }

    public void StrafeSoundStart()
    {
        turnInstance.start();
    }

    public void StrafeSoundStop()
    {
        turnInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}