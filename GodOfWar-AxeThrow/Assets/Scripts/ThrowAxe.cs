using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class ThrowAxe : MonoBehaviour
{
    private Animator anim;
    private Movement input;
    private Axe axeScript;
    private Rigidbody axeRb;
    private float returnTime;

    public Transform axe;
    public Transform hand;
    public Transform curvePoint;

    public float throwPower = 30;
    public float cameraZoomOffset = .3f;

    private Vector3 origLocPos;
    private Vector3 origLocRot;
    private Vector3 pullPosition;

    public bool walking = true;
    public bool aiming = false;
    public bool hasWeapon = true; 
    public bool pulling = false;

    public CinemachineFreeLook virtualCamera;
    public CinemachineImpulseSource impulseSource;
    void Start()
    {
        anim = GetComponent<Animator>();
        input = GetComponent<Movement>();
        axeRb = axe.GetComponent<Rigidbody>();
        axeScript = axe.GetComponent<Axe>();
        origLocPos = axe.localPosition;
        origLocRot = axe.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (aiming)
        {
           input.RotateToCamera(transform); 
        }
        else
        {
            transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, 0, .2f), transform.eulerAngles.y, transform.eulerAngles.z);
        }

        anim.SetBool("Pulling", pulling);


        if(Input.GetMouseButtonDown(1) && hasWeapon)
        {
            Aim(true,true, 0);
        }
        if(Input.GetMouseButtonUp(1) && hasWeapon)
        {
            Aim(false,true, 0);
        }

        if(hasWeapon)
        {
            if (aiming && Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("Throw");
            } 
        }

        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                WeaponStartPull();
            }
        }

        if(pulling)
        {
            if(returnTime < 1)
            {
                axe.position = GetQuadraticCurvePoint(returnTime, pullPosition, curvePoint.position, hand.position);
                returnTime += Time.deltaTime * 1.5f;
            }

            else
            {
                pulling = false;
                anim.SetBool("Pulling", pulling);
                WeaponCatch();
            }
        }
    }

    void Aim(bool state, bool changeCamera, float delay)
    {
        aiming = state;
        anim.SetBool("Aiming", aiming);
        input.aiming = aiming;
        float newAim = state ? cameraZoomOffset : 0;
        float originalAim = !state ? cameraZoomOffset : 0;
        //DOVirtual.Float(originalAim, newAim, .5f, CameraOffset).SetDelay(delay);
    }

    void WeaponThrow()
    {
        Aim(false, true, 1f);

        hasWeapon = false;
        axeScript.activated = true;
        axeRb.isKinematic = false;
        axeRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        axe.parent = null;
        axe.eulerAngles = new Vector3(0, -90 + transform.eulerAngles.y, 0);
        axe.transform.position += transform.right/5;
        axeRb.AddForce(Camera.main.transform.forward * throwPower + transform.up * 2, ForceMode.Impulse);
    }

    void WeaponStartPull()
    {
        pullPosition = axe.position;
        axeRb.Sleep();
        axeRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        axeRb.isKinematic = true;
        axe.DORotate(new Vector3(-90, -90, 0), .2f).SetEase(Ease.InOutSine);
        axe.DOBlendableLocalRotateBy(Vector3.right * 90, .5f);
        axeScript.activated = true;
        pulling = true;
    }

    void WeaponCatch()
    {
        returnTime = 0;
        axe.parent = hand;
        axeScript.activated = false;
        axe.localEulerAngles = origLocRot;
        axe.localPosition = origLocPos;
        hasWeapon = true;

        impulseSource.GenerateImpulse(Vector3.right);

    }

    public Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }

    void CameraOffset(float offset)
    {
        virtualCamera.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(offset, 1.5f, 0);
        virtualCamera.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(offset, 1.5f, 0);
        virtualCamera.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(offset, 1.5f, 0);
    }
}
