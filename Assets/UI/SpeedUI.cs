using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUI : MonoBehaviour {


    [SerializeField] List<Speed> speeds;
    [SerializeField] Dictionary<Speed, float> speedValues = new Dictionary<Speed, float>();
    [SerializeField] SpeedUI speedUI;
    [SerializeField] Speed currentSpeed;
    [SerializeField] Image currentSpeedIcon;

    // Use this for initialization
    void Start () {
        if(!currentSpeed)
        {
            currentSpeed = speeds[0];
        }

        SetSpeed(currentSpeed);
    }
	

    public void IncreaseSpeed()
    {
        int index = speeds.IndexOf(currentSpeed);
        if (index + 1 < speeds.Count)
        {
            SetSpeed(speeds[index + 1]);
        }
    }

    public void DecreaseSpeed()
    {
        int index = speeds.IndexOf(currentSpeed);
        if (index > 0)
        {
            SetSpeed(speeds[index - 1]);
        }
    }

    public void SetSpeed(Speed speed)
    {
        this.currentSpeed = speed;
        currentSpeedIcon.sprite = speed.GetIcon();

    }

    public float GetSpeed()
    {
        return currentSpeed.GetSpeed();
    }

}
