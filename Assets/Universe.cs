using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour {

    [SerializeField] float timePerDay = 1.0f;
    int currentDay;

    float startTime;
    // Use this for initialization
    void Start () {
        currentDay = 1;
    }
	
	// Update is called once per frame
	void Update () {
        int newDay = Mathf.FloorToInt(Time.time / timePerDay);
        if(newDay != currentDay)
        {
            onDayChanged(newDay - currentDay);
            currentDay = newDay;
        }

    }

    public delegate void OnDayChanged(int days); // declare new delegate type
    public event OnDayChanged onDayChanged; // instantiate an observer set
}
