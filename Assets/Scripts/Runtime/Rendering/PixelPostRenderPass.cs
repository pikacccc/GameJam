using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

internal class PxielPostRenderFeture : ScriptableRenderPass
{
    // 给profiler入一个新的事件
    private ProfilingSampler m_ProfilingSampler = new ProfilingSampler("Pixel");
    private Material m_Material;
    // RTHandle，封装了纹理及相关信息，可以认为是CPU端纹理
    private RTHandle m_CameraColorTarget;
    private RTHandle m_CopiedColor;

    private float m_Intensity;
    public bool m_CopyActiveColor = true;

    public PxielPostRenderFeture(Material material)
    {
        profilingSampler = new ProfilingSampler("Pixelize");
        m_Material = material;
        // 指定执行这个Pass的时机
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    // 指定进行后处理的target
    public void SetTarget(RTHandle colorHandle, float intensity)
    {
        m_CameraColorTarget = colorHandle;
        m_Intensity = intensity;
    }

    // OnCameraSetup是纯虚函数，相机初始化时调用
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        // (父类函数)指定pass的render target
        // ConfigureTarget(m_CameraColorTarget);
        if (m_CopiedColor == null)
            ReAllocate(renderingData.cameraData.cameraTargetDescriptor);

    }
    internal void ReAllocate(RenderTextureDescriptor desc)
    {
        desc.msaaSamples = 1;
        desc.depthBufferBits = (int)DepthBits.None;
        RenderingUtils.ReAllocateIfNeeded(ref m_CopiedColor, desc, name: "_FullscreenPassColorCopy");
    }
    private static MaterialPropertyBlock s_SharedPropertyBlock = new MaterialPropertyBlock();

    // Execute时抽象函数，把cmd命令添加到context中（然后进一步送到GPU调用）
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var cameraData = renderingData.cameraData;
        if (cameraData.cameraType != CameraType.Game)
            return;

        if (m_Material == null)
            return;

        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, profilingSampler))
        {
            CoreUtils.SetRenderTarget(cmd, m_CopiedColor);
            Blitter.BlitTexture(cmd, m_CameraColorTarget, new Vector4(1, 1, 0, 0), 0.0f, false);
            m_Material.SetTexture("_MainTex", m_CopiedColor);
            m_Material.SetFloat("_Intensity", m_Intensity);
            Blitter.BlitCameraTexture(cmd, m_CopiedColor, m_CameraColorTarget, m_Material, 0);
        }
        // 把cmd中的命令入到context中
        context.ExecuteCommandBuffer(cmd);
        // 清空cmd栈
        cmd.Clear();

        CommandBufferPool.Release(cmd);
    }
}