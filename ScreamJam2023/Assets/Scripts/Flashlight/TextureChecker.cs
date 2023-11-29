using UnityEngine;

public class TextureChecker : MonoBehaviour
{
    public RenderTexture targetRenderTexture; // Поле для вашей RenderTexture
    public float colorThreshold = 0.5f; // Порог для определения другого цвета (0.5 = 50% другого цвета)
    //private bool hasReachedThreshold = false; // Флаг для отслеживания достижения порога
    public Material _Material;

    private void Update()
    {
        // if (!hasReachedThreshold && targetRenderTexture != null)
        // {
        //     Debug.Log("texture here");
        //     Texture2D texture2D = RenderTextureToTexture2D(targetRenderTexture);
        //
        //     float nonBlackPixels = CountNonBlackPixels(texture2D);
        //     float totalPixels = texture2D.width * texture2D.height;
        //     float colorPercentage = nonBlackPixels / totalPixels;
        //
        //     if (colorPercentage >= colorThreshold)
        //     {
        //         Debug.Log("Текстура стала 50% другого цвета!");
        //         hasReachedThreshold = true; // Помечаем, что порог достигнут
        //     }
        //
        //     Destroy(texture2D); // Освобождаем ресурсы
        // }
        
        if (_Material.HasProperty("_AlphaToMask"))
        {
            float alphaClipThreshold = _Material.GetFloat("_AlphaToMask");
            Debug.Log("Threshold: " + alphaClipThreshold);
        }
        else
        {
            Debug.LogError("Материал не имеет свойства alphaClipThreshold (_AlphaClip)!");
        }
    }

    // private Texture2D RenderTextureToTexture2D(RenderTexture renderTexture)
    // {
    //     Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height);
    //     RenderTexture.active = renderTexture;
    //     texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
    //     texture2D.Apply();
    //     RenderTexture.active = null;
    //
    //     return texture2D;
    // }
    //
    // private float CountNonBlackPixels(Texture2D texture)
    // {
    //     Color[] pixels = texture.GetPixels();
    //     float nonBlackPixelCount = 0;
    //
    //     foreach (Color pixel in pixels)
    //     {
    //         if (pixel.r > 0.01f || pixel.g > 0.01f || pixel.b > 0.01f)
    //         {
    //             nonBlackPixelCount++;
    //         }
    //     }
    //
    //     return nonBlackPixelCount;
    // }
}