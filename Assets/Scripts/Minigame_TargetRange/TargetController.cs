using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public List<Transform> waypoints;
    public float speed = 1f;

    private Transform nextWaypoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "projectile")
        {
            Debug.Log("HIT");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (nextWaypoint == null)
        {
            nextWaypoint = waypoints[Random.Range(0, waypoints.Count)];
        } 
        
        if (Vector3.Distance(transform.position, nextWaypoint.position) < .1f)
        {
            nextWaypoint = waypoints[Random.Range(0, waypoints.Count)];
        } else
        {
            transform.position = Vector3.MoveTowards(transform.position, nextWaypoint.position, speed * Time.deltaTime);
        }
    }
}
