using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PixelPostprocessing : VolumeComponent, IPostProcessComponent
{
    public bool IsActive()
    {
        return true;
    }

    public bool IsTileCompatible() => true;


    private void Start()
    {
        // Create Shader
    }

}
