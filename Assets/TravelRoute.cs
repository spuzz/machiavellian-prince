using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelRoute : MonoBehaviour {

    [SerializeField] Planet planetOne;
    [SerializeField] Planet planetTwo;
    ParticleSystem moveParticleSystem;

    float distanceBetweenPlanets;
    // Use this for initialization
    void Start ()
    {
        distanceBetweenPlanets = Vector3.Distance(planetOne.transform.position, planetTwo.transform.position);
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, distanceBetweenPlanets);
        gameObject.transform.position = planetOne.transform.position + (planetTwo.transform.position - planetOne.transform.position) / 2;
        gameObject.transform.LookAt(planetTwo.transform);

        Transform lineRendererOffsetLeft = transform.Find("LineRendererOffsetLeft");
        Transform lineRendererOffsetRight = transform.Find("LineRendererOffsetRight");
        ConfigureMovementSystem(transform.Find("PlanetOneToTwo").gameObject, lineRendererOffsetLeft.position, lineRendererOffsetLeft.position + (planetOne.transform.position - planetTwo.transform.position));
        ConfigureMovementSystem(transform.Find("PlanetTwoToOne").gameObject, lineRendererOffsetRight.position, lineRendererOffsetRight.position + (planetOne.transform.position - planetTwo.transform.position));
    }

    private void ConfigureMovementSystem(GameObject moveSystem, Vector3 startPosition, Vector3 endPosition)
    {
        moveSystem.transform.position = gameObject.transform.position;
        //moveSystem.transform.LookAt(endPosition);
        //ParticleSystem moveParticleSystem = moveSystem.GetComponent<ParticleSystem>();
        //moveParticleSystem.Stop();
        //moveParticleSystem.transform.position = moveSystem.transform.position;
        //var main = moveParticleSystem.main;
        //main.startLifetime = distanceBetweenPlanets / 5;
        //moveParticleSystem.Play();

        LineRenderer lineRenderer = moveSystem.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
