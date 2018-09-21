using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

    SystemUI systemUI;

    void Start()
    {
        GetComponent<Canvas>().enabled = true;
        systemUI = FindObjectOfType<SystemUI>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SelectObject(GameObject selectedObject)
    {
        SolarSystem system = selectedObject.GetComponent<SolarSystem>();
        if (system)
        {
            systemUI.SetSystem(system);
        }
    }
}
