using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidReset : MonoBehaviour
{
    public GameObject m_Player;
    private Vector3 startPosition;
    private Rigidbody m_rb; 

    private void Awake()
    {
        startPosition = m_Player.transform.position;
        m_rb = m_Player.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_rb.velocity.Set(0, 0, 0);
            m_Player.transform.position = startPosition;
        }
        //Debug.Log(collision.gameObject.CompareTag("Player"));
    }
}