using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRaycaster : MonoBehaviour {

    float maxRaycastDepth = 100f; // Hard coded value
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
    [SerializeField] Texture2D systemCursor = null;

    Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
    // New Delegates
    public delegate void OnMouseOverSystem(SolarSystem system); // declare new delegate type
    public event OnMouseOverSystem onMouseOverSystem; // instantiate an observer set

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
            if (RayCastForSystem(ray)) { return; }
        }

    }

    private bool RayCastForSystem(Ray ray)
    {
        RaycastHit raycastHit;
        bool potentialEnemyHit = Physics.Raycast(ray, out raycastHit, maxRaycastDepth);
        if (potentialEnemyHit) // if hit no priority object
        {
            SolarSystem system = raycastHit.collider.gameObject.GetComponent<SolarSystem>();
            if (system)
            {
                Cursor.SetCursor(systemCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverSystem(system);
                return true;
            }
        }
        return false;
    }
}
