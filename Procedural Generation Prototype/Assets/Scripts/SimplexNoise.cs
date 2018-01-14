using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//http://catlikecoding.com/unity/tutorials/simplex-noise/
public static class SimplexNoise {
    private static int[] hash = {
        151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
        140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
        247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
         57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
         74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
         60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
         65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
        200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
         52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
        207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
        119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
        129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
        218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
         81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
        184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
        222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180
    };

    private const int hashMask = 255;

    private static float squaresToTriangles = (3f - Mathf.Sqrt(3f)) / 6f; //Converts points from square grid to triangel grid
    private static float trianglesToSquares = (Mathf.Sqrt(3f) - 1f) / 2f; //Converts points from triangle grid to square grid

    //Computes the 1D Simplex value of point
    public static float SimplexValue1D(Vector3 point, float frequency) {
        point *= frequency;
        int ix = Mathf.FloorToInt(point.x);
        float sample = SimplexValue1DPart(point, ix);
        sample += SimplexValue1DPart(point, ix + 1);
        return sample * (2f / hashMask) - 1f;
    }

    public static float SimplexValue2D(Vector3 point, float frequency) {
        point *= frequency;
        float skew = (point.x + point.y) * trianglesToSquares; //Transform the triangle grid to a cube grid
        float sx = point.x + skew;
        float sy = point.y + skew;
        int ix = Mathf.FloorToInt(sx);
        int iy = Mathf.FloorToInt(sy);
        float sample = SimplexValue2DPart(point, ix, iy);
        sample += SimplexValue2DPart(point, ix + 1, iy + 1);
        if (sx - ix >= sy - iy) { // Work out which triangle the point is inside
            sample += SimplexValue2DPart(point, ix + 1, iy);
        } else {
            sample += SimplexValue2DPart(point, ix, iy + 1);
        }
        return sample * (8f * 2f / hashMask) - 1f;
    }

    public static float SimplexValue3D(Vector3 point, float frequency) {
        point *= frequency;
        float skew = (point.x + point.y + point.z) * (1f / 3f);
        float sx = point.x + skew;
        float sy = point.y + skew;
        float sz = point.z + skew;
        int ix = Mathf.FloorToInt(sx);
        int iy = Mathf.FloorToInt(sy);
        int iz = Mathf.FloorToInt(sz);
        float sample = SimplexValue3DPart(point, ix, iy, iz);
        sample += SimplexValue3DPart(point, ix + 1, iy + 1, iz + 1);
        float x = sx - ix;
        float y = sy - iy;
        float z = sz - iz;
        if (x >= y) { //Figure out which tetrahedron we are in
            if (x >= z) {
                sample += SimplexValue3DPart(point, ix + 1, iy, iz);
                if (y >= z) {
                    sample += SimplexValue3DPart(point, ix + 1, iy + 1, iz);
                } else {
                    sample += SimplexValue3DPart(point, ix + 1, iy, iz + 1);
                }
            } else {
                sample += SimplexValue3DPart(point, ix, iy, iz + 1);
                sample += SimplexValue3DPart(point, ix + 1, iy, iz + 1);
            }
        } else {
            if (y >= z) {
                sample += SimplexValue3DPart(point, ix, iy + 1, iz);
                if (x >= z) {
                    sample += SimplexValue3DPart(point, ix + 1, iy + 1, iz);
                } else {
                    sample += SimplexValue3DPart(point, ix, iy + 1, iz + 1);
                }
            } else {
                sample += SimplexValue3DPart(point, ix, iy, iz + 1);
                sample += SimplexValue3DPart(point, ix, iy + 1, iz + 1);
            }
        }
        return sample * (8f * 2f / hashMask) - 1f;
    }


    //Helper function for SimplexValue1D
    private static float SimplexValue1DPart(Vector3 point, int ix) {
        float x = point.x - ix;
        float f = 1f - x * x;
        float f2 = f * f;
        float f3 = f * f2;
        float h = hash[ix & hashMask];
        return f3 * h;
    }

    private static float SimplexValue2DPart(Vector3 point, int ix, int iy) {
        float unskew = (ix + iy) * squaresToTriangles;
        float x = point.x - ix + unskew;
        float y = point.y - iy + unskew;
        float f = 0.5f - x * x - y * y;
        if (f > 0f) {
            float f2 = f * f;
            float f3 = f * f2;
            float h = hash[hash[ix & hashMask] + iy & hashMask];
            return f3 * h;
        }
        return 0;
    }

    private static float SimplexValue3DPart(Vector3 point, int ix, int iy, int iz) {
        float unskew = (ix + iy + iz) * (1f / 6f);
        float x = point.x - ix + unskew;
        float y = point.y - iy + unskew;
        float z = point.z - iz + unskew;
        float f = 0.5f - x * x - y * y - z * z;
        if (f > 0f) {
            float f2 = f * f;
            float f3 = f * f2;
            float h = hash[hash[hash[ix & hashMask] + iy & hashMask] + iz & hashMask];
            return f3 * h;
        }
        return 0;
    }
}
