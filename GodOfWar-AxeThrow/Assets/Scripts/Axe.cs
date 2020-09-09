using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public bool activated;
    public float rotationSpeed;

    void Update()
    {
        if (activated)
        {
            transform.localEulerAngles += transform.forward * rotationSpeed;
        
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        Debug.Log(other.gameObject.name);
        GetComponent<Rigidbody>().Sleep();
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        GetComponent<Rigidbody>().isKinematic = true;
        activated = false;
    }
}
