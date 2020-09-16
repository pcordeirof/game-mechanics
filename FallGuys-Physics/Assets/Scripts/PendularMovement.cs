using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendularMovement : MonoBehaviour
{
    public float MaxAngleDeflection;
    public float SpeedOfPendulum;

    void Update()
    {
        float angle = MaxAngleDeflection * Mathf.Sin(Time.time * SpeedOfPendulum);
        transform.localRotation = Quaternion.Euler( 0, 0, angle);
    }
}
