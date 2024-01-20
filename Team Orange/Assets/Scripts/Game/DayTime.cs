using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTime : MonoBehaviour
{   
    public static bool day = true;
    Camera cam;
    Light l;
    [SerializeField] private float transitionTime = 5f;
    [SerializeField] private float lowIntensity = 0.2f;

    void Awake()
    {
        day = true;
        l = GameObject.Find("NaturalLight").GetComponent<Light>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (day) Day();
        else Night();
    }

    private void Night() {
        l.intensity -= (1-lowIntensity) / transitionTime * Time.deltaTime;
        if (l.intensity <= lowIntensity) 
        {
            l.intensity = lowIntensity;
            cam.clearFlags = CameraClearFlags.SolidColor;
        }
        
        if (l.intensity < (0.2f + 0.8f * lowIntensity))
            PlaneMovement.lightsOn();
    }

    private void Day() {
        cam.clearFlags = CameraClearFlags.Skybox;

        l.intensity += (1-lowIntensity) / transitionTime * Time.deltaTime;
        if (l.intensity > 1)
            l.intensity = 1;
        
        if (l.intensity > (0.5f + 0.5f * lowIntensity))
            PlaneMovement.lightsOff();
    }
    
    public static void toggleDay() {
        day = !day;
    } 
}
