using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogCamera : MonoBehaviour {

    Dictionary<GameObject, GameObject> visibleObjects = new Dictionary<GameObject, GameObject>();
    List<GameObject> nearbyVisibleObjects = new List<GameObject>();
    [SerializeField] GameObject fogObject;

    private void Update()
    {
        nearbyVisibleObjects.Clear();
        foreach (GameObject visibleObj in visibleObjects.Keys)
        {
            SetPosition(visibleObj.transform, visibleObjects[visibleObj]);
            Collider[] hitColliders = Physics.OverlapSphere(visibleObj.transform.position, 34);
            int i = 0;
            while (i < hitColliders.Length)
            {

                if (!nearbyVisibleObjects.Contains(hitColliders[i].gameObject))
                {
                    nearbyVisibleObjects.Add(hitColliders[i].gameObject);
                    UpdateVisibility(hitColliders[i].gameObject.transform);
                }

                i++;
            }
        }
    }

    public bool IsGameObjectVisible(GameObject visibleGameObject)
    {
        if(nearbyVisibleObjects.Contains(visibleGameObject))
        {
            return true;
        }

        if(visibleObjects.ContainsKey(visibleGameObject))
        {
            return true;
        }

        return false;
    }

    public void UpdateObject(GameObject gameObjectToUpdate)
    {
        if(visibleObjects.ContainsKey(gameObjectToUpdate))
        {
            SetPosition(gameObjectToUpdate.transform, visibleObjects[gameObjectToUpdate]);
        }
    }

    public void AddObject(Transform objectTransform)
    {
        if(!visibleObjects.ContainsKey(objectTransform.gameObject))
        {
            UpdateVisibility(objectTransform);
            GameObject visibleObj = Instantiate(fogObject, base.transform);
            SetPosition(objectTransform, visibleObj);
            visibleObjects.Add(objectTransform.gameObject, visibleObj);
        }

    }

    private  void UpdateVisibility(Transform objectTransform)
    {
        if(objectTransform.gameObject.layer == 9)
        {
            objectTransform.gameObject.layer = 10;
            foreach (Transform trans in objectTransform.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = 10;
            }

        }
        SelectableComponent select = objectTransform.GetComponentInChildren<SelectableComponent>();
        if (select)
        {
            select.SetVisible(true);
        }

    }


    public void RemoveObject(Transform objectTransform)
    {
        if(visibleObjects.ContainsKey(objectTransform.gameObject))
        {
            GameObject visibleObj = visibleObjects[objectTransform.gameObject];
            visibleObjects.Remove(objectTransform.gameObject);
            Destroy(visibleObj);
        }

    }

    private static void SetPosition(Transform objectTransform, GameObject visibleObj)
    {
        visibleObj.transform.localPosition = new Vector3(objectTransform.localPosition.x / 5.0f, objectTransform.localPosition.z / 5.0f, 0.31f);
    }
}
