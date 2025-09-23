using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Shop/Skin Data")]
public class SkinData : ScriptableObject
{
    [Header("Skin Information")] public string skinName;
    public string skinDescription;
    public Sprite skinIcon;
    public Sprite skinPreview;

    [Header("Pricing")] public int coinPrice = 0;
    public int gemPrice = 0;

    [Header("Skin Properties")] public bool isDefault = false;
    public bool isUnlocked = false;
    public float cooldown = 10f;


    public bool CanBePurchased()
    {
        return !isDefault && !isUnlocked && (coinPrice > 0 || gemPrice > 0);
    }

    public bool HasCoinPrice()
    {
        return coinPrice > 0;
    }

    public bool HasGemPrice()
    {
        return gemPrice > 0;
    }


    public virtual void Use(GameManager gameManager, RabbitController rabbitController)
    {
        
    }
}