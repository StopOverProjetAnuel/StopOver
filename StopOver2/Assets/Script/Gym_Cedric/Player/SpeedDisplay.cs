using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedDisplay : MonoBehaviour
{
    public GameObject speedText;
    public TextMeshProUGUI tMPSpeed;
    public GameObject m_Player;
    public Rigidbody rb_Player;

    private void Awake()
    {
        tMPSpeed = speedText.GetComponent<TextMeshProUGUI>();
        rb_Player = m_Player.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float currentSpeed = Mathf.Clamp(rb_Player.velocity.magnitude, 0, Mathf.Infinity);
        tMPSpeed.SetText("" + Mathf.RoundToInt(currentSpeed));
        //Debug.Log("Speed = " + rb_Player.velocity.magnitude);
    }
}