using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Animator anim;
    public CharacterController controller;
    
    public Vector2 OnMovementInput;
    public Vector3 OnMovementDirectionInput;

    private float turnSmoothVelocity;

    bool grounded;
    public Vector3 playerVelocity;
    float gravityValue = -9.81f;  
    float jumpheight = 3f;


    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        grounded = controller.isGrounded;
        GetMovementInput();
        GetMovementDirection();   
        Movement();
        Jump();
     
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

    void Movement()
    {
        if(OnMovementInput.magnitude > 0)
        {
            if(grounded)
            {
                anim.SetBool("Walking", true);
            }
            else
            {
                anim.SetBool("Walking", false);
            }

            float targetAngle = Mathf.Atan2(OnMovementInput.x, OnMovementInput.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;// o angulo do input é local e o da camera é global, mas o do input é de -180 a 180 e o da camera 0 360 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);//não entendi direito esse ref
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //como funciona essa transformação direito 

            controller.Move(moveDir * Time.deltaTime * 3);
        }

        else 
        {
            anim.SetBool("Walking", false);
        }
    }

    void Jump()
    {
        if (grounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }
        if(controller.isGrounded && Input.GetButton("Submit"))
        {
            anim.SetBool("Jumping", true);
            playerVelocity.y += Mathf.Sqrt(jumpheight * -3.0f * gravityValue);
        }

        if(!grounded && playerVelocity.y >=jumpheight)
        {
            anim.SetBool("Jumping", false);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
   
    }
}
