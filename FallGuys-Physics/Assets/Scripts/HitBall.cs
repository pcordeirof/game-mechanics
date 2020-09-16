using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBall : MonoBehaviour
{
    public GameObject Parent;
    Vector3 direction;

    private void OnCollisionEnter(Collision other) 
    {
        direction = Parent.transform.rotation.z < 0 ? new Vector3(-1, 0,0) : new Vector3(1, 0, 0);
        Debug.Log(direction);
        other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(1,0,0));
    }

}
