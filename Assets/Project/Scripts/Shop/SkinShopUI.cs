using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class SkinShopUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform skinItemsParent;
    [SerializeField] private GameObject skinItemPrefab;
    [SerializeField] private ScrollRect scrollRect;
    
    [Header("Messages")]
    [SerializeField] private GameObject insufficientFundsPopup;
    [SerializeField] private TextMeshProUGUI insufficientFundsText;
    [SerializeField] private float messageDisplayTime = 2f;
    
    [Header("Filters")]
    [SerializeField] private Button allSkinsButton;
    [SerializeField] private Button ownedSkinsButton;
    [SerializeField] private Button availableSkinsButton;
    
    [Header("Current Skin Display")]
    [SerializeField] private Image currentSkinPreview;
    [SerializeField] private TextMeshProUGUI currentSkinNameText;
    
    private List<SkinShopItem> skinItemComponents = new List<SkinShopItem>();
    private FilterType currentFilter = FilterType.All;
    
    public enum FilterType
    {
        All,
        Owned,
        Available
    }
    
    private void Start()
    {
        InitializeUI();
        SetupFilterButtons();
        
        // Подписываемся на события
        SkinManager.OnSkinPurchased += OnSkinPurchasedHandler;
        SkinManager.OnSkinEquipped += OnSkinEquippedHandler;
        SkinManager.OnSkinsInitialized += RefreshShop;
        
        // Обновляем отображение текущего скина
        UpdateCurrentSkinDisplay();
    }
    
    private void OnDestroy()
    {
        // Отписываемся от событий
        SkinManager.OnSkinPurchased -= OnSkinPurchasedHandler;
        SkinManager.OnSkinEquipped -= OnSkinEquippedHandler;
        SkinManager.OnSkinsInitialized -= RefreshShop;
    }
    
    private void InitializeUI()
    {
        if (insufficientFundsPopup != null)
            insufficientFundsPopup.SetActive(false);
            
        RefreshShop();
    }
    
    private void SetupFilterButtons()
    {
        allSkinsButton?.onClick.AddListener(() => SetFilter(FilterType.All));
        ownedSkinsButton?.onClick.AddListener(() => SetFilter(FilterType.Owned));
        availableSkinsButton?.onClick.AddListener(() => SetFilter(FilterType.Available));
    }
    
    public void SetFilter(FilterType filter)
    {
        currentFilter = filter;
        RefreshShop();
        
        // Обновляем визуальное состояние кнопок фильтров
        UpdateFilterButtonsVisual();
    }
    
    private void UpdateFilterButtonsVisual()
    {
        // Сбрасываем все кнопки
        ResetButtonVisual(allSkinsButton);
        ResetButtonVisual(ownedSkinsButton);
        ResetButtonVisual(availableSkinsButton);
        
        // Выделяем активную кнопку
        Button activeButton = currentFilter switch
        {
            FilterType.All => allSkinsButton,
            FilterType.Owned => ownedSkinsButton,
            FilterType.Available => availableSkinsButton,
            _ => allSkinsButton
        };
        
        if (activeButton != null)
        {
            ColorBlock colors = activeButton.colors;
            colors.normalColor = colors.selectedColor;
            activeButton.colors = colors;
        }
    }
    
    private void ResetButtonVisual(Button button)
    {
        if (button == null) return;
        
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        button.colors = colors;
    }
    
    public void RefreshShop()
    {
        ClearShopItems();
        PopulateShopItems();
    }
    
    private void ClearShopItems()
    {
        foreach (var item in skinItemComponents)
        {
            if (item != null && item.gameObject != null)
                DestroyImmediate(item.gameObject);
        }
        skinItemComponents.Clear();
    }
    
    private void PopulateShopItems()
    {
        if (SkinManager.Instance == null) return;
        
        var skinsToShow = GetFilteredSkins();
        
        foreach (var skinItem in skinsToShow)
        {
            CreateSkinItemUI(skinItem);
        }
    }
    
    private IEnumerable<SkinItem> GetFilteredSkins()
    {
        return currentFilter switch
        {
            FilterType.All => SkinManager.Instance.AllSkins,
            FilterType.Owned => SkinManager.Instance.OwnedSkins,
            FilterType.Available => SkinManager.Instance.AvailableForPurchase,
            _ => SkinManager.Instance.AllSkins
        };
    }
    
    private void CreateSkinItemUI(SkinItem skinItem)
    {
        if (skinItemPrefab == null || skinItemsParent == null) return;
        
        GameObject itemObject = Instantiate(skinItemPrefab, skinItemsParent);
        SkinShopItem shopItem = itemObject.GetComponent<SkinShopItem>();
        
        if (shopItem != null)
        {
            shopItem.Initialize(skinItem, this);
            skinItemComponents.Add(shopItem);
        }
    }
    
    public void OnSkinPurchased(SkinItem skinItem)
    {
        // Обновляем все элементы UI
        RefreshAllSkinItems();
        
        // Показываем сообщение об успешной покупке
        StartCoroutine(ShowPurchaseSuccessMessage(skinItem));
    }
    
    public void OnSkinEquipped(SkinItem skinItem)
    {
        // Обновляем все элементы UI
        RefreshAllSkinItems();
        
        // Обновляем отображение текущего скина
        UpdateCurrentSkinDisplay();
    }
    
    private void OnSkinPurchasedHandler(SkinItem skinItem)
    {
        OnSkinPurchased(skinItem);
    }
    
    private void OnSkinEquippedHandler(SkinItem skinItem)
    {
        OnSkinEquipped(skinItem);
    }
    
    private void RefreshAllSkinItems()
    {
        foreach (var shopItem in skinItemComponents)
        {
            shopItem?.RefreshUI();
        }
    }
    
    private void UpdateCurrentSkinDisplay()
    {
        if (SkinManager.Instance?.CurrentSkin != null)
        {
            var currentSkin = SkinManager.Instance.CurrentSkin;
            
            if (currentSkinPreview != null)
                currentSkinPreview.sprite = currentSkin.skinData.skinPreview;
                
            if (currentSkinNameText != null)
                currentSkinNameText.text = currentSkin.skinData.skinName;
        }
    }
    
    public void ShowInsufficientFundsMessage()
    {
        StartCoroutine(ShowMessage("Not enough money!"));
    }
    
    private IEnumerator ShowPurchaseSuccessMessage(SkinItem skinItem)
    {
        string message = $"You bought!\n \"{skinItem.skinData.skinName}\"";
        yield return ShowMessage(message);
    }
    
    private IEnumerator ShowMessage(string message)
    {
        if (insufficientFundsPopup != null && insufficientFundsText != null)
        {
            insufficientFundsText.text = message;
            insufficientFundsPopup.SetActive(true);
            
            yield return new WaitForSeconds(messageDisplayTime);
            
            insufficientFundsPopup.SetActive(false);
        }
    }
    
    // Публичные методы для вызова из UI
    public void CloseShop()
    {
        gameObject.SetActive(false);
    }
    
    public void OpenShop()
    {
        gameObject.SetActive(true);
        RefreshShop();
    }
}
