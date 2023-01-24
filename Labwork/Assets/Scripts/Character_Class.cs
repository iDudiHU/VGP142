using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float gravity = 9.81f;
    public float jumpSpeed = 10.0f;

    CharacterController charController;
    Vector3 curMoveInput;

    int errorCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            charController = GetComponent<CharacterController>();
            if (!charController)
                throw new NullReferenceException("character controller not set");

            charController.minMoveDistance = 0.0f;

            if (moveSpeed <= 0.0f)
            {
                moveSpeed = 10.0f;
                throw new UnassignedReferenceException("Speed not set on " + name + "defaulting to " + moveSpeed.ToString());
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.Message);
            errorCounter++;
        }
        catch (UnassignedReferenceException e)
        {
            Debug.Log(e.Message);
            errorCounter++;
        }
        finally
        {
            Debug.Log("The script ran with " + errorCounter.ToString() + "errors.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (charController.isGrounded)
        {
            curMoveInput = transform.TransformDirection(curMoveInput);
        }
        else
        {
            curMoveInput.y -= gravity;
        }
        
        charController.Move(curMoveInput * Time.deltaTime);
        
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            curMoveInput = Vector3.zero;
            return;
        }

        Vector2 move = context.action.ReadValue<Vector2>();
        move.Normalize();

        Vector3 moveDir = new Vector3(move.x, 0, move.y) * moveSpeed;

        curMoveInput = moveDir;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame())
        {
            Debug.Log("fire pressed");
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (charController.isGrounded)
            curMoveInput.y = jumpSpeed;
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
           Debug.Log("Collided with: " + other.name);
           Destroy(other.gameObject);
        }
    }
}
