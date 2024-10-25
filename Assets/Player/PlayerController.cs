using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    WorldController worldController;
    PhysicsController physicsController;
    float tempGravity = 0.5f;
    public GameObject head;
    public GameObject graphics;

    float moveForce = 4f;
    float jumpForce = 12f;
    bool isAttemptingJump = false;

    float sensitivity = 2f;
    float xRotation = 0f;
    float yRotation = 0f;

    Vector3 groundNormal = Vector3.up;  // Assuming flat ground to start with

    void Start()
    {
        physicsController = gameObject.GetComponent<PhysicsController>();
        physicsController.Init(0.6f, 1.8f, true, true, new Vector3(0f, 1f, 0f));

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Rotate the camera based on mouse input
        yRotation += Input.GetAxisRaw("Mouse X") * sensitivity;
        xRotation -= Mathf.Clamp(Input.GetAxisRaw("Mouse Y") * sensitivity, -100f, 100f);

        // Check if jump is attempted
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAttemptingJump = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Physics.Raycast(head.transform.position + new Vector3(0f, 0.225f, 0f), head.transform.forward, out hit);

            if (hit.collider)
            {
                //WorldController.Break
            }
        }
    }

    void FixedUpdate()
    {
        // Rotate the player visuals
        graphics.transform.localEulerAngles = new Vector3(0f, yRotation, 0f);
        head.transform.localEulerAngles = new Vector3(xRotation, 0f, 0f);

        // Apply gravity force
        ApplyForce(Vector3.down * tempGravity, false);

        // Check if the player is trying to jump
        if (isAttemptingJump)
        {
            isAttemptingJump = false;
            if (physicsController.InContact)  // Only jump if grounded
            {
                ApplyForce(Vector3.up * jumpForce, false);
            }
        }

        // Use the movement keys and GetMoveDirection to move the player
        Vector3 moveVector = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            moveVector += GetMoveDirection(graphics.transform, groundNormal); // Forward
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVector -= GetMoveDirection(graphics.transform, groundNormal); // Backward
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVector += GetRightDirection(graphics.transform, groundNormal); // Right
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVector -= GetRightDirection(graphics.transform, groundNormal); // Left
        }

        // Apply the force for movement
        ApplyForce(moveVector.normalized * moveForce, true);
    }

    // Function to apply forces
    void ApplyForce(Vector3 force, bool useTraction)
    {
        physicsController.ApplyForce(force, useTraction);
    }

    // Function to get the movement direction based on ground normal and player's forward direction
    Vector3 GetMoveDirection(Transform player, Vector3 groundNormal)
    {
        // Project player's forward direction onto the ground plane
        return Vector3.ProjectOnPlane(player.forward, groundNormal).normalized;
    }

    // Function to get the movement direction for strafing (left/right) based on ground normal and player's right direction
    Vector3 GetRightDirection(Transform player, Vector3 groundNormal)
    {
        // Project player's right direction onto the ground plane
        return Vector3.ProjectOnPlane(player.right, groundNormal).normalized;
    }

    public void Initialize(WorldController _worldController)
    {
        worldController = _worldController;
    }
}
