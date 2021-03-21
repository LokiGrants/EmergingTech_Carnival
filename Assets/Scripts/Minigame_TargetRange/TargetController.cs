using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "projectile")
        {
            Debug.Log("HIT");
            Destroy(gameObject);
        }
    }

    //Move around
}
