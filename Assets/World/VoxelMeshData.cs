using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public static class VoxelMeshData
{
    public static List<Vector3> GenerateVertices
    (
        bool[,,] adjacentVoxels,
        Vector3 offset
    )
    {
        List<Vector3> vertices = new List<Vector3>();

        bool lbh = adjacentVoxels[0, 0, 0];
        bool lbm = adjacentVoxels[0, 0, 1];
        bool lbf = adjacentVoxels[0, 0, 2];
        bool lmh = adjacentVoxels[0, 1, 0];
        bool lmm = adjacentVoxels[0, 1, 1];
        bool lmf = adjacentVoxels[0, 1, 2];
        bool lth = adjacentVoxels[0, 2, 0];
        bool ltm = adjacentVoxels[0, 2, 1];
        bool ltf = adjacentVoxels[0, 2, 2];

        bool mbh = adjacentVoxels[1, 0, 0];
        bool mbm = adjacentVoxels[1, 0, 1];
        bool mbf = adjacentVoxels[1, 0, 2];
        bool mmh = adjacentVoxels[1, 1, 0];
        // bool mmm = adjacentVoxels[1, 1, 1];
        bool mmf = adjacentVoxels[1, 1, 2];
        bool mth = adjacentVoxels[1, 2, 0];
        bool mtm = adjacentVoxels[1, 2, 1];
        bool mtf = adjacentVoxels[1, 2, 2];

        bool rbh = adjacentVoxels[2, 0, 0];
        bool rbm = adjacentVoxels[2, 0, 1];
        bool rbf = adjacentVoxels[2, 0, 2];
        bool rmh = adjacentVoxels[2, 1, 0];
        bool rmm = adjacentVoxels[2, 1, 1];
        bool rmf = adjacentVoxels[2, 1, 2];
        bool rth = adjacentVoxels[2, 2, 0];
        bool rtm = adjacentVoxels[2, 2, 1];
        bool rtf = adjacentVoxels[2, 2, 2];

        // INNER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LBH (0)
        vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f) + offset);
        // LBM (1)
        vertices.Add(new Vector3(-0.5f, -0.5f, 0f) + offset);
        // LBF (2)
        vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f) + offset);
        // LMH (3)
        vertices.Add(new Vector3(-0.5f, 0f, -0.5f) + offset);
        // LMF (4)
        vertices.Add(new Vector3(-0.5f, 0f, 0.5f) + offset);
        // LTH (5)
        vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f) + offset);
        // LTM (6)
        vertices.Add(new Vector3(-0.5f, 0.5f, 0f) + offset);
        // LTF (7)
        vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f) + offset);
        // MBH (8)
        vertices.Add(new Vector3(0f, -0.5f, -0.5f) + offset);
        // MBF (9)
        vertices.Add(new Vector3(0f, -0.5f, 0.5f) + offset);
        // MTH (10)
        vertices.Add(new Vector3(0f, 0.5f, -0.5f) + offset);
        // MTF (11)
        vertices.Add(new Vector3(0f, 0.5f, 0.5f) + offset);
        // RBH (12)
        vertices.Add(new Vector3(0.5f, -0.5f, -0.5f) + offset);
        // RBM (13)
        vertices.Add(new Vector3(0.5f, -0.5f, 0f) + offset);
        // RBF (14)
        vertices.Add(new Vector3(0.5f, -0.5f, 0.5f) + offset);
        // RMH (15)
        vertices.Add(new Vector3(0.5f, 0f, -0.5f) + offset);
        // RMF (16)
        vertices.Add(new Vector3(0.5f, 0f, 0.5f) + offset);
        // RTH (17)
        vertices.Add(new Vector3(0.5f, 0.5f, -0.5f) + offset);
        // RTM (18)
        vertices.Add(new Vector3(0.5f, 0.5f, 0f) + offset);
        // RTF (19)
        vertices.Add(new Vector3(0.5f, 0.5f, 0.5f) + offset);

        // LEFT OUTER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LBH (20)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, -0.5f)
            + offset);
        // LBM (21)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, 0f)
            + Vector3.left * 0.5f * (lbh || lbm || lbf || lmh || lmm || lmf ? 1 : 0)
            + Vector3.down * 0.5f * (lbh || lbm || lbf /*|| mbh || mbm || mbf*/ ? 1 : 0)
            + offset);
        // LBF (22)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, 0.5f)
            + offset);
        // LMH (23)
        vertices.Add
            (new Vector3(-0.5f, 0f, -0.5f)
            + Vector3.left * 0.5f * (lbh || lmh || lth || lbm || lmm || ltm ? 1 : 0)
            + Vector3.back * 0.5f * (lbh || lmh || lth /*|| mbh || mmh || mth*/ ? 1 : 0)
            + offset);
        // LMM (24)
        vertices.Add
            (new Vector3(-0.5f, 0f, 0f)
            + Vector3.left * 0.5f * (lbh || lbm || lbf || lmh || lmm || lmf || lth || ltm || ltf ? 1 : 0)
            + offset);
        // LMF (25)
        vertices.Add
            (new Vector3(-0.5f, 0f, 0.5f)
            + Vector3.left * 0.5f * (lbf || lmf || ltf || lbm || lmm || ltm ? 1 : 0)
            + Vector3.forward * 0.5f * (lbf || lmf || ltf /*|| mbf || mmf || mtf*/ ? 1 : 0)
            + offset);
        // LTH (26)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, -0.5f)
            + offset);
        // LTM (27)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, 0f)
            + Vector3.left * 0.5f * (lth || ltm || ltf || lmh || lmm || lmf ? 1 : 0)
            + Vector3.up * 0.5f * (lth || ltm || ltf /*|| mth || mtm || mtf*/ ? 1 : 0)
            + offset);
        // LTF (28)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, 0.5f)
            + offset);

        // RIGHT OUTER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // RBH (29)
        vertices.Add
            (new Vector3(0.5f, -0.5f, -0.5f)
            + offset);
        // RBM (30)
        vertices.Add
            (new Vector3(0.5f, -0.5f, 0f)
            + Vector3.right * 0.5f * (rbh || rbm || rbf || rmh || rmm || rmf ? 1 : 0)
            + Vector3.down * 0.5f * (rbh || rbm || rbf /*|| mbh || mbm || mbf*/ ? 1 : 0)
            + offset);
        // RBF (31)
        vertices.Add
            (new Vector3(0.5f, -0.5f, 0.5f)
            + offset);
        // RMH (32)
        vertices.Add
            (new Vector3(0.5f, 0f, -0.5f)
            + Vector3.right * 0.5f * (rbh || rmh || rth || rbm || rmm || rtm ? 1 : 0)
            + Vector3.back * 0.5f * (rbh || rmh || rth /*|| mbh || mmh || mth*/ ? 1 : 0)
            + offset);
        // RMM (33)
        vertices.Add
            (new Vector3(0.5f, 0f, 0f)
            + Vector3.right * 0.5f * (rbh || rbm || rbf || rmh || rmm || rmf || rth || rtm || rtf ? 1 : 0)
            + offset);
        // RMF (34)
        vertices.Add
            (new Vector3(0.5f, 0f, 0.5f)
            + Vector3.right * 0.5f * (rbf || rmf || rtf || rbm || rmm || rtm ? 1 : 0)
            + Vector3.forward * 0.5f * (rbf || rmf || rtf /*|| mbf || mmf || mtf*/ ? 1 : 0)
            + offset);
        // RTH (35)
        vertices.Add
            (new Vector3(0.5f, 0.5f, -0.5f)
            + offset);
        // RTM (36)
        vertices.Add
            (new Vector3(0.5f, 0.5f, 0f)
            + Vector3.right * 0.5f * (rth || rtm || rtf || rmh || rmm || rmf ? 1 : 0)
            + Vector3.up * 0.5f * (rth || rtm || rtf /*|| mth || mtm || mtf*/ ? 1 : 0)
            + offset);
        // RTF (37)
        vertices.Add
            (new Vector3(0.5f, 0.5f, 0.5f)
            + offset);

        // BOTTOM OUTER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LBH (38)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, -0.5f)
            + offset);
        // LBM (39)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, 0f)
            + Vector3.left * 0.5f * (lbh || lbm || lbf /*|| lmh || lmm || lmf*/ ? 1 : 0)
            + Vector3.down * 0.5f * (lbh || lbm || lbf || mbh || mbm || mbf ? 1 : 0)
            + offset);
        // LBF (40)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, 0.5f)
            + offset);
        // MBH (41)
        vertices.Add
            (new Vector3(0f, -0.5f, -0.5f)
            + Vector3.down * 0.5f * (lbh || mbh || rbh || lbm || mbm || rbm ? 1 : 0)
            + Vector3.back * 0.5f * (lbh || mbh || rbh /*|| lmh || mmh || rmh*/ ? 1 : 0)
            + offset);
        // MBM (42)
        vertices.Add
            (new Vector3(0f, -0.5f, 0f)
            + Vector3.down * 0.5f * (lbh || lbm || lbf || mbh || mbm || mbf || rbh || rbm || rbf ? 1 : 0)
            + offset);
        // MBF (43)
        vertices.Add
            (new Vector3(0f, -0.5f, 0.5f)
            + Vector3.down * 0.5f * (lbf || mbf || rbf || lbm || mbm || rbm ? 1 : 0)
            + Vector3.forward * 0.5f * (lbf || mbf || rbf /*|| lmf || mmf || rmf*/ ? 1 : 0)
            + offset);
        // RBH (44)
        vertices.Add
            (new Vector3(0.5f, -0.5f, -0.5f)
            + offset);
        // RBM (45)
        vertices.Add
            (new Vector3(0.5f, -0.5f, 0f)
            + Vector3.right * 0.5f * (rbh || rbm || rbf /*|| rmh || rmm || rmf*/ ? 1 : 0)
            + Vector3.down * 0.5f * (rbh || rbm || rbf || mbh || mbm || mbf ? 1 : 0)
            + offset);
        // RBF (46)
        vertices.Add
            (new Vector3(0.5f, -0.5f, 0.5f)
            + offset);


        // TOP OUTER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LTH (47)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, -0.5f)
            + offset);
        // LTM (48)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, 0f)
            + Vector3.left * 0.5f * (lth || ltm || ltf /*|| lmh || lmm || lmf*/ ? 1 : 0)
            + Vector3.up * 0.5f * (lth || ltm || ltf || mth || mtm || mtf ? 1 : 0)
            + offset);
        // LTF (49)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, 0.5f)
            + offset);
        // MTH (50)
        vertices.Add
            (new Vector3(0f, 0.5f, -0.5f)
            + Vector3.up * 0.5f * (lth || mth || rth || ltm || mtm || rtm ? 1 : 0)
            + Vector3.back * 0.5f * (lth || mth || rth /*|| lmh || mmh || rmh*/ ? 1 : 0)
            + offset);
        // MTM (51)
        vertices.Add
            (new Vector3(0f, 0.5f, 0f)
            + Vector3.up * 0.5f * (lth || ltm || ltf || mth || mtm || mtf || rth || rtm || rtf ? 1 : 0)
            + offset);
        // MTF (52)
        vertices.Add
            (new Vector3(0f, 0.5f, 0.5f)
            + Vector3.up * 0.5f * (ltf || mtf || rtf || ltm || mtm || rtm ? 1 : 0)
            + Vector3.forward * 0.5f * (ltf || mtf || rtf /*|| lmf || mmf || rmf*/ ? 1 : 0)
            + offset);
        // RTH (53)
        vertices.Add
            (new Vector3(0.5f, 0.5f, -0.5f)
            + offset);
        // RTM (54)
        vertices.Add
            (new Vector3(0.5f, 0.5f, 0f)
            + Vector3.right * 0.5f * (rth || rtm || rtf /*|| rmh || rmm || rmf*/ ? 1 : 0)
            + Vector3.up * 0.5f * (rth || rtm || rtf || mth || mtm || mtf ? 1 : 0)
            + offset);
        // RTF (55)
        vertices.Add
            (new Vector3(0.5f, 0.5f, 0.5f)
            + offset);


        // BACK OUTER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LBH (56)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, -0.5f)
            + offset);
        // LMH (57)
        vertices.Add
            (new Vector3(-0.5f, 0f, -0.5f)
            + Vector3.left * 0.5f * (lbh || lmh || lth /*|| lbm || lmm || ltm*/ ? 1 : 0)
            + Vector3.back * 0.5f * (lbh || lmh || lth || mbh || mmh || mth ? 1 : 0)
            + offset);
        // LTH (58)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, -0.5f)
            + offset);
        // MBH (59)
        vertices.Add
            (new Vector3(0f, -0.5f, -0.5f)
            + Vector3.down * 0.5f * (lbh || mbh || rbh /*|| lbm || mbm || rbm*/ ? 1 : 0)
            + Vector3.back * 0.5f * (lbh || mbh || rbh || lmh || mmh || rmh ? 1 : 0)
            + offset);
        // MMH (60)
        vertices.Add
            (new Vector3(0f, 0f, -0.5f)
            + Vector3.back * 0.5f * (lbh || lmh || lth || mbh || mmh || mth || rbh || rmh || rth ? 1 : 0)
            + offset);
        // MTH (61)
        vertices.Add
            (new Vector3(0f, 0.5f, -0.5f)
            + Vector3.up * 0.5f * (lth || mth || rth /*|| ltm || mtm || rtm*/ ? 1 : 0)
            + Vector3.back * 0.5f * (lth || mth || rth || lmh || mmh || rmh ? 1 : 0)
            + offset);
        // RBH (62)
        vertices.Add
            (new Vector3(0.5f, -0.5f, -0.5f)
            + offset);
        // RMH (63)
        vertices.Add
            (new Vector3(0.5f, 0f, -0.5f)
            + Vector3.right * 0.5f * (rbh || rmh || rth /*|| rbm || rmm || rtm*/ ? 1 : 0)
            + Vector3.back * 0.5f * (rbh || rmh || rth || mbh || mmh || mth ? 1 : 0)
            + offset);
        // RTH (64)
        vertices.Add
            (new Vector3(0.5f, 0.5f, -0.5f)
            + offset);

        // FRONT OUTER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LBF (65)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, 0.5f)
            + offset);
        // LMF (66)
        vertices.Add
            (new Vector3(-0.5f, 0f, 0.5f)
            + Vector3.left * 0.5f * (lbf || lmf || ltf /*|| lbm || lmm || ltm*/ ? 1 : 0)
            + Vector3.forward * 0.5f * (lbf || lmf || ltf || mbf || mmf || mtf ? 1 : 0)
            + offset);
        // LTF (67)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, 0.5f)
            + offset);
        // MBF (68)
        vertices.Add
            (new Vector3(0f, -0.5f, 0.5f)
            + Vector3.down * 0.5f * (lbf || mbf || rbf /*|| lbm || mbm || rbm*/ ? 1 : 0)
            + Vector3.forward * 0.5f * (lbf || mbf || rbf || lmf || mmf || rmf ? 1 : 0)
            + offset);
        // MMF (69)
        vertices.Add
            (new Vector3(0f, 0f, 0.5f)
            + Vector3.forward * 0.5f * (lbf || lmf || ltf || mbf || mmf || mtf || rbf || rmf || rtf ? 1 : 0)
            + offset);
        // MTF (70)
        vertices.Add
            (new Vector3(0f, 0.5f, 0.5f)
            + Vector3.up * 0.5f * (ltf || mtf || rtf /*|| ltm || mtm || rtm*/ ? 1 : 0)
            + Vector3.forward * 0.5f * (ltf || mtf || rtf || lmf || mmf || rmf ? 1 : 0)
            + offset);
        // RBF (71)
        vertices.Add
            (new Vector3(0.5f, -0.5f, 0.5f)
            + offset);
        // RMF (72)
        vertices.Add
            (new Vector3(0.5f, 0f, 0.5f)
            + Vector3.right * 0.5f * (rbf || rmf || rtf /*|| rbm || rmm || rtm*/ ? 1 : 0)
            + Vector3.forward * 0.5f * (rbf || rmf || rtf || mbf || mmf || mtf ? 1 : 0)
            + offset);
        // RTF (73)
        vertices.Add
            (new Vector3(0.5f, 0.5f, 0.5f)
            + offset);



        return vertices;
    }

    public static List<Vector3> GenerateSoftVertices
    (
        bool[,,] adjacentVoxels,
        Vector3 offset
    )
    {
        List<Vector3> vertices = new List<Vector3>();

        bool lbh = adjacentVoxels[0, 0, 0];
        bool lbm = adjacentVoxels[0, 0, 1];
        bool lbf = adjacentVoxels[0, 0, 2];
        bool lmh = adjacentVoxels[0, 1, 0];
        bool lmm = adjacentVoxels[0, 1, 1];
        bool lmf = adjacentVoxels[0, 1, 2];
        bool lth = adjacentVoxels[0, 2, 0];
        bool ltm = adjacentVoxels[0, 2, 1];
        bool ltf = adjacentVoxels[0, 2, 2];

        bool mbh = adjacentVoxels[1, 0, 0];
        bool mbm = adjacentVoxels[1, 0, 1];
        bool mbf = adjacentVoxels[1, 0, 2];
        bool mmh = adjacentVoxels[1, 1, 0];
        // bool mmm = adjacentVoxels[1, 1, 1];
        bool mmf = adjacentVoxels[1, 1, 2];
        bool mth = adjacentVoxels[1, 2, 0];
        bool mtm = adjacentVoxels[1, 2, 1];
        bool mtf = adjacentVoxels[1, 2, 2];

        bool rbh = adjacentVoxels[2, 0, 0];
        bool rbm = adjacentVoxels[2, 0, 1];
        bool rbf = adjacentVoxels[2, 0, 2];
        bool rmh = adjacentVoxels[2, 1, 0];
        bool rmm = adjacentVoxels[2, 1, 1];
        bool rmf = adjacentVoxels[2, 1, 2];
        bool rth = adjacentVoxels[2, 2, 0];
        bool rtm = adjacentVoxels[2, 2, 1];
        bool rtf = adjacentVoxels[2, 2, 2];

        float deformation = 0.125f;

        // INNER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LBH (0)
        vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f)
            - Vector3.left * deformation * (!lbh && !mbh && !lmh && !lbm && !lmm && (!mbm || !mmh) ? 1 : 0)
            - Vector3.down * deformation * (!lbh && !mbh && !lmh && !lbm && !mbm && (!lmm || !mmh) ? 1 : 0)
            - Vector3.back * deformation * (!lbh && !mbh && !lmh && !lbm && !mmh && (!lmm || !mbm) ? 1 : 0)
            + offset);
        // LBM (1)
        vertices.Add(new Vector3(-0.5f, -0.5f, 0f) 
            - (Vector3.left * deformation + Vector3.down * deformation) * (!lbm && !lmm && !mbm ? 1 : 0)
            + offset);
        // LBF (2)
        vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f) + offset);
        // LMH (3)
        vertices.Add(new Vector3(-0.5f, 0f, -0.5f)
            - (Vector3.left * deformation + Vector3.back * deformation) * (!lmh && !lmm && !mmh ? 1 : 0)
            + offset);
        // LMF (4)
        vertices.Add(new Vector3(-0.5f, 0f, 0.5f)
            - (Vector3.left * deformation + Vector3.forward * deformation) * (!lmf && !lmm && !mmf ? 1 : 0) 
            + offset);
        // LTH (5)
        vertices.Add(new Vector3(-0.5f, 0.5f, -0.5f) + offset);
        // LTM (6)
        vertices.Add(new Vector3(-0.5f, 0.5f, 0f)
            - (Vector3.left * deformation + Vector3.up * deformation) * (!ltm && !lmm && !mtm ? 1 : 0) 
            + offset);
        // LTF (7)
        vertices.Add(new Vector3(-0.5f, 0.5f, 0.5f) + offset);
        // MBH (8)
        vertices.Add(new Vector3(0f, -0.5f, -0.5f)
            - (Vector3.down * deformation + Vector3.back * deformation) * (!mbh && !mbm && !mmh ? 1 : 0) 
            + offset);
        // MBF (9)
        vertices.Add(new Vector3(0f, -0.5f, 0.5f)
            - (Vector3.down * deformation + Vector3.forward * deformation) * (!mbf && !mbm && !mmf ? 1 : 0) 
            + offset);
        // MTH (10)
        vertices.Add(new Vector3(0f, 0.5f, -0.5f)
            - (Vector3.up * deformation + Vector3.back * deformation) * (!mth && !mtm && !mmh ? 1 : 0) 
            + offset);
        // MTF (11)
        vertices.Add(new Vector3(0f, 0.5f, 0.5f)
            - (Vector3.up * deformation + Vector3.forward * deformation) * (!mtf && !mtm && !mmf ? 1 : 0) 
            + offset);
        // RBH (12)
        vertices.Add(new Vector3(0.5f, -0.5f, -0.5f) + offset);
        // RBM (13)
        vertices.Add(new Vector3(0.5f, -0.5f, 0f)
            - (Vector3.right * deformation + Vector3.down * deformation) * (!rbm && !rmm && !mbm ? 1 : 0) 
            + offset);
        // RBF (14)
        vertices.Add(new Vector3(0.5f, -0.5f, 0.5f) + offset);
        // RMH (15)
        vertices.Add(new Vector3(0.5f, 0f, -0.5f)
            - (Vector3.right * deformation + Vector3.back * deformation) * (!rmh && !rmm && !mmh ? 1 : 0) 
            + offset);
        // RMF (16)
        vertices.Add(new Vector3(0.5f, 0f, 0.5f)
            - (Vector3.right * deformation + Vector3.forward * deformation) * (!rmf && !rmm && !mmf ? 1 : 0) 
            + offset);
        // RTH (17)
        vertices.Add(new Vector3(0.5f, 0.5f, -0.5f) + offset);
        // RTM (18)
        vertices.Add(new Vector3(0.5f, 0.5f, 0f)
            - (Vector3.right * deformation + Vector3.up * deformation) * (!rtm && !rmm && !mtm ? 1 : 0) 
            + offset);
        // RTF (19)
        vertices.Add(new Vector3(0.5f, 0.5f, 0.5f) + offset);

        // LEFT OUTER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LBH (20)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, -0.5f)
            - Vector3.left * deformation * (!lbh && !mbh && !lmh && !lbm && !lmm && (!mbm || !mmh) ? 1 : 0)
            - Vector3.down * deformation * (!lbh && !mbh && !lmh && !lbm && !mbm && (!lmm || !mmh) ? 1 : 0)
            - Vector3.back * deformation * (!lbh && !mbh && !lmh && !lbm && !mmh && (!lmm || !mbm) ? 1 : 0)
            + offset);
        // LBM (21)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, 0f)
            + Vector3.left * 0.5f * (lbh || lbm || lbf || lmh || lmm || lmf ? 1 : 0)
            + Vector3.down * 0.5f * (lbh || lbm || lbf /*|| mbh || mbm || mbf*/ ? 1 : 0)
            - (Vector3.left * deformation + Vector3.down * deformation) * (!lbm && !lmm && !mbm ? 1 : 0)
            + offset);
        // LBF (22)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, 0.5f)
            + offset);
        // LMH (23)
        vertices.Add
            (new Vector3(-0.5f, 0f, -0.5f)
            + Vector3.left * 0.5f * (lbh || lmh || lth || lbm || lmm || ltm ? 1 : 0)
            + Vector3.back * 0.5f * (lbh || lmh || lth /*|| mbh || mmh || mth*/ ? 1 : 0)
            - (Vector3.left * deformation + Vector3.back * deformation) * (!lmh && !lmm && !mmh ? 1 : 0)
            + offset);
        // LMM (24)
        vertices.Add
            (new Vector3(-0.5f, 0f, 0f)
            + Vector3.left * 0.5f * (lbh || lbm || lbf || lmh || lmm || lmf || lth || ltm || ltf ? 1 : 0)
            + offset);
        // LMF (25)
        vertices.Add
            (new Vector3(-0.5f, 0f, 0.5f)
            + Vector3.left * 0.5f * (lbf || lmf || ltf || lbm || lmm || ltm ? 1 : 0)
            + Vector3.forward * 0.5f * (lbf || lmf || ltf /*|| mbf || mmf || mtf*/ ? 1 : 0)
            - (Vector3.left * deformation + Vector3.forward * deformation) * (!lmf && !lmm && !mmf ? 1 : 0)
            + offset);
        // LTH (26)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, -0.5f)
            + offset);
        // LTM (27)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, 0f)
            + Vector3.left * 0.5f * (lth || ltm || ltf || lmh || lmm || lmf ? 1 : 0)
            + Vector3.up * 0.5f * (lth || ltm || ltf /*|| mth || mtm || mtf*/ ? 1 : 0)
            - (Vector3.left * deformation + Vector3.up * deformation) * (!ltm && !lmm && !mtm ? 1 : 0)
            + offset);
        // LTF (28)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, 0.5f)
            + offset);

        // RIGHT OUTER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // RBH (29)
        vertices.Add
            (new Vector3(0.5f, -0.5f, -0.5f)
            + offset);
        // RBM (30)
        vertices.Add
            (new Vector3(0.5f, -0.5f, 0f)
            + Vector3.right * 0.5f * (rbh || rbm || rbf || rmh || rmm || rmf ? 1 : 0)
            + Vector3.down * 0.5f * (rbh || rbm || rbf /*|| mbh || mbm || mbf*/ ? 1 : 0)
            - (Vector3.right * deformation + Vector3.down * deformation) * (!rbm && !rmm && !mbm ? 1 : 0)
            + offset);
        // RBF (31)
        vertices.Add
            (new Vector3(0.5f, -0.5f, 0.5f)
            + offset);
        // RMH (32)
        vertices.Add
            (new Vector3(0.5f, 0f, -0.5f)
            + Vector3.right * 0.5f * (rbh || rmh || rth || rbm || rmm || rtm ? 1 : 0)
            + Vector3.back * 0.5f * (rbh || rmh || rth /*|| mbh || mmh || mth*/ ? 1 : 0)
            - (Vector3.right * deformation + Vector3.back * deformation) * (!rmh && !rmm && !mmh ? 1 : 0)
            + offset);
        // RMM (33)
        vertices.Add
            (new Vector3(0.5f, 0f, 0f)
            + Vector3.right * 0.5f * (rbh || rbm || rbf || rmh || rmm || rmf || rth || rtm || rtf ? 1 : 0)
            + offset);
        // RMF (34)
        vertices.Add
            (new Vector3(0.5f, 0f, 0.5f)
            + Vector3.right * 0.5f * (rbf || rmf || rtf || rbm || rmm || rtm ? 1 : 0)
            + Vector3.forward * 0.5f * (rbf || rmf || rtf /*|| mbf || mmf || mtf*/ ? 1 : 0)
            - (Vector3.right * deformation + Vector3.forward * deformation) * (!rmf && !rmm && !mmf ? 1 : 0)
            + offset);
        // RTH (35)
        vertices.Add
            (new Vector3(0.5f, 0.5f, -0.5f)
            + offset);
        // RTM (36)
        vertices.Add
            (new Vector3(0.5f, 0.5f, 0f)
            + Vector3.right * 0.5f * (rth || rtm || rtf || rmh || rmm || rmf ? 1 : 0)
            + Vector3.up * 0.5f * (rth || rtm || rtf /*|| mth || mtm || mtf*/ ? 1 : 0)
            - (Vector3.right * deformation + Vector3.up * deformation) * (!rtm && !rmm && !mtm ? 1 : 0)
            + offset);
        // RTF (37)
        vertices.Add
            (new Vector3(0.5f, 0.5f, 0.5f)
            + offset);

        // BOTTOM OUTER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LBH (38)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, -0.5f)
            - Vector3.left * deformation * (!lbh && !mbh && !lmh && !lbm && !lmm && (!mbm || !mmh) ? 1 : 0)
            - Vector3.down * deformation * (!lbh && !mbh && !lmh && !lbm && !mbm && (!lmm || !mmh) ? 1 : 0)
            - Vector3.back * deformation * (!lbh && !mbh && !lmh && !lbm && !mmh && (!lmm || !mbm) ? 1 : 0)
            + offset);
        // LBM (39)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, 0f)
            + Vector3.left * 0.5f * (lbh || lbm || lbf /*|| lmh || lmm || lmf*/ ? 1 : 0)
            + Vector3.down * 0.5f * (lbh || lbm || lbf || mbh || mbm || mbf ? 1 : 0)
            - (Vector3.left * deformation + Vector3.down * deformation) * (!lbm && !lmm && !mbm ? 1 : 0)
            + offset);
        // LBF (40)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, 0.5f)
            + offset);
        // MBH (41)
        vertices.Add
            (new Vector3(0f, -0.5f, -0.5f)
            + Vector3.down * 0.5f * (lbh || mbh || rbh || lbm || mbm || rbm ? 1 : 0)
            + Vector3.back * 0.5f * (lbh || mbh || rbh /*|| lmh || mmh || rmh*/ ? 1 : 0)
            - (Vector3.down * deformation + Vector3.back * deformation) * (!mbh && !mbm && !mmh ? 1 : 0)
            + offset);
        // MBM (42)
        vertices.Add
            (new Vector3(0f, -0.5f, 0f)
            + Vector3.down * 0.5f * (lbh || lbm || lbf || mbh || mbm || mbf || rbh || rbm || rbf ? 1 : 0)
            + offset);
        // MBF (43)
        vertices.Add
            (new Vector3(0f, -0.5f, 0.5f)
            + Vector3.down * 0.5f * (lbf || mbf || rbf || lbm || mbm || rbm ? 1 : 0)
            + Vector3.forward * 0.5f * (lbf || mbf || rbf /*|| lmf || mmf || rmf*/ ? 1 : 0)
            - (Vector3.down * deformation + Vector3.forward * deformation) * (!mbf && !mbm && !mmf ? 1 : 0)
            + offset);
        // RBH (44)
        vertices.Add
            (new Vector3(0.5f, -0.5f, -0.5f)
            + offset);
        // RBM (45)
        vertices.Add
            (new Vector3(0.5f, -0.5f, 0f)
            + Vector3.right * 0.5f * (rbh || rbm || rbf /*|| rmh || rmm || rmf*/ ? 1 : 0)
            + Vector3.down * 0.5f * (rbh || rbm || rbf || mbh || mbm || mbf ? 1 : 0)
            - (Vector3.right * deformation + Vector3.down * deformation) * (!rbm && !rmm && !mbm ? 1 : 0)
            + offset);
        // RBF (46)
        vertices.Add
            (new Vector3(0.5f, -0.5f, 0.5f)
            + offset);


        // TOP OUTER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LTH (47)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, -0.5f)
            + offset);
        // LTM (48)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, 0f)
            + Vector3.left * 0.5f * (lth || ltm || ltf /*|| lmh || lmm || lmf*/ ? 1 : 0)
            + Vector3.up * 0.5f * (lth || ltm || ltf || mth || mtm || mtf ? 1 : 0)
            - (Vector3.left * deformation + Vector3.up * deformation) * (!ltm && !lmm && !mtm ? 1 : 0)
            + offset);
        // LTF (49)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, 0.5f)
            + offset);
        // MTH (50)
        vertices.Add
            (new Vector3(0f, 0.5f, -0.5f)
            + Vector3.up * 0.5f * (lth || mth || rth || ltm || mtm || rtm ? 1 : 0)
            + Vector3.back * 0.5f * (lth || mth || rth /*|| lmh || mmh || rmh*/ ? 1 : 0)
            - (Vector3.up * deformation + Vector3.back * deformation) * (!mth && !mtm && !mmh ? 1 : 0)
            + offset);
        // MTM (51)
        vertices.Add
            (new Vector3(0f, 0.5f, 0f)
            + Vector3.up * 0.5f * (lth || ltm || ltf || mth || mtm || mtf || rth || rtm || rtf ? 1 : 0)
            + offset);
        // MTF (52)
        vertices.Add
            (new Vector3(0f, 0.5f, 0.5f)
            + Vector3.up * 0.5f * (ltf || mtf || rtf || ltm || mtm || rtm ? 1 : 0)
            + Vector3.forward * 0.5f * (ltf || mtf || rtf /*|| lmf || mmf || rmf*/ ? 1 : 0)
            - (Vector3.up * deformation + Vector3.forward * deformation) * (!mtf && !mtm && !mmf ? 1 : 0)
            + offset);
        // RTH (53)
        vertices.Add
            (new Vector3(0.5f, 0.5f, -0.5f)
            + offset);
        // RTM (54)
        vertices.Add
            (new Vector3(0.5f, 0.5f, 0f)
            + Vector3.right * 0.5f * (rth || rtm || rtf /*|| rmh || rmm || rmf*/ ? 1 : 0)
            + Vector3.up * 0.5f * (rth || rtm || rtf || mth || mtm || mtf ? 1 : 0)
            - (Vector3.right * deformation + Vector3.up * deformation) * (!rtm && !rmm && !mtm ? 1 : 0)
            + offset);
        // RTF (55)
        vertices.Add
            (new Vector3(0.5f, 0.5f, 0.5f)
            + offset);


        // BACK OUTER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LBH (56)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, -0.5f)
            - Vector3.left * deformation * (!lbh && !mbh && !lmh && !lbm && !lmm && (!mbm || !mmh) ? 1 : 0)
            - Vector3.down * deformation * (!lbh && !mbh && !lmh && !lbm && !mbm && (!lmm || !mmh) ? 1 : 0)
            - Vector3.back * deformation * (!lbh && !mbh && !lmh && !lbm && !mmh && (!lmm || !mbm) ? 1 : 0)
            + offset);
        // LMH (57)
        vertices.Add
            (new Vector3(-0.5f, 0f, -0.5f)
            + Vector3.left * 0.5f * (lbh || lmh || lth /*|| lbm || lmm || ltm*/ ? 1 : 0)
            + Vector3.back * 0.5f * (lbh || lmh || lth || mbh || mmh || mth ? 1 : 0)
            - (Vector3.left * deformation + Vector3.back * deformation) * (!lmh && !lmm && !mmh ? 1 : 0)
            + offset);
        // LTH (58)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, -0.5f)
            + offset);
        // MBH (59)
        vertices.Add
            (new Vector3(0f, -0.5f, -0.5f)
            + Vector3.down * 0.5f * (lbh || mbh || rbh /*|| lbm || mbm || rbm*/ ? 1 : 0)
            + Vector3.back * 0.5f * (lbh || mbh || rbh || lmh || mmh || rmh ? 1 : 0)
            - (Vector3.down * deformation + Vector3.back * deformation) * (!mbh && !mbm && !mmh ? 1 : 0)
            + offset);
        // MMH (60)
        vertices.Add
            (new Vector3(0f, 0f, -0.5f)
            + Vector3.back * 0.5f * (lbh || lmh || lth || mbh || mmh || mth || rbh || rmh || rth ? 1 : 0)
            + offset);
        // MTH (61)
        vertices.Add
            (new Vector3(0f, 0.5f, -0.5f)
            + Vector3.up * 0.5f * (lth || mth || rth /*|| ltm || mtm || rtm*/ ? 1 : 0)
            + Vector3.back * 0.5f * (lth || mth || rth || lmh || mmh || rmh ? 1 : 0)
            - (Vector3.up * deformation + Vector3.back * deformation) * (!mth && !mtm && !mmh ? 1 : 0)
            + offset);
        // RBH (62)
        vertices.Add
            (new Vector3(0.5f, -0.5f, -0.5f)
            + offset);
        // RMH (63)
        vertices.Add
            (new Vector3(0.5f, 0f, -0.5f)
            + Vector3.right * 0.5f * (rbh || rmh || rth /*|| rbm || rmm || rtm*/ ? 1 : 0)
            + Vector3.back * 0.5f * (rbh || rmh || rth || mbh || mmh || mth ? 1 : 0)
            - (Vector3.right * deformation + Vector3.back * deformation) * (!rmh && !rmm && !mmh ? 1 : 0)
            + offset);
        // RTH (64)
        vertices.Add
            (new Vector3(0.5f, 0.5f, -0.5f)
            + offset);

        // FRONT OUTER ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LBF (65)
        vertices.Add
            (new Vector3(-0.5f, -0.5f, 0.5f)
            + offset);
        // LMF (66)
        vertices.Add
            (new Vector3(-0.5f, 0f, 0.5f)
            + Vector3.left * 0.5f * (lbf || lmf || ltf /*|| lbm || lmm || ltm*/ ? 1 : 0)
            + Vector3.forward * 0.5f * (lbf || lmf || ltf || mbf || mmf || mtf ? 1 : 0)
            - (Vector3.left * deformation + Vector3.forward * deformation) * (!lmf && !lmm && !mmf ? 1 : 0)
            + offset);
        // LTF (67)
        vertices.Add
            (new Vector3(-0.5f, 0.5f, 0.5f)
            + offset);
        // MBF (68)
        vertices.Add
            (new Vector3(0f, -0.5f, 0.5f)
            + Vector3.down * 0.5f * (lbf || mbf || rbf /*|| lbm || mbm || rbm*/ ? 1 : 0)
            + Vector3.forward * 0.5f * (lbf || mbf || rbf || lmf || mmf || rmf ? 1 : 0)
            - (Vector3.down * deformation + Vector3.forward * deformation) * (!mbf && !mbm && !mmf ? 1 : 0)
            + offset);
        // MMF (69)
        vertices.Add
            (new Vector3(0f, 0f, 0.5f)
            + Vector3.forward * 0.5f * (lbf || lmf || ltf || mbf || mmf || mtf || rbf || rmf || rtf ? 1 : 0)
            + offset);
        // MTF (70)
        vertices.Add
            (new Vector3(0f, 0.5f, 0.5f)
            + Vector3.up * 0.5f * (ltf || mtf || rtf /*|| ltm || mtm || rtm*/ ? 1 : 0)
            + Vector3.forward * 0.5f * (ltf || mtf || rtf || lmf || mmf || rmf ? 1 : 0)
            - (Vector3.up * deformation + Vector3.forward * deformation) * (!mtf && !mtm && !mmf ? 1 : 0)
            + offset);
        // RBF (71)
        vertices.Add
            (new Vector3(0.5f, -0.5f, 0.5f)
            + offset);
        // RMF (72)
        vertices.Add
            (new Vector3(0.5f, 0f, 0.5f)
            + Vector3.right * 0.5f * (rbf || rmf || rtf /*|| rbm || rmm || rtm*/ ? 1 : 0)
            + Vector3.forward * 0.5f * (rbf || rmf || rtf || mbf || mmf || mtf ? 1 : 0)
            - (Vector3.right * deformation + Vector3.forward * deformation) * (!rmf && !rmm && !mmf ? 1 : 0)
            + offset);
        // RTF (73)
        vertices.Add
            (new Vector3(0.5f, 0.5f, 0.5f)
            + offset);



        return vertices;
    }


    public static List<int> GenerateTriangles
    (
        bool[,,] adjacentVoxels
    )
    {
        bool lbr = adjacentVoxels[0, 0, 0];
        bool lbm = adjacentVoxels[0, 0, 1];
        bool lbf = adjacentVoxels[0, 0, 2];
        bool lmr = adjacentVoxels[0, 1, 0];
        bool lmm = adjacentVoxels[0, 1, 1];
        bool lmf = adjacentVoxels[0, 1, 2];
        bool ltr = adjacentVoxels[0, 2, 0];
        bool ltm = adjacentVoxels[0, 2, 1];
        bool ltf = adjacentVoxels[0, 2, 2];

        bool mbr = adjacentVoxels[1, 0, 0];
        bool mbm = adjacentVoxels[1, 0, 1];
        bool mbf = adjacentVoxels[1, 0, 2];
        bool mmr = adjacentVoxels[1, 1, 0];
        // bool mmm = adjacentVoxels[1, 1, 1];
        bool mmf = adjacentVoxels[1, 1, 2];
        bool mtr = adjacentVoxels[1, 2, 0];
        bool mtm = adjacentVoxels[1, 2, 1];
        bool mtf = adjacentVoxels[1, 2, 2];

        bool rbr = adjacentVoxels[2, 0, 0];
        bool rbm = adjacentVoxels[2, 0, 1];
        bool rbf = adjacentVoxels[2, 0, 2];
        bool rmr = adjacentVoxels[2, 1, 0];
        bool rmm = adjacentVoxels[2, 1, 1];
        bool rmf = adjacentVoxels[2, 1, 2];
        bool rtr = adjacentVoxels[2, 2, 0];
        bool rtm = adjacentVoxels[2, 2, 1];
        bool rtf = adjacentVoxels[2, 2, 2];

        List<int> tris = new List<int>();

        void AddTriangle(int v1, int v2, int v3)
        {
            tris.Add(v1);
            tris.Add(v2);
            tris.Add(v3);
        }

        // left face
        // face
        AddTriangle(24, 28, 27);
        AddTriangle(24, 27, 26);
        AddTriangle(24, 26, 23);
        AddTriangle(24, 23, 20);
        AddTriangle(24, 20, 21);
        AddTriangle(24, 21, 22);
        AddTriangle(24, 22, 25);
        AddTriangle(24, 25, 28);
        // walls
        // top (top)
        AddTriangle(27, 28, 7);
        AddTriangle(27, 7, 6);
        AddTriangle(27, 6, 5);
        AddTriangle(27, 5, 26);
        // front (left)
        AddTriangle(25, 22, 2);
        AddTriangle(25, 2, 4);
        AddTriangle(25, 4, 7);
        AddTriangle(25, 7, 28);
        // back (right)
        AddTriangle(23, 26, 5);
        AddTriangle(23, 5, 3);
        AddTriangle(23, 3, 0);
        AddTriangle(23, 0, 20);
        // bottom (bottom)
        AddTriangle(21, 20, 0);
        AddTriangle(21, 0, 1);
        AddTriangle(21, 1, 2);
        AddTriangle(21, 2, 22);

        // right face
        // face
        AddTriangle(33,35,36);
        AddTriangle(33,36,37);
        AddTriangle(33,37,34);
        AddTriangle(33,34,31);
        AddTriangle(33,31,30);
        AddTriangle(33,30,29);
        AddTriangle(33,29,32);
        AddTriangle(33,32,35);
        // walls
        // top (top)
        AddTriangle(36,35,17);
        AddTriangle(36,17,18);
        AddTriangle(36,18,19);
        AddTriangle(36,19,37);
        // forward (right)
        AddTriangle(34,37,19);
        AddTriangle(34,19,16);
        AddTriangle(34,16,14);
        AddTriangle(34,14,31);
        // bottom (bottom)
        AddTriangle(30,31,14);
        AddTriangle(30,14,13);
        AddTriangle(30,13,12);
        AddTriangle(30,12,29);
        // back (left)
        AddTriangle(32,29,12);
        AddTriangle(32,12,15);
        AddTriangle(32,15,17);
        AddTriangle(32,17,35);

        // bottom face
        // face
        AddTriangle(42,38,41);
        AddTriangle(42,41,44);
        AddTriangle(42,44,45);
        AddTriangle(42,45,46);
        AddTriangle(42,46,43);
        AddTriangle(42,43,40);
        AddTriangle(42,40,39);
        AddTriangle(42,39,38);
        // walls
        // back (top)
        AddTriangle(41,38,0);
        AddTriangle(41,0,8);
        AddTriangle(41,8,12);
        AddTriangle(41,12,44);
        // right (right)
        AddTriangle(45,44,12);
        AddTriangle(45,12,13);
        AddTriangle(45,13,14);
        AddTriangle(45,14,46);
        // front (bototm)
        AddTriangle(43,46,14);
        AddTriangle(43,14,9);
        AddTriangle(43,9,2);
        AddTriangle(43,2,40);
        // left (left)
        AddTriangle(39,40,2);
        AddTriangle(39,2,1);
        AddTriangle(39,1,0);
        AddTriangle(39,0,38);

        // top face
        // face
        AddTriangle(51,49,52);
        AddTriangle(51,52,55);
        AddTriangle(51,55,54);
        AddTriangle(51,54,53);
        AddTriangle(51,53,50);
        AddTriangle(51,50,47);
        AddTriangle(51,47,48);
        AddTriangle(51,48,49);
        // walls
        // front (top)
        AddTriangle(52,49,7);
        AddTriangle(52,7,11);
        AddTriangle(52,11,19);
        AddTriangle(52,19,55);
        // right (right)
        AddTriangle(54,55,19);
        AddTriangle(54,19,18);
        AddTriangle(54,18,17);
        AddTriangle(54,17,53);
        // back (bottom)
        AddTriangle(50,53,17);
        AddTriangle(50,17,10);
        AddTriangle(50,10,5);
        AddTriangle(50,5,47);
        // left (left)
        AddTriangle(48,47,5);
        AddTriangle(48,5,6);
        AddTriangle(48,6,7);
        AddTriangle(48,7,49);

        // back face
        // face
        AddTriangle(60,58,61);
        AddTriangle(60,61,64);
        AddTriangle(60,64,63);
        AddTriangle(60,63,62);
        AddTriangle(60,62,59);
        AddTriangle(60,59,56);
        AddTriangle(60,56,57);
        AddTriangle(60,57,58);
        // walls
        // top (top)
        AddTriangle(61,58,5);
        AddTriangle(61,5,10);
        AddTriangle(61,10,17);
        AddTriangle(61,17,64);
        // rright (right)
        AddTriangle(63,64,17);
        AddTriangle(63,17,15);
        AddTriangle(63,15,12);
        AddTriangle(63,12,62);
        // bottom (bottom)
        AddTriangle(59,62,12);
        AddTriangle(59,12,8);
        AddTriangle(59,8,0);
        AddTriangle(59,0,56);
        // left (left)
        AddTriangle(57,56,0);
        AddTriangle(57,0,3);
        AddTriangle(57,3,5);
        AddTriangle(57,5,58);

        // front face
        // face
        AddTriangle(69,65,68);
        AddTriangle(69,68,71);
        AddTriangle(69,71,72);
        AddTriangle(69,72,73);
        AddTriangle(69,73,70);
        AddTriangle(69,70,67);
        AddTriangle(69,67,66);
        AddTriangle(69,66,65);
        // walls
        // bottom (top)
        AddTriangle(68,65,2);
        AddTriangle(68,2,9);
        AddTriangle(68,9,14);
        AddTriangle(68,14,71);
        // right (right)
        AddTriangle(72,71,14);
        AddTriangle(72,14,16);
        AddTriangle(72,16,19);
        AddTriangle(72,19,73);
        // top (bottom)
        AddTriangle(70,73,19);
        AddTriangle(70,19,11);
        AddTriangle(70,11,7);
        AddTriangle(70,7,67);
        // left (left)
        AddTriangle(66,67,7);
        AddTriangle(66,7,4);
        AddTriangle(66,4,2);
        AddTriangle(66,2,65);

        return tris;
    }

}