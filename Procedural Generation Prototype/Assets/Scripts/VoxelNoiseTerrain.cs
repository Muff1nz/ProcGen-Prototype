using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VoxelNoiseTerrain))]
public class VoxelNoiseTerrain : MonoBehaviour {

    public float Frequency = 1.5f;
    public int seed = 1337;
    public float MaxHeight = 10.0f;
    public int worldSize = 100;

    private List<GameObject> voxels = new List<GameObject>();

    public void killAllCubes() {
        clearVoxels();
        MeshFilter[] cubes = GameObject.FindObjectsOfType<MeshFilter>();
        foreach (var cube in cubes) DestroyImmediate(cube.gameObject);
        Debug.Log("Killed all cubes!");
    }

    public void gen1DSimplexValueHM() {
        StartCoroutine(generateWorld1DSimplexValueHM(worldSize));
    }

    public void gen2DSimplexValueHM() {
        StartCoroutine(generateWorld2DSimplexValueHM(worldSize));
    }
	
    private void clearVoxels() {
        foreach (var voxel in voxels) DestroyImmediate(voxel);
        voxels.Clear();
    }

	private IEnumerator generateWorld1DSimplexValueHM(int size) {
        clearVoxels();
        const int speed = 200;
        int count = 0;
       
        for (int x = -size; x <= size; x++) {

            if (count++ > speed) {
                count = 0;
                yield return 0;
            }

            var voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            voxel.transform.localScale = new Vector3(1, 50, 1);
            voxels.Add(voxel);
            float height = SimplexNoise.SimplexValue1D(new Vector3(x, 0, 0) + seed * Vector3.one, Frequency) * MaxHeight;
            voxel.transform.position = new Vector3(x, height);            
        }
    }

    private IEnumerator generateWorld2DSimplexValueHM(int size) {
        clearVoxels();
        const int speed = 400;
        int count = 0;

        for (int z = -size; z <= size; z++) {
            for (int x = -size; x <= size; x++) {

                if (count++ > speed) {
                    count = 0;
                    yield return 0;
                }

                var voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                voxel.transform.localScale = new Vector3(1, 50, 1);
                voxels.Add(voxel);
                float height = SimplexNoise.SimplexValue2D(new Vector3(x, z, 0) + seed * Vector3.one, Frequency) * MaxHeight;
                voxel.transform.position = new Vector3(x, height, z);
            }
        }
    }
}
