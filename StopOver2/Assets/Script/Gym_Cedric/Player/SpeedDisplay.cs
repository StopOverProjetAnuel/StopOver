using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedDisplay : MonoBehaviour
{
    //public GameObject speedText;
    //public TextMeshProUGUI tMPSpeed;
    public GameObject m_Player;
    public Rigidbody rb_Player;

    //public blendtreeExemple blendtreeExemple;

    [HideInInspector] public float currentSpeed;

    private void Awake()
    {
        //tMPSpeed = speedText.GetComponent<TextMeshProUGUI>();
        rb_Player = m_Player.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        currentSpeed = Mathf.Clamp(rb_Player.velocity.magnitude, 0, Mathf.Infinity);
        int currentSpeedInt = Mathf.RoundToInt(currentSpeed);
        //tMPSpeed.SetText("" + currentSpeedInt);
        

        //float currentSpeedIntNormilezed = currentSpeed / 58;
        //Debug.Log(currentSpeedIntNormilezed);
        //blendtreeExemple.speed = currentSpeedIntNormilezed;
    }
}