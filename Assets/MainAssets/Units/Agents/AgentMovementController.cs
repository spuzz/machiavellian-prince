using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovementController : MonoBehaviour {

    [SerializeField] float stoppingDistance = 2.0f;
    [SerializeField] float movementSpeed = 10.0f;
    public delegate void OnReachedTarget(); // declare new delegate type
    public event OnReachedTarget onReachedTarget; // instantiate an observer set

    SpeedUI speedUI;
    Vector3 target;
    bool moving = false;
    // Update is called once per frame

    private void Awake()
    {
        speedUI = FindObjectOfType<SpeedUI>();
    }
    void Update ()
    {
        if(moving)
        {
            UpdatePosition();
        }
        
    }

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
        moving = true;
    }

    private void UpdatePosition()
    {
        if (Vector3.Distance(transform.position, target) < stoppingDistance)
        {
            onReachedTarget();
            moving = false;
        }
        else
        {
            Vector3 direction = (target - transform.position).normalized;
            transform.position = transform.position + (direction * (movementSpeed * (Time.deltaTime * speedUI.GetSpeed())));
        }
    }
}
