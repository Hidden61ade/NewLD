using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class ScanlineSettings
{
    public Material scanlineMaterial;
    public float scanlineIntensity = 0.5f;
    [Range(1, 100)]
    public float scanlineDensity = 30f; // 新增扫描线密度属性
}

public class ScanlineEffect : ScriptableRendererFeature
{
    public ScanlineSettings settings = new ScanlineSettings();

    class ScanlineRenderPass : ScriptableRenderPass
    {
        private Material scanlineMaterial;
        private float scanlineIntensity;
        private float scanlineDensity;

        public ScanlineRenderPass(Material material, float intensity, float density)
        {
            this.scanlineMaterial = material;
            this.scanlineIntensity = intensity;
            this.scanlineDensity = density;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (scanlineMaterial == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("ScanlineEffect");

            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;
            RenderTargetHandle tempRenderTexture = RenderTargetHandle.CameraTarget;

            // 更新材质属性
            scanlineMaterial.SetFloat("_ScanlineIntensity", scanlineIntensity);
            scanlineMaterial.SetFloat("_ScanlineDensity", scanlineDensity);

            cmd.GetTemporaryRT(tempRenderTexture.id, -1, -1, 0, FilterMode.Bilinear);
            cmd.Blit(source, tempRenderTexture.Identifier(), scanlineMaterial);
            cmd.Blit(tempRenderTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    ScanlineRenderPass scanlinePass;

    public override void Create()
    {
        if (settings.scanlineMaterial == null)
        {
            Debug.LogError("Scanline Material is not assigned!");
            return;
        }

        scanlinePass = new ScanlineRenderPass(settings.scanlineMaterial, settings.scanlineIntensity, settings.scanlineDensity);
        scanlinePass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents; // 选择合适的渲染事件
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(scanlinePass);
    }
}