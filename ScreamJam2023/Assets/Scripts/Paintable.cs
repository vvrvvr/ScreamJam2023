using UnityEngine;
using Lighter;
using DG.Tweening;

public class Paintable : MonoBehaviour {
    const int TEXTURE_SIZE = 1024;

    public float extendsIslandOffset = 1;

    RenderTexture extendIslandsRenderTexture;
    RenderTexture uvIslandsRenderTexture;
    RenderTexture maskRenderTexture;
    RenderTexture supportTexture;
    
    //dissolve and destroy
    private bool isStartDissolve = false;
    [HideInInspector] public float paintTime = 0;
    public float maxPaintTime = 2.5f;
    private Material mat;          // Ссылка на материал, который вы хотите анимировать
    public float dissolveTime = 1f;

    // private TextureChecker _textureChecker;
    
    Renderer rend;

    int maskTextureID = Shader.PropertyToID("_MaskTexture");

    public RenderTexture getMask() => maskRenderTexture;
    public RenderTexture getUVIslands() => uvIslandsRenderTexture;
    public RenderTexture getExtend() => extendIslandsRenderTexture;
    public RenderTexture getSupport() => supportTexture;
    public Renderer getRenderer() => rend;

    void Start() {
        maskRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        maskRenderTexture.filterMode = FilterMode.Bilinear;

        extendIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        extendIslandsRenderTexture.filterMode = FilterMode.Bilinear;

        uvIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        uvIslandsRenderTexture.filterMode = FilterMode.Bilinear;

        supportTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        supportTexture.filterMode =  FilterMode.Bilinear;

        rend = GetComponent<Renderer>();
        mat = rend.material;
        rend.material.SetTexture(maskTextureID, extendIslandsRenderTexture);

        PaintManager.instance.initTextures(this);
    }

    public void DissolveAndDestroy()
    {
        if (isStartDissolve)
            return;
        isStartDissolve = true;
        if (mat != null && dissolveTime > 0)
        {
            // Используем DoTween для анимации значения dissolve от 0 до 1
            mat.DOFloat(0f, "_dissolve", dissolveTime)
                .OnComplete(() =>
                {
                    // Анимация завершена
                   Destroy(gameObject);
                });
        }
    }

    void OnDisable(){
        maskRenderTexture.Release();
        uvIslandsRenderTexture.Release();
        extendIslandsRenderTexture.Release();
        supportTexture.Release();
    }
}