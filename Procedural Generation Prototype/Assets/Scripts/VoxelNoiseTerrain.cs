using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VoxelNoiseTerrain))]
public class VoxelNoiseTerrain : MonoBehaviour {

    public float Frequency = 1.5f;
    public int seed = 1337;
    public float MaxHeight = 10.0f;

    private List<GameObject> voxels = new List<GameObject>();

    public void killAllCubes() {
        clearVoxels();
        MeshFilter[] cubes = GameObject.FindObjectsOfType<MeshFilter>();
        foreach (var cube in cubes) DestroyImmediate(cube.gameObject);
    }

    public void gen1DSimplex() {
        StartCoroutine(generateWorld1DSimplex(20));
    }
	
    private void clearVoxels() {
        foreach (var voxel in voxels) DestroyImmediate(voxel);
        voxels.Clear();
    }

	private IEnumerator generateWorld1DSimplex(int size) {
        clearVoxels();
        const int speed = 200;
        int count = 0;
       
        for (int x = -size; x <= size; x++) {

            if (count++ > speed) {
                count = 0;
                yield return 0;
            }

            
            var voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            voxels.Add(voxel);
            float height = SimplexNoise.SimplexValue1D(new Vector3(x, 0, 0) + seed * Vector3.one, Frequency) * MaxHeight;
            voxel.transform.position = new Vector3(x, height);            
        }
    }
}
