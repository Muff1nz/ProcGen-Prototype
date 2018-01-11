using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashNoiseTerrain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(generateWorld(10));
	}
	
	private IEnumerator generateWorld(int size) {
        const int speed = 200;
        int count = 0;

        for (int z = -size; z <= size; z++) {
            for (int y = -size; y <= size; y++) {
                for (int x = -size; x <= size; x++) {

                    if (count++ > speed) {
                        count = 0;
                        yield return 0;
                    }

                    Vector3 position = new Vector3(x, y, z) ;
                    if (Noise.hashNoise(position / 4.4f) < 0.5f) {
                        var voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        voxel.transform.position = position;
                    }

                }
            }
        }

    }
}
