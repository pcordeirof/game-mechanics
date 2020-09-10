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
            transform.localEulerAngles += Vector3.forward * rotationSpeed;
        
        }

    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.layer == 8)
        {
            Debug.Log(other.gameObject.name);
            GetComponent<Rigidbody>().Sleep();
            GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            GetComponent<Rigidbody>().isKinematic = true;
            activated = false;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Breakable"))
        {
            if(other.GetComponent<BreakBox>() != null)
            {
                //other.GetComponent<BreakBox>().Break();
            }
        }
    }
}
