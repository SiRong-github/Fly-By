using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class Fog : MonoBehaviour
{   
    public static bool fogEnabled;
    public Material fogMat;
    public Material noFogMat;
    private Material currFogMat;
    public Color fogColour;
    public static float fogCoeff = 0.05f;

    void Start() {
        if(fogMat == null) {
            enabled = false;
            Debug.Log("null material");
            return;
        }

        fogEnabled = false;
    }

    void Update() {
        if(fogEnabled) {
            currFogMat = fogMat;
        } else {
            currFogMat = noFogMat;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        fogMat.SetFloat("_FogCoeff", fogCoeff);
        fogMat.SetVector("_FogColour", fogColour);
        Graphics.Blit(src, dest, currFogMat);
    }

    public static void toggleFog() {
        fogEnabled = !fogEnabled;
    }
}
