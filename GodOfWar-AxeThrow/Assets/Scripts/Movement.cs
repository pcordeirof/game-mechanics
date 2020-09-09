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

    public bool aiming = false;

    public Vector2 OnMovementInput {get; set;} //não sei o que é isso hahaha
    public Vector3 OnMovementDirectionInput {get; set;}
    
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
        controller = this.GetComponent<CharacterController>();
    }
    
    void Update()
    {
        GetMovementInput();
        GetMovementDirection();
        Rotate();
        if(!aiming)
        {
            PlayerMovement();
            AnimatePlayer();
        }
        //InputMagnitude();
    }

    /*void InputMagnitude()
    {
        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");

        Speed = new Vector2(InputX, InputY).sqrMagnitude;

        if (Speed > allowPlayerRotation)
        {
            //PlayerMove(); // ver sobre como faazer isso sme o if
            //RotateToCamera(transform);
        }
    }*/
    /*public void RotateToCamera(Transform t)
    {

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        desiredMoveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }*/

    /*void PlayerMove()
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
        
    }*/

    void AnimatePlayer()
    {
        Speed = new Vector2(InputX, InputY).sqrMagnitude;

        if(Speed> 0f)
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

    void GetMovementInput()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        OnMovementInput = input;
    }

    void GetMovementDirection()
    {
        var cameraForwardDirection = Camera.main.transform.forward;
        Debug.DrawRay(Camera.main.transform.position, cameraForwardDirection * 10, Color.red);
        var directionToMoveIn = Vector3.Scale(cameraForwardDirection, (Vector3.right + Vector3.forward));
        Debug.DrawRay(Camera.main.transform.position, directionToMoveIn * 10, Color.blue);
        OnMovementDirectionInput = directionToMoveIn.normalized;
    }

    void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(OnMovementDirectionInput), desiredRotationSpeed);
    }

    void PlayerMovement()
    {
        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward * InputY + transform.right * InputX; 

        controller.Move(movement * Time.deltaTime * 3);
    }
}
