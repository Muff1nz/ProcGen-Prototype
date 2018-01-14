using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VoxelNoiseTerrain))]
public class VoxelNoiseEditor : Editor {

    VoxelNoiseTerrain VNT;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        VNT = (VoxelNoiseTerrain)target;

        if (GUILayout.Button("1D Simplex Value HM")) {
            VNT.gen1DSimplexValueHM();
        }

        if (GUILayout.Button("2D Simplex Value HM")) {
            VNT.gen2DSimplexValueHM();
        }

        if (GUILayout.Button("3D Simplex Value MD")) {
            VNT.gen3DSimplexValueMD();
        }

        if (GUILayout.Button("Kill all cubes")) {
            VNT.killAllCubes();
        }
    }
}
