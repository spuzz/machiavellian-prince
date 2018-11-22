using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildTracker : MonoBehaviour {

    float timeLeft;
    float buildTIme;
    int id = 1;
    public delegate void OnBuildComplete(long id);
    public event OnBuildComplete onBuildComplete;
    Universe universe;
    float timePerDay;
    Dictionary<long, Tracker> trackers = new Dictionary<long, Tracker>();

    public class Tracker
    {
        public long startDay;
        public float timetoBuild;
        public float timeLeft;
        public long id;
    };
    private void Awake()
    {
        universe = FindObjectOfType<Universe>();
        
    }
    private void Start()
    {
        timePerDay = universe.GetTimePerDay();
    }
    public long StartBuild(float timeToBuild)
    {
        Tracker tracker = new Tracker();
        tracker.startDay = universe.GetCurrentDay();
        tracker.timetoBuild = timeToBuild;
        tracker.timeLeft = timeToBuild;
        tracker.id = id;
        trackers.Add(id,tracker);
        return id;
    }

    public float GetTimeLeftInSeconds(long id)
    {
        if(!trackers.ContainsKey(id))
        {
            return 0;
        }
        else
        {
            return trackers[id].timeLeft;
        }
    }

    public float GetTimeLeftInDays(long id)
    {
        return Convert.ToInt32(GetTimeLeftInSeconds(id) / timePerDay);
    }

    public int PercentCompleted(long id)
    {
        if (!trackers.ContainsKey(id) || trackers[id].timeLeft == 0)
        {
            return 100;
        }
        else
        {
            return Convert.ToInt32(100.0f - ((trackers[id].timeLeft / trackers[id].timetoBuild) * 100.0f));
        }
    }

    private void Update()
    {
        foreach (Tracker tracker in trackers.Values)
        {
            tracker.timeLeft -= Time.deltaTime * universe.GetSpeed();
            if (tracker.timeLeft <= 0)
            {
                if (onBuildComplete != null)
                {
                    onBuildComplete(tracker.id);
                }
            }
        }
        
    }
    private void LateUpdate()
    {
        foreach (var i in trackers.Where(d => d.Value.timeLeft <= 0).ToList())
        {
            trackers.Remove(i.Key);
        }
    }

}
