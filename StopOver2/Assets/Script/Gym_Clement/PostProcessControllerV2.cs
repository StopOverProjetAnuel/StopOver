using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor;


public class PostProcessControllerV2 : MonoBehaviour
{
    #region variables public 
    public VolumeProfile Twilight;
    public VolumeProfile Night;
    public VolumeProfile Dawn;

    public float Timer;

    [Range(0,100)]
    public float twilightToNight;
    [Range(0, 100)]
    public float nightToDawn;
    #endregion

    #region variables private
    private float currentTimer;

    #endregion
    public void Start()
    {
        initiate();
    }
    private void initiate()
    {

    }
    private IEnumerator CurrentTimer(float Timer)
    {
        if(Timer <= 0)
        {
        yield return null;
          //  StartCoroutine(CurrentTimer)
        }

    }
    private IEnumerator TimesUp(float Timer)
    {
        yield return new WaitForSeconds(Timer);
    }

}
