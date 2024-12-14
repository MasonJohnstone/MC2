using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCubes
{
    public void Generate(int vmx, int vmy, int vmz, bool[] cubeOccupations, IList<Vector3> vertList, IList<int> indexList)
    {
        if (cubeOccupations[0] == false)
        {
            return;
        }

        // Helper function to add vertices and indices for a face
        void AddFace(Vector3[] faceVerts)
        {
            int startIndex = vertList.Count; // Starting index for new vertices

            // Add vertices to the main vertex list
            foreach (var vertex in faceVerts)
            {
                vertList.Add(new Vector3(vertex.x + vmx, vertex.y + vmy, vertex.z + vmz)); // Adjust for position
            }

            // Add indices for the two triangles of the face
            indexList.Add(startIndex + 0);
            indexList.Add(startIndex + 1);
            indexList.Add(startIndex + 2);

            indexList.Add(startIndex + 3);
            indexList.Add(startIndex + 4);
            indexList.Add(startIndex + 5);
        }

        // Rear face
        if (cubeOccupations[1] == false)
        {
            Vector3[] rearFace = new Vector3[]
            {
            new Vector3(-0.5f,  0.5f, -0.5f), // Top-left
            new Vector3( 0.5f,  0.5f, -0.5f), // Top-right
            new Vector3( 0.5f, -0.5f, -0.5f), // Bottom-right
            new Vector3(-0.5f,  0.5f, -0.5f), // Top-left (repeated for second triangle)
            new Vector3( 0.5f, -0.5f, -0.5f), // Bottom-right (repeated)
            new Vector3(-0.5f, -0.5f, -0.5f)  // Bottom-left
            };
            AddFace(rearFace);
        }

        // Front face
        if (cubeOccupations[2] == false)
        {
            Vector3[] frontFace = new Vector3[]
            {
            new Vector3(-0.5f, -0.5f,  0.5f), // Top-left
            new Vector3( 0.5f, -0.5f,  0.5f), // Top-right
            new Vector3( 0.5f,  0.5f,  0.5f), // Bottom-right
            new Vector3(-0.5f, -0.5f,  0.5f), // Top-left (repeated for second triangle)
            new Vector3( 0.5f,  0.5f,  0.5f), // Bottom-right (repeated)
            new Vector3(-0.5f,  0.5f,  0.5f)  // Bottom-left
            };
            AddFace(frontFace);
        }

        // Bottom face
        if (cubeOccupations[3] == false)
        {
            Vector3[] bottomFace = new Vector3[]
            {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3(-0.5f, -0.5f,  0.5f)
            };
            AddFace(bottomFace);
        }

        // Top face
        if (cubeOccupations[4] == false)
        {
            Vector3[] topFace = new Vector3[]
            {
            new Vector3(-0.5f,  0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f)
            };
            AddFace(topFace);
        }

        // Left face
        if (cubeOccupations[5] == false)
        {
            Vector3[] leftFace = new Vector3[]
            {
            new Vector3(-0.5f,  0.5f,  0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f,  0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f,  0.5f)
            };
            AddFace(leftFace);
        }

        // Right face
        if (cubeOccupations[6] == false)
        {
            Vector3[] rightFace = new Vector3[]
            {
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f,  0.5f),
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f)
            };
            AddFace(rightFace);
        }
    }

}
