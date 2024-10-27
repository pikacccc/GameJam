using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
[ExecuteAlways]
public class LightingSystem : MonoBehaviour
{
    public RenderTexture Lightmap;
    public Material customMaterial;
    private Camera mainCamera;
    private CommandBuffer commandBuffer;
     List<CustomLight> renderers = new List<CustomLight>();
     HashSet<CustomLight> distinctLight = new HashSet<CustomLight>();

    public Color ClearColor = new Color(0, 0, 0, 0.5f);
    public static LightingSystem Main;
    public bool Debug = false;


    public void Add(CustomLight light)
    {
        if(distinctLight.Add(light))
        {
            renderers.Add(light);
        }
    }
    public void Remove(CustomLight light)
    {
        if(distinctLight.Remove(light))
        {
            renderers.Remove(light);
        }
    }

    

    void OnDisable()
    {
        if (Main == this)
        {
            Main = null;
        }
        if (mainCamera != null && commandBuffer != null)
        {
            mainCamera.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, commandBuffer);
        }
    }
    void OnEnable()
    {

        Main = this;
        if (commandBuffer == null)
        {
            commandBuffer = new CommandBuffer();
            commandBuffer.name = "CustomRendererCollector";
        }
        // 收集所有需要绘制的渲染器
        CollectRenderers();
        // 添加 CommandBuffer 到摄像机
        mainCamera = GetComponent<Camera>();
        mainCamera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, commandBuffer);
    }

    void CollectRenderers()
    {
        renderers.Clear();
        // 收集场景中的所有 MeshRenderer
        renderers.AddRange(FindObjectsOfType<CustomLight>());
        // 收集场景中的所有 SkinnedMeshRenderer
        // renderers.AddRange(FindObjectsOfType<SkinnedMeshRenderer>());
    }

    void UpdateCommandBuffer()
    {
        commandBuffer.Clear();
        commandBuffer.SetRenderTarget(Lightmap);
        commandBuffer.SetGlobalColor("_ClearColor", ClearColor);
        commandBuffer.DrawMesh(RenderUtil.BlitMesh, Matrix4x4.identity, RenderUtil.ClearMaterial);
        foreach (var renderer in renderers)
        {
            if (renderer != null && renderer.enabled && renderer.mesh)
            {
                // 获取渲染器的网格和变换矩阵
                // Mesh mesh = renderer.GetComponent<MeshFilter>()?.sharedMesh;
                // if (mesh == null) continue;
                commandBuffer.SetGlobalTexture("_Mask", renderer.mask);
                Matrix4x4 matrix = renderer.transform.localToWorldMatrix;
                commandBuffer.DrawMesh(renderer.mesh, matrix, customMaterial, 0, 0);
            }
        }
        commandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
        if(Debug)
        {
            commandBuffer.Blit(Lightmap, BuiltinRenderTextureType.CameraTarget);
        }
    }

    void LateUpdate()
    {
        // 每帧更新 CommandBuffer
        UpdateCommandBuffer();
    }
}