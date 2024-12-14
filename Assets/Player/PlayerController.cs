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
        //cam.transform.localEulerAngles = new Vector3(xRotation, 0f, 0f);
        head.transform.localEulerAngles = new Vector3(xRotation, 0f, 0f);

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
                Vector3 targetPosition = hit.point + head.transform.forward * 0.1f; // Project further from hit point
                Vector3 targetPositionAdjusted = targetPosition / (worldController.chunkSize * worldController.voxelSize);

                // Determine chunk position in the world
                Vector3Int chunkPosition = new Vector3Int(
                    Mathf.FloorToInt(targetPositionAdjusted.x),
                    Mathf.FloorToInt(targetPositionAdjusted.y),
                    Mathf.FloorToInt(targetPositionAdjusted.z)
                );

                // Convert chunkPosition from Vector3Int to Vector3
                Vector3 chunkPositionVector = new Vector3(chunkPosition.x, chunkPosition.y, chunkPosition.z);

                // Get the chunk's real-world position
                Vector3 chunkWorldPosition = chunkPositionVector * worldController.chunkSize * worldController.voxelSize;


                // Calculate the voxel position within the chunk
                Vector3 localVoxelPosition = (targetPosition - chunkWorldPosition) / worldController.voxelSize;
                Vector3Int voxelPosition = new Vector3Int(
                    Mathf.RoundToInt(localVoxelPosition.x),
                    Mathf.RoundToInt(localVoxelPosition.y),
                    Mathf.RoundToInt(localVoxelPosition.z)
                );

                // Access the voxel map within the chunk
                Voxel[,,] voxelMap = worldController.chunkDataCache[chunkPosition].voxelMap;

                // Update the voxel at the calculated local position
                voxelMap[voxelPosition.x, voxelPosition.y, voxelPosition.z] = new Voxel { id = 0,  type = Type.none, density = 0f };

                // Update the chunk with the modified voxel map
                worldController.UpdateChunk(worldController.GetChunkAtPosition(chunkPosition), voxelMap);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Physics.Raycast(head.transform.position + new Vector3(0f, 0.225f, 0f), head.transform.forward, out hit);

            if (hit.collider)
            {
                Vector3 targetPosition = hit.point + head.transform.forward * -0.1f; // Project further from hit point
                Vector3 targetPositionAdjusted = targetPosition / (worldController.chunkSize * worldController.voxelSize);

                // Determine chunk position in the world
                Vector3Int chunkPosition = new Vector3Int(
                    Mathf.FloorToInt(targetPositionAdjusted.x),
                    Mathf.FloorToInt(targetPositionAdjusted.y),
                    Mathf.FloorToInt(targetPositionAdjusted.z)
                );

                // Convert chunkPosition from Vector3Int to Vector3
                Vector3 chunkPositionVector = new Vector3(chunkPosition.x, chunkPosition.y, chunkPosition.z);

                // Get the chunk's real-world position
                Vector3 chunkWorldPosition = chunkPositionVector * worldController.chunkSize * worldController.voxelSize;


                // Calculate the voxel position within the chunk
                Vector3 localVoxelPosition = (targetPosition - chunkWorldPosition) / worldController.voxelSize;
                Vector3Int voxelPosition = new Vector3Int(
                    Mathf.RoundToInt(localVoxelPosition.x),
                    Mathf.RoundToInt(localVoxelPosition.y),
                    Mathf.RoundToInt(localVoxelPosition.z)
                );

                // Access the voxel map within the chunk
                Voxel[,,] voxelMap = worldController.chunkDataCache[chunkPosition].voxelMap;

                // Update the voxel at the calculated local position
                voxelMap[voxelPosition.x, voxelPosition.y, voxelPosition.z] = new Voxel { id = 2, type = Type.block, density = 0f };

                // Update the chunk with the modified voxel map
                worldController.UpdateChunk(worldController.GetChunkAtPosition(chunkPosition), voxelMap);
            }
        }


        if (Input.GetKeyDown(KeyCode.G))
        {
            applyGravity = !applyGravity;
        }

        if (Input.GetMouseButtonDown(0))
        {
            
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
