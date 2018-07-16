using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderUI : MonoBehaviour {

    Leader leader;
	// Use this for initialization
	void Start () {
        var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.onMouseOverPlanet += ProcessMouseOverPlanet;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ProcessMouseOverPlanet(Planet planet)
    {
        if (Input.GetMouseButton(0) == true)
        {
            StopAllCoroutines();
            this.leader = planet.GetLeader();
        }
    }

    public void AddInfluence()
    {

        // Temp Code for testing - increase influence from random player;
        Player[] players = FindObjectsOfType<Player>();
        int owner = Random.Range(0, players.Length);
        leader.IncreaseInfluence(players[owner], 100);
    }
}
