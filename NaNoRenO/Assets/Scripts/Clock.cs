using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Clock : MonoBehaviour
{
    DateTime Time;
    [SerializeField] bool Debugger;
    [SerializeField] int StartingHour;
    [SerializeField] int EndingHour;
    [SerializeField] float Speed;
    [SerializeField] TextMeshProUGUI Hour;
    [SerializeField] TextMeshProUGUI Minute;

    // Start is called before the first frame update
    void Start()
    {
        Time = new DateTime(2016, 7, 15, StartingHour, 0, 0);
        Debug.Log(Time.Hour + " " + Time.Minute);
        StartCoroutine(MoveTime());
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    IEnumerator MoveTime()
    {
        while(Time.Hour != EndingHour)
        {
            Time = Time.AddMinutes(1);
            yield return new WaitForSeconds(Speed);
            Debug.Log(Time.Hour + " " + Time.Minute);
            Hour.text = Time.Hour.ToString() +":";
            if(Time.Minute < 10)
            {
                Minute.text = "0" + Time.Minute.ToString();
            }
            else
                Minute.text = Time.Minute.ToString();
        }
    }
}
