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

        if (GUILayout.Button("GenerateTerrain")) {
            VNT.genWorld();
        }

        if (GUILayout.Button("Kill all cubes")) {
            VNT.killAllCubes();
        }
    }
}
