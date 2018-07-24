using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpireController : MonoBehaviour {

    enum MissionState
    {
        Idle,
        Expand,
        Defend,
        Attack,
        Grow,
    }

    [SerializeField] MissionState currentState;
    Empire empire;


    void Start () {
        empire = gameObject.GetComponent<Empire>();
        currentState = MissionState.Idle;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
