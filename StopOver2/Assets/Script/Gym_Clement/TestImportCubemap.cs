using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor;


public class TestImportCubemap : MonoBehaviour
{
    private Camera mCamera;
    private Tex2DToCubemap tex2cube;
    private Volume vol;
    private VolumeProfile volProfil;
    private HDRISky hdriSky;

    private void Start()
    {
        mCamera = Camera.main;
        tex2cube = GetComponent<Tex2DToCubemap>();
        vol = mCamera.GetComponent<Volume>();
        volProfil = vol.profile;
        volProfil.TryGet<HDRISky>(out hdriSky);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            hdriSky.hdriSky.value = tex2cube.output;
        }
    }


}
