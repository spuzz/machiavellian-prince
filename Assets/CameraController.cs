using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] float maxDistance = 300;
    [SerializeField] float minDistance = 50;
    [SerializeField] float startDistance = 75;
    [SerializeField] float scrollSpeed = 10;

    [SerializeField] float mapWidthLimitOnZoomedIn = 1f;
    [SerializeField] float mapHeightLimitOnZoomedIn = 1f;
    [SerializeField] float mapWidthLimitOnZoomedOut = 0;
    [SerializeField] float mapHeightLimitOnZoomedOut = 0;

    float currentZoom;
    Universe universe;

    Vector3 mapSize;

    private void Awake()
    {
        universe = FindObjectOfType<Universe>();
        transform.position = new Vector3(0, startDistance, 0);
        currentZoom = transform.localPosition.y;
    }
    private void Start()
    {

        mapSize = universe.GetMapSize();

    }


    void Update () {
        CheckZoom();
        Vector3 position = transform.position;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            position = position + (new Vector3(0, 0, -1));

        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            position = position + (new Vector3(-1, 0, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            position = position + (new Vector3(0, 0, 1));
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            position = position + (new Vector3(1, 0, 0));
        }
        transform.position = CheckLimits(position);
       
    }

    public void SetMapSize(Vector3 mapSize)
    {
        this.mapSize = mapSize;
    }

    public void SetPosition(Vector3 position)
    {
        
        if(position.y == 0)
        {
            position.y = currentZoom;
        }
        transform.position = position;
        CheckZoom();
        transform.position = CheckLimits(transform.position);
    }

    private Vector3 CheckLimits(Vector3 position)
    {
        float zoomPerc = 1.0f - ((position.y -minDistance )/ (maxDistance - minDistance));

        float xPos = position.x;
        float maxWidth = mapSize.x * zoomPerc;
        if (xPos > maxWidth)
        {
            xPos = maxWidth;
        }
        else if(xPos < -(maxWidth))
        {
            xPos = -(maxWidth);
        }

        float zPos = position.z;
        float maxHeight = mapSize.z * zoomPerc;
        if (zPos > maxHeight)
        {
            zPos = maxHeight;
        }
        else if (zPos < -(maxHeight))
        {
            zPos = -(maxHeight);
        }

        return new Vector3(xPos, position.y, zPos);
    }

    private void CheckZoom()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        if(zoom != 0)
        {

            currentZoom -= zoom * scrollSpeed;
            if (currentZoom > maxDistance)
            {
                currentZoom = maxDistance;
            }
            else if (currentZoom < minDistance)
            {
                currentZoom = minDistance;
            }

            if(zoom > 0)
            {
                Vector3 cursorPoint = Input.mousePosition;
                cursorPoint.z = transform.position.y;
                cursorPoint = Camera.main.ScreenToWorldPoint(cursorPoint);

                float xPosMove = (cursorPoint.x - transform.localPosition.x) * 0.1f;
                float zPosMove = (cursorPoint.z - transform.localPosition.z) * 0.1f;
                transform.localPosition = new Vector3(transform.localPosition.x + xPosMove, currentZoom, transform.localPosition.z + zPosMove);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, currentZoom, transform.localPosition.z);
            }
            
        }

    }
}
