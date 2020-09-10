using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBox : MonoBehaviour
{
    public GameObject breakedBox;
    
    public void Break()
    {
        GameObject breaked = Instantiate(breakedBox, transform.position, transform.rotation);
        Rigidbody[] rbs = breaked.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbs)
        {
            float force = Random.Range(-1000f,1000f);
            //rb.AddExplosionForce(force, transform.position, 30);
            rb.useGravity = true;
        }
        Destroy(gameObject);
    }
}
