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
    public GameObject cam;

    float moveForce = 4f;
    float jumpForce = 12f;
    bool isAttemptingJump = false;

    float sensitivity = 2f;
    float xRotation = 0f;
    float yRotation = 0f;

    Vector3 groundNormal = Vector3.up;  // Assuming flat ground to start with

    bool applyGravity = false;

    void Start()
    {
        physicsController = gameObject.GetComponent<PhysicsController>();
        physicsController.Init(0.6f, 1.8f, true, true, new Vector3(0f, 1f, 0f));

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Init(WorldController _worldController)
    {
        worldController = _worldController;
    }

    void Update()
    {
        // Rotate the camera based on mouse input
        yRotation += Input.GetAxisRaw("Mouse X") * sensitivity;
        xRotation -= Mathf.Clamp(Input.GetAxisRaw("Mouse Y") * sensitivity, -100f, 100f);
        // Rotate the player visuals
        graphics.transform.localEulerAngles = new Vector3(0f, yRotation, 0f);
        cam.transform.localEulerAngles = new Vector3(xRotation, 0f, 0f);
        head.transform.localEulerAngles = new Vector3(xRotation / 2f, 0f, 0f);

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

        if (Input.GetKeyDown(KeyCode.G))
        {
            applyGravity = !applyGravity;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Physics.Raycast();
        }
    }

    void FixedUpdate()
    {
        // Apply gravity force
        if (applyGravity)
            ApplyForce(Vector3.down * tempGravity, false, true);

        // Check if the player is trying to jump
        if (isAttemptingJump)
        {
            isAttemptingJump = false;
            if (physicsController.InContact)  // Only jump if grounded
            {
                ApplyForce(Vector3.up * jumpForce, false, false);
            }
        }

        RaycastHit ground;
        if (Physics.CapsuleCast(transform.position + (Vector3.down * (1.8f / 2f - 0.6f / 2f)), transform.position + (Vector3.up * (1.8f / 2f - 0.6f / 2f)), 0.6f / 2f, Vector3.down, out ground, 0.11f))
        {
            groundNormal = ground.normal;
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
        ApplyForce(moveVector.normalized * moveForce, true, false);
    }

    // Function to apply forces
    void ApplyForce(Vector3 force, bool useTraction, bool useFriction)
    {
        physicsController.ApplyForce(force, useTraction, useFriction);
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
