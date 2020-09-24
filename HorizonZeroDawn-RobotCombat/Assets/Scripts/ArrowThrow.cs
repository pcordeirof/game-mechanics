using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using Cinemachine;

public class ArrowThrow : MonoBehaviour
{
    Animator anim;
    CharacterMovement input;

    public float throwPower = 30;
    public float cameraZoomOffset = .3f;

    public bool walking = true;
    public bool aiming = false;

    public CinemachineFreeLook virtualCamera;

    void Start()
    {
        anim = GetComponent<Animator>();
        input = GetComponent<CharacterMovement>();
    }

    
    void Update()
    {
        if(!aiming  && Input.GetMouseButtonDown(1))
        {
            Aim(true,true, 0);
        }
    }

    void Aim(bool state, bool changeCamera, float delay)
    {
        aiming = state;
        anim.SetBool("Aiming", aiming);
        input.aiming = aiming;

        float newAim = state ? cameraZoomOffset : 0;
        float originalAim = !state ? cameraZoomOffset : 0;
        DOVirtual.Float(originalAim, newAim, .5f, CameraOffset).SetDelay(delay);

    }

    void ThrowArrow()
    {
        Aim(false, true, 1f);
        anim.SetBool("Aiming", aiming);
    }

    void CameraOffset(float offset)
    {
        virtualCamera.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(offset, 1.5f, 0);
        virtualCamera.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(offset, 1.5f, 0);
        virtualCamera.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = new Vector3(offset, 1.5f, 0);
    }
}
