using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[ExecuteAlways]
public class CustomLight : MonoBehaviour
{
    public Texture2D mask;
    public Mesh mesh;
    

    void Awake()
    {
        mesh ??= RenderUtil.QuadMesh;
    }

    void OnEnable()
    {
        LightingSystem.Main?.Add(this);
    }
    void OnDisable()
    {
        LightingSystem.Main?.Remove(this);
    }
}