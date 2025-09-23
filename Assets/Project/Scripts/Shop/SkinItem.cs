using UnityEngine;
using System;

[System.Serializable]
public class SkinItem
{
    public string skinId;
    public SkinData skinData;
    public bool isOwned;
    public bool isEquipped;
    
    public SkinItem(string id, SkinData data)
    {
        skinId = id;
        skinData = data;
        isOwned = data.isDefault;
        isEquipped = data.isDefault;
    }
    
    public bool CanPurchase()
    {
        if (isOwned) return false;
        
        if (skinData.HasCoinPrice())
        {
            return Wallet.Instance.HasEnoughCoins(skinData.coinPrice);
        }
        else if (skinData.HasGemPrice())
        {
            return Wallet.Instance.HasEnoughGems(skinData.gemPrice);
        }
        
        return false;
    }
    
    public bool TryPurchase()
    {
        if (isOwned) return false;
        
        bool purchaseSuccess = false;
        
        if (skinData.HasCoinPrice())
        {
            purchaseSuccess = Wallet.Instance.SpendCoins(skinData.coinPrice);
        }
        else if (skinData.HasGemPrice())
        {
            purchaseSuccess = Wallet.Instance.SpendGems(skinData.gemPrice);
        }
        
        if (purchaseSuccess)
        {
            isOwned = true;
            SkinManager.Instance?.SaveSkinData(skinId, isOwned, isEquipped);
        }
        
        return purchaseSuccess;
    }
    
    public void Equip()
    {
        if (!isOwned) return;
        
        isEquipped = true;
       // SkinManager.Instance?.EquipSkin(this);
    }
    
    public void Unequip()
    {
        isEquipped = false;
        SkinManager.Instance?.SaveSkinData(skinId, isOwned, isEquipped);
    }
}
