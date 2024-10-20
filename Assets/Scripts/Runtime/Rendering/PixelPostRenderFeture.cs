using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

internal class PixelPostRenderFeture : ScriptableRendererFeature
{
    //public Shader m_Shader;
    public float m_Intensity;
    public Texture2D m_Dither;
    public Material m_Material;

    private PxielPostRenderFeture m_RenderPass = null;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
            // Pass入队
            renderer.EnqueuePass(m_RenderPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        // 只对游戏摄像机应用后处理（还有预览摄像机等）
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            // 设置向pass输入color (m_RenderPass父类)
            m_RenderPass.ConfigureInput(ScriptableRenderPassInput.Color);
            // 设置RT为相机的color
            m_RenderPass.SetTarget(renderer.cameraColorTargetHandle, m_Intensity);
        }
    }

    // 基类的抽象函数 OnEnable和OnValidate时调用
    public override void Create()
    {
        // 创建BiltPass脚本实例
        m_RenderPass = new PxielPostRenderFeture(m_Material);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(m_Material);
    }
}