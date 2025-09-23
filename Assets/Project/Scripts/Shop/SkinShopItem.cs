using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinShopItem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image skinIcon;
    [SerializeField] private TextMeshProUGUI skinNameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private GameObject ownedIndicator;
    [SerializeField] private GameObject equippedIndicator;
    [SerializeField] private Image currencyIcon;
    [SerializeField] private Sprite coinIcon;
    [SerializeField] private Sprite gemIcon;
    
    private SkinItem skinItem;
    private SkinShopUI shopUI;
    
    public void Initialize(SkinItem item, SkinShopUI shop)
    {
        skinItem = item;
        shopUI = shop;
        
        UpdateUI();
        
        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(OnPurchaseClicked);
        
    }
    
    private void UpdateUI()
    {
        if (skinItem == null) return;
        
        // Обновляем основную информацию
        skinIcon.sprite = skinItem.skinData.skinIcon;
        skinNameText.text = skinItem.skinData.skinName;
        
        // Обновляем состояние кнопок и индикаторов
        bool isOwned = skinItem.isOwned;
        bool isEquipped = skinItem.isEquipped;
        bool canPurchase = skinItem.CanPurchase();
        
        ownedIndicator.SetActive(isOwned);
        equippedIndicator.SetActive(isEquipped);
        
        // Настраиваем кнопки
        purchaseButton.gameObject.SetActive(!isOwned);
        
        if (!isOwned)
        {
            // Настраиваем цену и валюту
            if (skinItem.skinData.HasCoinPrice())
            {
                priceText.text = skinItem.skinData.coinPrice.ToString();
                currencyIcon.sprite = coinIcon;
            }
            else if (skinItem.skinData.HasGemPrice())
            {
                priceText.text = skinItem.skinData.gemPrice.ToString();
                currencyIcon.sprite = gemIcon;
            }
            
            // purchaseButton.interactable = canPurchase;
        }
        else
        {
            priceText.text = "OWNED";
        }
    }
    
    private void OnPurchaseClicked()
    {
        if (skinItem == null || skinItem.isOwned) return;
        
        bool success = SkinManager.Instance.PurchaseSkin(skinItem.skinId);
        
        if (success)
        {
            UpdateUI();
            shopUI?.OnSkinPurchased(skinItem);
        }
        else
        {
            Debug.Log("adasd");
            shopUI?.ShowInsufficientFundsMessage();
        }
    }
    
    private void OnEquipClicked()
    {
        if (skinItem == null || !skinItem.isOwned) return;
        
        SkinManager.Instance.EquipSkin(skinItem);
        shopUI?.OnSkinEquipped(skinItem);
    }
    
    public void RefreshUI()
    {
        UpdateUI();
    }
}
