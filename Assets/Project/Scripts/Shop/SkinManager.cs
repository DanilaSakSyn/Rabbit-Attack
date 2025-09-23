using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }
    
    [Header("Skin Configuration")]
    [SerializeField] private SkinData[] availableSkins;
    
    private Dictionary<string, SkinItem> skinItems = new Dictionary<string, SkinItem>();
    private SkinItem currentEquippedSkin;
    
    public static event Action<SkinItem> OnSkinPurchased;
    public static event Action<SkinItem> OnSkinEquipped;
    public static event Action OnSkinsInitialized;
    
    public SkinItem CurrentSkin => currentEquippedSkin;
    public IEnumerable<SkinItem> AllSkins => skinItems.Values;
    public IEnumerable<SkinItem> OwnedSkins => skinItems.Values.Where(s => s.isOwned);
    public IEnumerable<SkinItem> AvailableForPurchase => skinItems.Values.Where(s => s.skinData.CanBePurchased());
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSkins();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializeSkins()
    {
        skinItems.Clear();
        
        for (int i = 0; i < availableSkins.Length; i++)
        {
            SkinData skinData = availableSkins[i];
            string skinId = $"skin_{i}_{skinData.skinName}";
            
            SkinItem skinItem = new SkinItem(skinId, skinData);
            
            // Загружаем сохраненные данные
            LoadSkinData(skinId, skinItem);
            
            skinItems[skinId] = skinItem;
            
            // Устанавливаем дефолтный скин как экипированный
            if (skinData.isDefault)
            {
                currentEquippedSkin = skinItem;
            }
        }
        
        // Если нет экипированного скина, берем первый доступный
        if (currentEquippedSkin == null && skinItems.Count > 0)
        {
            var firstOwned = skinItems.Values.FirstOrDefault(s => s.isOwned);
            if (firstOwned != null)
            {
                EquipSkin(firstOwned);
            }
        }
        
        OnSkinsInitialized?.Invoke();
    }
    
    public bool PurchaseSkin(string skinId)
    {
        if (!skinItems.ContainsKey(skinId)) return false;
        
        SkinItem skinItem = skinItems[skinId];
        bool success = skinItem.TryPurchase();
        
        if (success)
        {
            OnSkinPurchased?.Invoke(skinItem);
        }
        
        return success;
    }
    
    public void EquipSkin(SkinItem skinItem)
    {
        if (!skinItem.isOwned) return;
        
        // Снимаем предыдущий скин
        if (currentEquippedSkin != null)
        {
            currentEquippedSkin.Unequip();
        }
        
        // Экипируем новый скин
        skinItem.Equip();
        currentEquippedSkin = skinItem;
        
        OnSkinEquipped?.Invoke(skinItem);
    }
    
    public bool EquipSkin(string skinId)
    {
        if (!skinItems.ContainsKey(skinId)) return false;
        
        SkinItem skinItem = skinItems[skinId];
        if (!skinItem.isOwned) return false;
        
        EquipSkin(skinItem);
        return true;
    }
    
    public SkinItem GetSkin(string skinId)
    {
        return skinItems.ContainsKey(skinId) ? skinItems[skinId] : null;
    }
    
    public bool IsSkinOwned(string skinId)
    {
        return skinItems.ContainsKey(skinId) && skinItems[skinId].isOwned;
    }
    
    public bool IsSkinEquipped(string skinId)
    {
        return skinItems.ContainsKey(skinId) && skinItems[skinId].isEquipped;
    }
    
    public List<SkinItem> GetOwnedSkins()
    {
        return skinItems.Values.Where(s => s.isOwned).ToList();
    }
    
    #region Save/Load
    public void SaveSkinData(string skinId, bool isOwned, bool isEquipped)
    {
        PlayerPrefs.SetInt($"Skin_{skinId}_Owned", isOwned ? 1 : 0);
        PlayerPrefs.SetInt($"Skin_{skinId}_Equipped", isEquipped ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    private void LoadSkinData(string skinId, SkinItem skinItem)
    {
        skinItem.isOwned = PlayerPrefs.GetInt($"Skin_{skinId}_Owned", skinItem.skinData.isDefault ? 1 : 0) == 1;
        skinItem.isEquipped = PlayerPrefs.GetInt($"Skin_{skinId}_Equipped", skinItem.skinData.isDefault ? 1 : 0) == 1;
    }
    
    public void SaveAllSkinData()
    {
        foreach (var skinPair in skinItems)
        {
            SaveSkinData(skinPair.Key, skinPair.Value.isOwned, skinPair.Value.isEquipped);
        }
    }
    #endregion
    
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveAllSkinData();
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            SaveAllSkinData();
    }
}
