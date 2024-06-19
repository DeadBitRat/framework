using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/PixelBoy")]
public class PixelBoy : MonoBehaviour
{
    public int h = 64;
    int w;
    private RenderTexture intermediateRT;

    void Update()
    {
        float ratio = ((float)Camera.main.pixelWidth) / (float)Camera.main.pixelHeight;
        w = Mathf.RoundToInt(h * ratio);

        if (intermediateRT == null || intermediateRT.width != w || intermediateRT.height != h)
        {
            if (intermediateRT != null)
            {
                intermediateRT.Release();
            }
            intermediateRT = new RenderTexture(w, h, 0);
            intermediateRT.filterMode = FilterMode.Point;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;
        Graphics.Blit(source, intermediateRT);
        Graphics.Blit(intermediateRT, destination);
    }

    public RenderTexture GetIntermediateRT()
    {
        return intermediateRT;
    }
}
