using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_Timer : MonoBehaviour
{

    public Text textTimer;
    public Text[] arrayTextTime;

    public float timer;
    private float oldTime;

    private bool writtingTime;
    private int x;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += 1 * Time.deltaTime;

        textTimer.text = "Time = "+timer;

        if (writtingTime)
        {
            WrittingTime();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oldTime = timer;
            x = 0;
            writtingTime = true;
            timer = 0;

        }
    }

    private void WrittingTime()
    {
        foreach (Text textTimeX in arrayTextTime)
        {
            x += 1;
            Debug.Log(textTimeX.text);
            if(textTimeX.text == "" && writtingTime)
            {
                writtingTime = false;
                textTimeX.text = "Time"+ x + " =" + oldTime;
                Debug.Log("timer note");
            }
        }
    }
}
