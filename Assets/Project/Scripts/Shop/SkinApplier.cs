using UnityEngine;
using UnityEngine.UI;

public class SkinApplier : MonoBehaviour
{
    [Header("Skin Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Image uiImage;
    
    [Header("Default Skin")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Material defaultMaterial;
    
    private void Start()
    {
        // Подписываемся на события смены скина
        SkinManager.OnSkinEquipped += OnSkinEquipped;
        SkinManager.OnSkinsInitialized += OnSkinsInitialized;
        
        // Применяем текущий скин, если он уже установлен
        ApplyCurrentSkin();
    }
    
    private void OnDestroy()
    {
        // Отписываемся от событий
        SkinManager.OnSkinEquipped -= OnSkinEquipped;
        SkinManager.OnSkinsInitialized -= OnSkinsInitialized;
    }
    
    private void OnSkinEquipped(SkinItem skinItem)
    {
        ApplySkin(skinItem);
    }
    
    private void OnSkinsInitialized()
    {
        ApplyCurrentSkin();
    }
    
    private void ApplyCurrentSkin()
    {
        if (SkinManager.Instance?.CurrentSkin != null)
        {
            ApplySkin(SkinManager.Instance.CurrentSkin);
        }
        else
        {
            ApplyDefaultSkin();
        }
    }
    
    public void ApplySkin(SkinItem skinItem)
    {
 
    }
    
    public void ApplyDefaultSkin()
    {
        // Применяем дефолтные настройки
        if (spriteRenderer != null && defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite;
        }
        
        if (meshRenderer != null && defaultMaterial != null)
        {
            meshRenderer.material = defaultMaterial;
        }
        
        if (uiImage != null && defaultSprite != null)
        {
            uiImage.sprite = defaultSprite;
        }
    }
    
    // Метод для ручной установки компонентов
    public void SetSpriteRenderer(SpriteRenderer renderer)
    {
        spriteRenderer = renderer;
        ApplyCurrentSkin();
    }
    
    public void SetMeshRenderer(MeshRenderer renderer)
    {
        meshRenderer = renderer;
        ApplyCurrentSkin();
    }
    
    public void SetUIImage(Image image)
    {
        uiImage = image;
        ApplyCurrentSkin();
    }
}
