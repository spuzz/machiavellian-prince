using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRaycaster : MonoBehaviour {

    float maxRaycastDepth = 100f; // Hard coded value
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
    [SerializeField] Texture2D planetCursor = null;

    Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
    // New Delegates
    public delegate void OnMouseOverPlanet(Planet planet); // declare new delegate type
    public event OnMouseOverPlanet onMouseOverPlanet; // instantiate an observer set

	// Update is called once per frame
	void Update () {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        // Check if pointer is over an interactable UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Impiment UI Interaction
        }
        else
        {
            PerformRayCasts();
        }
    }

    void PerformRayCasts()
    {
        if (screenRect.Contains(Input.mousePosition))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (RayCastForPlanet(ray)) { return; }
        }

    }

    private bool RayCastForPlanet(Ray ray)
    {
        RaycastHit raycastHit;
        bool potentialEnemyHit = Physics.Raycast(ray, out raycastHit, maxRaycastDepth);
        if (potentialEnemyHit) // if hit no priority object
        {
            Planet planet = raycastHit.collider.gameObject.GetComponent<Planet>();
            if (planet)
            {
                Cursor.SetCursor(planetCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverPlanet(planet);
                return true;
            }
        }
        return false;
    }
}
