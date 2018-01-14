using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VoxelNoiseTerrain))]
public class VoxelNoiseTerrain : MonoBehaviour {

    public float Frequency = 1.5f;
    public int seed = 1337;
    public float MaxHeight = 10.0f;
    public float MassCutoff = 0.5f;
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

    //MD = mass densitiy, instead of looking at the noise as height, it's looked at as where voxels should and shouldn't be.
    public void gen3DSimplexValueMD() {
        StartCoroutine(generateWorld3DSimplexValueMD(worldSize));
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
                Debug.Log("Generating!");
                count = 0;
                yield return 0;
            }

            var voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            voxel.transform.localScale = new Vector3(1, 50, 1);
            voxels.Add(voxel);
            float height = SimplexNoise.SimplexValue1D(new Vector3(x, 0, 0) + seed * Vector3.one, Frequency) * MaxHeight;
            voxel.transform.position = new Vector3(x, height);            
        }
        Debug.Log("DONE GENERATING!");
    }

    private IEnumerator generateWorld2DSimplexValueHM(int size) {
        clearVoxels();
        const int speed = 400;
        int count = 0;

        for (int z = -size; z <= size; z++) {
            for (int x = -size; x <= size; x++) {

                if (count++ > speed) {
                    Debug.Log("Generating!");
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
        Debug.Log("DONE GENERATING!");
    }

    private IEnumerator generateWorld3DSimplexValueMD(int size) {
        clearVoxels();
        const int speed = 400;
        int count = 0;

        for (int y = -size; y <= size; y++) {
            for (int z = -size; z <= size; z++) {
                for (int x = -size; x <= size; x++) {

                    if (count++ > speed) {
                        Debug.Log("Generating!");
                        count = 0;
                        yield return 0;
                    }

                    float massValue = SimplexNoise.SimplexValue3D(new Vector3(x, y, z) + seed * Vector3.one, Frequency);
                    massValue = (massValue + 1) / 2;

                    if (massValue > MassCutoff) {
                        var voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        voxels.Add(voxel);
                        voxel.transform.position = new Vector3(x, y, z);
                    }
                }
            }
        }
        Debug.Log("DONE GENERATING!");
    }
}
