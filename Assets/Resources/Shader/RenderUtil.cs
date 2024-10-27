using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public static class RenderUtil
{
    static Mesh _blitMesh;
    public static Mesh BlitMesh
    {
        get
        {
            if (_blitMesh == null)
            {
                _blitMesh = new Mesh
                {
                    name = "BlitMesh",
                    vertices = new Vector3[] {
                            new Vector3(-1f, -1f, 0f),
                            new Vector3(-1f,  3f, 0f),
                            new Vector3( 3f, -1f, 0f),
                        },
                    triangles = new int[] { 0, 1, 2 },
                };
                _blitMesh.UploadMeshData(true);
            }
            return _blitMesh;
        }
    }

    // static Material _blitMaterial;
    // public static Material BlitMaterial
    // {
    //     get
    //     {
    //         if (_blitMaterial == null || _blitMaterial.shader == null)
    //             _blitMaterial = new Material(Resources.Load<Shader>("Shader/BlitCopy"));

    //         return _blitMaterial;
    //     }
    // }

    static Material _clearMaterial;
    public static Material ClearMaterial
    {
        get
        {
            if (_clearMaterial == null || _clearMaterial.shader == null)
                _clearMaterial = new Material(Resources.Load<Shader>("Shader/LightClear"));

            return _clearMaterial;
        }
    }
    public static Mesh QuadMesh =>Resources.GetBuiltinResource<Mesh>("unity default resources/Quad");

}