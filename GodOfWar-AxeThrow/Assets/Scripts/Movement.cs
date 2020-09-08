using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float InputX;
    public float InputY;
    public bool blockRotationPlayer;
    public Vector3 desiredMoveDirection;
    public float desiredRotationSpeed = 0.1f;
    public float Speed;
    public float allowPlayerRotation = 0.1f;
    public Animator anim;
    public Camera cam;
    public CharacterController controller;
    
    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();
    }
    
    void Update()
    {
        InputMagnitude();
        AnimatePlayer();
    }

    void InputMagnitude()
    {
        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");

        Speed = new Vector2(InputX, InputY).sqrMagnitude;

        if (Speed > allowPlayerRotation)
        {
            PlayerMove(); // ver sobre como faazer isso sme o if
            RotateToCamera(transform);
        }
    }
    public void RotateToCamera(Transform t)
    {

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        desiredMoveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }

    void PlayerMove()
    {
        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * InputY + right * InputX;

           
        controller.Move(desiredMoveDirection * Time.deltaTime * 3);

    }

    void AnimatePlayer()
    {
        if(Speed> 0.1f)
        {
            anim.SetBool("Walking", true);
            anim.SetFloat("InputY", InputY, VerticalAnimTime, Time.deltaTime * 2f);
		    anim.SetFloat("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }
}
