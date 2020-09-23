using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    float Speed;
    public Animator anim;
    //public Camera cam;
    public CharacterController controller;
    float desiredRotationSpeed = 0.1f;

    public float HorizontalAnimSmoothTime = 0.2f;
    public float VerticalAnimTime = 0.2f;
    public bool aiming = false;

    public Vector2 OnMovementInput;
    public Vector3 OnMovementDirectionInput;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

    }

    void Update()
    {
        GetMovementInput();
        GetMovementDirection();
        Rotate();
        if(!aiming)
        {
            AnimatePlayer(); //como suavizar a animação
            PlayerMovement();
        }
    }

    void GetMovementInput()
    {
        OnMovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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
        Vector3 movement = transform.forward * OnMovementInput.y + transform.right * OnMovementInput.x;

        controller.Move(movement * 0.5f * Time.deltaTime * 3);
    }

    void AnimatePlayer()
    {
        Speed = new Vector2(OnMovementInput.x, OnMovementInput.y).sqrMagnitude;

        if(Speed > 0f)
        {
            anim.SetBool("Walking", true);
            anim.SetFloat("InputZ", OnMovementInput.y, VerticalAnimTime, Time.deltaTime);
            anim.SetFloat("InputX", OnMovementInput.x, HorizontalAnimSmoothTime, Time.deltaTime);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }
}
