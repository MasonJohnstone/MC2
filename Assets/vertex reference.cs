using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class vertexreference : MonoBehaviour
{
    private Mesh mesh;
    public Material placeholderMaterial;

    public bool r, l, t, b, f, h,
        rtf, mtf, ltf,
        ltm, rtm,
        rth, mth, lth,
        rmf, lmf,
        rmh, lmh,
        rbf, mbf, lbf,
        lbm, rbm,
        rbh, mbh, lbh;




    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Initialize vertices and triangles arrays
        Vector3[] vertices = new Vector3[52]; // 9 vertices per face * 6 faces
        int[] triangles = new int[1008]; // 8 triangles per face * 3 vertices per triangle * 6 faces

        CreateVertices(vertices);
        CreateTriangles(triangles);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); // For correct lighting

        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>(); // Adds a MeshRenderer component if not already present
        renderer.material = placeholderMaterial; // Applies the placeholder material to the mesh
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Initialize vertices and triangles arrays
            Vector3[] vertices = new Vector3[52];
            int[] triangles = new int[1008]; // 8 triangles per face * 3 vertices per triangle * 6 faces

            CreateVertices(vertices);
            CreateTriangles(triangles);

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals(); // For correct lighting

            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>(); // Adds a MeshRenderer component if not already present
            renderer.material = placeholderMaterial; // Applies the placeholder material to the mesh
        }
    }

    void CreateVertices(Vector3[] vertices)
    {
        float z = 0.5f; // block radius
        float x = 0.25f; // min radius
        float y = z - x; // remainder to bring the radius to the boundary

        // (x + (y * (a || b ? 1 : 0)))
        vertices[0] = new Vector3( // lbh
            -(x + (y * (lbh || lbm || l || lmh ? 1 : 0))),
            -(x + (y * (lbh || mbh || b || lbm ? 1 : 0))),
            -(x + (y * (lbh || lmh || h || mbh ? 1 : 0))));
        vertices[1] = new Vector3( // lbm
            -(x + (y * (lbm || lbf || lmf || l || lmh || lbh ? 1 : 0))),
            -(x + (y * (lbm || lbh || mbh || b || mbf || lbf ? 1 : 0))),
            0);
        vertices[2] = new Vector3( // lbf
            -(x + (y * (lbf || lmf || l || lbm ? 1 : 0))),
            -(x + (y * (lbf || lbm || b || mbf ? 1 : 0))),
            (x + (y * (lbf || mbf || f || lmf ? 1 : 0))));
        vertices[3] = new Vector3( // lmh
            -(x + (y * (lmh || lbh || lbm || l || ltm || lth ? 1 : 0))),
            0,
            -(x + (y * (lmh || lth || mth || h || mbh || lbh ? 1 : 0))));
        vertices[4] = new Vector3( // l
            -(x + (y * (l || ltf || ltm || lth || lmh || lbh || lbm || lbf || lmf ? 1 : 0))),
            0,
            0);
        vertices[5] = new Vector3( // lmf
            -(x + (y * (lmf || ltf || ltm || l || lbm || lbf ? 1 : 0))),
            0,
            (x + (y * (lmf || lbf || mbf || f || mtf || ltf ? 1 : 0))));
        vertices[6] = new Vector3( // lth
            -(x + (y * (lth || lmh || l || ltm ? 1 : 0))),
            (x + (y * (lth || ltm || t || mth ? 1 : 0))),
            -(x + (y * (lth || mth || h || lmh ? 1 : 0))));
        vertices[7] = new Vector3( // ltm
            -(x + (y * (ltm || lth || lmh || l || lmf || ltf ? 1 : 0))),
            (x + (y * (ltm || ltf || mtf || t || mth || lth ? 1 : 0))),
            0);
        vertices[8] = new Vector3( // ltf
            -(x + (y * (ltf || ltm || l || lmf ? 1 : 0))),
            (x + (y * (ltf || mtf || t || ltm ? 1 : 0))),
            (x + (y * (ltf || lmf || f || mtf ? 1 : 0))));
        vertices[9] = new Vector3( // mbh
            0,
            -(x + (y * (mbh || rbh || rbm || b || lbm || lbh ? 1 : 0))),
            -(x + (y * (mbh || lbh || lmh || h || rmh || rbh ? 1 : 0))));
        vertices[10] = new Vector3( // b
            0,
            -(x + (y * (b || lbh || mbh || rbh || rbm || rbf || mbf || lbf || lbm ? 1 : 0))),
            0);
        vertices[11] = new Vector3( // mbf
            0,
            -(x + (y * (mbf || lbf || lbm || b || rbm || rbf ? 1 : 0))),
            (x + (y * (mbf || rbf || rmf || f || lmf || lbf ? 1 : 0))));
        vertices[12] = new Vector3( // h
            0,
            0,
            -(x + (y * (h || lth || mth || rth || rmh || rbh || mbh || lbh || lmh ? 1 : 0))));
        //vertices[] = new Vector3(0, 0, 0); center of cube so not useful vert
        vertices[13] = new Vector3( // f
            0,
            0,
            (x + (y * (f || rtf || mtf || ltf || lmf || lbf || mbf || rbf || rmf ? 1 : 0))));
        vertices[14] = new Vector3( // mth
            0,
            (x + (y * (mth || lth || ltm || t || rtm || rth ? 1 : 0))),
            -(x + (y * (mth || rth || rmh || h || lmh || lth ? 1 : 0))));
        vertices[15] = new Vector3( // t
            0,
            (x + (y * (t || ltf || mtf || rtf || rtm || rth || mth || lth || ltm ? 1 : 0))),
            0);
        vertices[16] = new Vector3( // mtf
            0,
            (x + (y * (mtf || rtf || rtm || t || ltm || ltf ? 1 : 0))),
            (x + (y * (mtf || ltf || lmf || f || rmf || rtf ? 1 : 0))));
        vertices[17] = new Vector3( // rbh
            (x + (y * (rbh || rbm || r || rmh ? 1 : 0))),
            -(x + (y * (rbh || mbh || b || rbm ? 1 : 0))),
            -(x + (y * (rbh || rmh || h || mbh ? 1 : 0))));
        vertices[18] = new Vector3( // rbm
            (x + (y * (rbm || rbh || rmh || r || rmf || rbf ? 1 : 0))),
            -(x + (y * (rbm || rbf || mbf || b || mbh || rbh ? 1 : 0))),
            0);
        vertices[19] = new Vector3( // rbf
            (x + (y * (rbf || rmf || r || rbm ? 1 : 0))),
            -(x + (y * (rbf || rbm || b || mbf ? 1 : 0))),
            (x + (y * (rbf || mbf || f || rmf ? 1 : 0))));
        vertices[20] = new Vector3( // rmh
            (x + (y * (rmh || rth || rtm || r || rbm || rbh ? 1 : 0))),
            0,
            -(x + (y * (rmh || rbh || mbh || h || mth || rth ? 1 : 0))));
        vertices[21] = new Vector3( // r
            (x + (y * (r || rth || rtm || rtf || rmf || rbf || rbm || rbh || rmh ? 1 : 0))),
            0,
            0);
        vertices[22] = new Vector3( // rmf
            (x + (y * (rmf || rbf || rbm || r || rtm || rtf ? 1 : 0))),
            0,
            (x + (y * (rmf || rtf || mtf || f || mbf || rbf ? 1 : 0))));
        vertices[23] = new Vector3( // rth
            (x + (y * (rth || rtm || r || rmh ? 1 : 0))),
            (x + (y * (rth || mth || t || rtm ? 1 : 0))),
            -(x + (y * (rth || rmh || h || mth ? 1 : 0))));
        vertices[24] = new Vector3( // rtm
            (x + (y * (rtm || rtf || rmf || r || rmh || rth ? 1 : 0))),
            (x + (y * (rtm || rth || mth || t || mtf || rtf ? 1 : 0))),
            0);
        vertices[25] = new Vector3( // rtf
            (x + (y * (rtf || rmf || r || rtm ? 1 : 0))),
            (x + (y * (rtf || rtm || t || mtf ? 1 : 0))),
            (x + (y * (rtf || mtf || f || rmf ? 1 : 0))));


    }


    void CreateTriangles(int[] triangles)
    {
        // Left face (center vertex: 4)
        int offset = 0; // Start at the beginning of the triangles array
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 4; triangles[offset++] = 8; triangles[offset++] = 7;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 4; triangles[offset++] = 7; triangles[offset++] = 6;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 4; triangles[offset++] = 6; triangles[offset++] = 3;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 4; triangles[offset++] = 3; triangles[offset++] = 0;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 4; triangles[offset++] = 0; triangles[offset++] = 1;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 4; triangles[offset++] = 1; triangles[offset++] = 2;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 4; triangles[offset++] = 2; triangles[offset++] = 5;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        // Right face (center vertex: 21)
        triangles[offset++] = 21; triangles[offset++] = 23; triangles[offset++] = 24;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 21; triangles[offset++] = 24; triangles[offset++] = 25;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 21; triangles[offset++] = 25; triangles[offset++] = 22;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 21; triangles[offset++] = 22; triangles[offset++] = 19;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 21; triangles[offset++] = 19; triangles[offset++] = 18;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 21; triangles[offset++] = 18; triangles[offset++] = 17;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 21; triangles[offset++] = 17; triangles[offset++] = 20;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 21; triangles[offset++] = 20; triangles[offset++] = 23;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        // Bottom face (center vertex: 10)
        triangles[offset++] = 10; triangles[offset++] = 19; triangles[offset++] = 11;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 10; triangles[offset++] = 11; triangles[offset++] = 2;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 10; triangles[offset++] = 2; triangles[offset++] = 1;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 10; triangles[offset++] = 1; triangles[offset++] = 0;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 10; triangles[offset++] = 0; triangles[offset++] = 9;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 10; triangles[offset++] = 9; triangles[offset++] = 17;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 10; triangles[offset++] = 17; triangles[offset++] = 18;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 10; triangles[offset++] = 18; triangles[offset++] = 19;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        // Top face (center vertex: 15)
        triangles[offset++] = 15; triangles[offset++] = 23; triangles[offset++] = 14;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 15; triangles[offset++] = 14; triangles[offset++] = 6;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 15; triangles[offset++] = 6; triangles[offset++] = 7;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 15; triangles[offset++] = 7; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 15; triangles[offset++] = 8; triangles[offset++] = 16;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 15; triangles[offset++] = 16; triangles[offset++] = 25;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 15; triangles[offset++] = 25; triangles[offset++] = 24;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 15; triangles[offset++] = 24; triangles[offset++] = 23;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;


        // Back face (center vertex: 12)
        triangles[offset++] = 12; triangles[offset++] = 6; triangles[offset++] = 14;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 12; triangles[offset++] = 14; triangles[offset++] = 23;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 12; triangles[offset++] = 23; triangles[offset++] = 20;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 12; triangles[offset++] = 20; triangles[offset++] = 17;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 12; triangles[offset++] = 17; triangles[offset++] = 9;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 12; triangles[offset++] = 9; triangles[offset++] = 0;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 12; triangles[offset++] = 0; triangles[offset++] = 3;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 12; triangles[offset++] = 3; triangles[offset++] = 6;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        // Front face (center vertex: 13)
        triangles[offset++] = 13; triangles[offset++] = 25; triangles[offset++] = 16;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 13; triangles[offset++] = 16; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 13; triangles[offset++] = 8; triangles[offset++] = 5;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 13; triangles[offset++] = 5; triangles[offset++] = 2;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 13; triangles[offset++] = 2; triangles[offset++] = 11;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 13; triangles[offset++] = 11; triangles[offset++] = 19;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 13; triangles[offset++] = 19; triangles[offset++] = 22;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;

        triangles[offset++] = 13; triangles[offset++] = 22; triangles[offset++] = 25;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
        triangles[offset++] = 4; triangles[offset++] = 5; triangles[offset++] = 8;
    }

}