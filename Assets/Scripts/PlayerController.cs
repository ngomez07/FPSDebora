using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    // podemos generar headers para que sea mas visual todo
    [Header("General")]
    public float gravityScale = -20f;

    [Header("Movement")]
    public float walkspeed = 5f;
    public float runspeed = 5f;

    [Header("Rotation")]
    public float rotationSensibility = 30;

    [Header("Jump")]
    public float jumpHeight = 1.9f;

    // variables sin header
    private float cameraVerticalAngle = 0f;
    Vector3 moveInput = Vector3.zero;
    Vector3 rotationInput = Vector3.zero; // variable de rotacion camara
    CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        if (characterController.isGrounded)
        {
            moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            moveInput = Vector3.ClampMagnitude(moveInput, 1f);

            if (Input.GetButton("Sprint"))
            {
                moveInput = transform.TransformDirection(moveInput) * runspeed;
            } else
            {
                moveInput = transform.TransformDirection(moveInput) * walkspeed;
            }
            if (Input.GetButtonDown("Jump"))
            {
                moveInput.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);
            }
        }

        //Debug.Log(moveInput);

        moveInput.y += gravityScale * Time.deltaTime;

        characterController.Move(moveInput*Time.deltaTime);
    }
    private void Look() // hay que ponerla en nuestro update para que no se te olvide
    {
        rotationInput.x = Input.GetAxis("Mouse X") * rotationSensibility * Time.deltaTime;
        rotationInput.y = Input.GetAxis("Mouse Y") * rotationSensibility * Time.deltaTime;

        cameraVerticalAngle = cameraVerticalAngle + rotationInput.y;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -70f, 70f);

        transform.Rotate(Vector3.up * rotationInput.x);
        playerCamera.transform.localRotation = Quaternion.Euler(-cameraVerticalAngle,0f,0f);
    }

}
