using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogCamera : MonoBehaviour {

    Dictionary<GameObject, GameObject> visibleObjects = new Dictionary<GameObject, GameObject>();
    List<GameObject> nearbyVisibleObjects = new List<GameObject>();
    List<GameObject> updateVisibleObjects = new List<GameObject>();
    [SerializeField] GameObject fogObject;


    private void Update()
    {
        updateVisibleObjects.Clear();
        foreach (GameObject visibleObj in visibleObjects.Keys)
        {
            
            Collider[] hitColliders = Physics.OverlapSphere(visibleObj.transform.position, 25);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if(UpdateVisibility(hitColliders[i].transform, true, 10) == true)
                {
                    if (!visibleObjects.ContainsKey(hitColliders[i].gameObject))
                    {
                        updateVisibleObjects.Add(hitColliders[i].gameObject);
                    }
                }
       
                
                i++;
            }
        }
        if(nearbyVisibleObjects.Count != updateVisibleObjects.Count)
        {
            int test = 0;
        }
        foreach(GameObject obj in nearbyVisibleObjects)
        {
            if(!updateVisibleObjects.Contains(obj))
            {
                UpdateVisibility(obj.transform, false, 9);
            }
        }
        nearbyVisibleObjects.Clear();
        foreach(GameObject obj in updateVisibleObjects)
        {
            nearbyVisibleObjects.Add(obj);
        }
    }

    public void ClearObjects()
    {
        foreach (GameObject obj in visibleObjects.Keys)
        {
            UpdateVisibility(obj.transform, false, 9);
            Destroy(visibleObjects[obj]);
        }
        visibleObjects.Clear();
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
            UpdateVisibility(objectTransform, true, 10);
            GameObject visibleObj = Instantiate(fogObject, base.transform);
            SetPosition(objectTransform, visibleObj);
            visibleObjects.Add(objectTransform.gameObject, visibleObj);
        }

    }

    private  bool UpdateVisibility(Transform objectTransform, bool visible, int layer)
    {
        SelectableComponent select = objectTransform.gameObject.GetComponentInChildren<SelectableComponent>();
        if (select)
        {
            objectTransform.gameObject.layer = layer;
            foreach (Transform trans in objectTransform.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layer;
            }
            select.SetVisible(visible);
            return true;
        }
        return false;



    }

    public void RemoveObject(Transform objectTransform)
    {
        if(visibleObjects.ContainsKey(objectTransform.gameObject))
        {
            UpdateVisibility(objectTransform, false, 9);
            GameObject visibleObj = visibleObjects[objectTransform.gameObject];
            visibleObjects.Remove(visibleObj);
            Destroy(visibleObj);
        }

    }

    private static void SetPosition(Transform objectTransform, GameObject visibleObj)
    {
        visibleObj.transform.localPosition = new Vector3(objectTransform.localPosition.x / 5.0f, objectTransform.localPosition.z / 5.0f, 0.31f);
    }
}
