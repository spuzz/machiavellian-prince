using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableComponent : MonoBehaviour {

    [Tooltip("The UI canvas prefab")]
    [SerializeField]
    GameObject enemyCanvasPrefab = null;
    string name;
    Camera cameraToLookAt;
    bool changeText = false;
    Button button;
    Text text;
    SystemUI systemUI;
    void Start()
    {
        cameraToLookAt = Camera.main;
        Instantiate(enemyCanvasPrefab, transform.position, Quaternion.identity, transform);
        text = GetComponentInChildren<Text>();
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(Selected);
        systemUI = FindObjectOfType<SystemUI>();
    }

    public void UpdateName(string name)
    {
        this.name = name;
        changeText = true;
    }

    private void LateUpdate()
    {
        if(changeText == true)
        {

            text.text = name;
            changeText = false;
        }

        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
    }

    public void Selected()
    {
        SolarSystem system = GetComponentInParent<SolarSystem>();
        if (system)
        {
            systemUI.SetSystem(system);
        }
    }
}
