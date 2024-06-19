using UnityEngine;

[ExecuteInEditMode]
public class CombinedEffectsManager : MonoBehaviour
{
    private PixelBoy pixelBoy;
    private OLDTVTube oldTVTube;
    private Camera mainCamera;

    void Start()
    {
        pixelBoy = GetComponent<PixelBoy>();
        oldTVTube = GetComponent<OLDTVTube>();
        mainCamera = GetComponent<Camera>();

        if (pixelBoy == null || oldTVTube == null || mainCamera == null)
        {
            Debug.LogError("Missing required components.");
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (pixelBoy != null && oldTVTube != null)
        {
            // Apply PixelBoy effect to an intermediate render texture
            RenderTexture pixelBoyRT = pixelBoy.GetIntermediateRT();
            if (pixelBoyRT == null)
            {
                Graphics.Blit(source, destination);
                return;
            }

            Graphics.Blit(source, pixelBoyRT);

            // Apply OLDTVTube effect to the result of PixelBoy effect
            oldTVTube.OnRenderImage(pixelBoyRT, destination);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
