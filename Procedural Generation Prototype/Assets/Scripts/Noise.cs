using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise {

    //From looking at varius noise functions
    // it looks like they're pretty much based on 
    // some hash function, input coordinates and interpolation.
    //this hash based noise function is based on https://www.shadertoy.com/view/XslGRr made by Inigo Quilez

    private static float frac(float n) {
        return n - Mathf.Floor(n);
    }

    private static Vector3 frac(Vector3 n) {
        return new Vector3(frac(n.x), frac(n.y), frac(n.z));
    }

    private static Vector3 floor(Vector3 x) {
        return new Vector3(Mathf.Floor(x.x), Mathf.Floor(x.y), Mathf.Floor(x.z));
    }

    public static float hash(float n) {
        return frac(Mathf.Abs(Mathf.Sin(n) * 43758.5453f));
    }

    public static float hashNoise(Vector3 x) {
        Vector3 p = floor(x);
        Vector3 f = frac(x);
        f.Scale(f);
        f = (3.0f * Vector3.one - 2.0f * f);
        float n = p.x + p.y * 57.0f + 113.0f * p.z;
        //Interpolate hash values at the corners of the integral units cube
        float x1 = Mathf.Lerp(hash(n + 0.0f), hash(n + 1.0f), f.x);
        float x2 = Mathf.Lerp(hash(n + 57), hash(n + 58), f.x);
        float y1 = Mathf.Lerp(x1, x2, f.y);

        float x3 = Mathf.Lerp(hash(n + 113), hash(n + 114), f.x);
        float x4 = Mathf.Lerp(hash(n + 170), hash(n + 171), f.x);
        float y2 = Mathf.Lerp(x3, x4, f.y);

        return Mathf.Lerp(y1, y2, f.z);
    }

    /*
        A HLSL port of ingio quilez's noise function written in GLSL i found in a unity forum
        https://forum.unity.com/threads/perlin-noise-procedural-shader.33725/

    float hash( float n )
    {
        return frac(sin(n)*43758.5453);
    }

    float noise( Vector3 x )
    {
        // The noise function returns a value in the range -1.0f -> 1.0f

        Vector3 p = floor(x);
        Vector3 f = frac(x);

        f       = f*f*(3.0-2.0*f);
        float n = p.x + p.y*57.0 + 113.0*p.z;

        return Mathf.Lerp(Mathf.Lerp(Mathf.Lerp( hash(n+0.0), hash(n+1.0),f.x),
                       Mathf.Lerp( hash(n+57.0), hash(n+58.0),f.x),f.y),
                   Mathf.Lerp(Mathf.Lerp( hash(n+113.0), hash(n+114.0),f.x),
                       Mathf.Lerp( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
    }
    */
}
