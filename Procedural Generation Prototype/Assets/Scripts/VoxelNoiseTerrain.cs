using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(VoxelNoiseTerrain))]
public class VoxelNoiseTerrain : MonoBehaviour {

    public float Frequency = 1.5f;
    public int seed = 1337;
    public float MaxHeight = 10.0f;

    public int WorldHeight = 20;
    public int ChunkCount = 10;    
    public int ChunkSize = 10;

    public void killAllCubes() {
        MeshFilter[] cubes = GameObject.FindObjectsOfType<MeshFilter>();
        foreach (var cube in cubes) DestroyImmediate(cube.gameObject);
        Debug.Log("Killed all cubes!");
    }

    public void genWorld() {
        StartCoroutine(generateWorld(ChunkCount));
    }


    private IEnumerator generateWorld(int cs) {
        killAllCubes();
        const int speed = 1;
        int count = 0;

        for (int z = -cs; z <= cs; z++) {
            for (int x = -cs; x <= cs; x++) {

                if (count++ > speed) {
                    Debug.Log("Generating!");
                    count = 0;
                    yield return 0;
                }
                generateChunk(new Vector3(x, 0, z));               
            }
        }
        
        Debug.Log("DONE GENERATING!");
    }

    private  void generateChunk(Vector3 chunkPos) {

        var chunkVoxels = new List<GameObject>();

        for (int y = -1; y < MaxHeight; y++) {
            for (int z = (int)chunkPos.z * ChunkSize; z < (int)chunkPos.z * ChunkSize + ChunkSize; z++) {
                for (int x = (int)chunkPos.x * ChunkSize; x < (int)chunkPos.x * ChunkSize + ChunkSize; x++) {
                    float height = SimplexNoise.SimplexValue2D(new Vector3(x, z, 0) + seed * Vector3.one, Frequency);
                    height = (height + 1f) / 2f;
                    height *= MaxHeight;
                    if (y < height) {
                        var voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        voxel.transform.position = new Vector3(x, y, z);
                        chunkVoxels.Add(voxel);
                    }     
                }
            }
        }

        Material mat = chunkVoxels[0].GetComponent<MeshRenderer>().sharedMaterial;
        CombineInstance[] voxels = new CombineInstance[chunkVoxels.Count];
        //Combine
        for (int i = 0; i < voxels.Length; i++) {
            voxels[i].mesh = chunkVoxels[i].GetComponent<MeshFilter>().sharedMesh;
            voxels[i].transform = chunkVoxels[i].transform.localToWorldMatrix;
            DestroyImmediate(chunkVoxels[i]);
        }
        var chunk = new GameObject();
        chunk.AddComponent<MeshFilter>();
        chunk.AddComponent<MeshRenderer>();
        chunk.GetComponent<MeshRenderer>().material = mat;
        chunk.GetComponent<MeshFilter>().mesh.CombineMeshes(voxels, true, true);
    }
}
