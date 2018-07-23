using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelRoute : MonoBehaviour {

    [SerializeField] SolarSystem systemOne;
    [SerializeField] SolarSystem systemTwo;
    ParticleSystem moveParticleSystem;

    float distanceBetweenSystems;
    // Use this for initialization
    void Start ()
    {
        distanceBetweenSystems = Vector3.Distance(systemOne.transform.position, systemTwo.transform.position);
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, distanceBetweenSystems);
        gameObject.transform.position = systemOne.transform.position + (systemTwo.transform.position - systemOne.transform.position) / 2;
        gameObject.transform.LookAt(systemTwo.transform);

        Transform lineRendererOffsetLeft = transform.Find("LineRendererOffsetLeft");
        Transform lineRendererOffsetRight = transform.Find("LineRendererOffsetRight");
        ConfigureMovementSystem(transform.Find("SystemOneToTwo").gameObject, lineRendererOffsetLeft.position, lineRendererOffsetLeft.position + (systemOne.transform.position - systemTwo.transform.position));
        ConfigureMovementSystem(transform.Find("SystemTwoToOne").gameObject, lineRendererOffsetRight.position, lineRendererOffsetRight.position + (systemOne.transform.position - systemTwo.transform.position));
    }

    private void ConfigureMovementSystem(GameObject moveSystem, Vector3 startPosition, Vector3 endPosition)
    {
        moveSystem.transform.position = gameObject.transform.position;
        //moveSystem.transform.LookAt(endPosition);
        //ParticleSystem moveParticleSystem = moveSystem.GetComponent<ParticleSystem>();
        //moveParticleSystem.Stop();
        //moveParticleSystem.transform.position = moveSystem.transform.position;
        //var main = moveParticleSystem.main;
        //main.startLifetime = distanceBetweenSystems / 5;
        //moveParticleSystem.Play();

        LineRenderer lineRenderer = moveSystem.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
