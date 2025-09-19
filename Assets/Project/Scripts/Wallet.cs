using UnityEngine;
using System;

[System.Serializable]
public class Wallet : MonoBehaviour
{
    public static Wallet Instance { get; private set; }

    [Header("Currency Values")]
    [SerializeField] private int coins = 100;
    [SerializeField] private int gems = 10;
    [SerializeField] private int energy = 50;
    [SerializeField] private int lives = 3;

    public static event Action<int> OnCoinsChanged;
    public static event Action<int> OnGemsChanged;
    public static event Action<int> OnEnergyChanged;
    public static event Action<int> OnLivesChanged;

    public int Coins 
    { 
        get => coins; 
        private set 
        { 
            coins = value; 
            OnCoinsChanged?.Invoke(coins);
            SaveData();
        } 
    }
    
    public int Gems 
    { 
        get => gems; 
        private set 
        { 
            gems = value; 
            OnGemsChanged?.Invoke(gems);
            SaveData();
        } 
    }
    
    public int Energy 
    { 
        get => energy; 
        private set 
        { 
            energy = value; 
            OnEnergyChanged?.Invoke(energy);
            SaveData();
        } 
    }
    
    public int Lives 
    { 
        get => lives; 
        private set 
        { 
            lives = value; 
            OnLivesChanged?.Invoke(lives);
            SaveData();
        } 
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Инициализируем UI
        OnCoinsChanged?.Invoke(coins);
        OnGemsChanged?.Invoke(gems);
        OnEnergyChanged?.Invoke(energy);
        OnLivesChanged?.Invoke(lives);
    }

    #region Add Currency
    public void AddCoins(int amount)
    {
        if (amount > 0)
            Coins += amount;
    }

    public void AddGems(int amount)
    {
        if (amount > 0)
            Gems += amount;
    }

    public void AddEnergy(int amount)
    {
        if (amount > 0)
            Energy += amount;
    }

    public void AddLives(int amount)
    {
        if (amount > 0)
            Lives += amount;
    }
    #endregion

    #region Spend Currency
    public bool SpendCoins(int amount)
    {
        if (amount > 0 && coins >= amount)
        {
            Coins -= amount;
            return true;
        }
        return false;
    }

    public bool SpendGems(int amount)
    {
        if (amount > 0 && gems >= amount)
        {
            Gems -= amount;
            return true;
        }
        return false;
    }

    public bool SpendEnergy(int amount)
    {
        if (amount > 0 && energy >= amount)
        {
            Energy -= amount;
            return true;
        }
        return false;
    }

    public bool SpendLives(int amount)
    {
        if (amount > 0 && lives >= amount)
        {
            Lives -= amount;
            return true;
        }
        return false;
    }
    #endregion

    #region Check Currency
    public bool HasEnoughCoins(int amount) => coins >= amount;
    public bool HasEnoughGems(int amount) => gems >= amount;
    public bool HasEnoughEnergy(int amount) => energy >= amount;
    public bool HasEnoughLives(int amount) => lives >= amount;
    #endregion

    #region Save/Load Data
    private void SaveData()
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("Gems", gems);
        PlayerPrefs.SetInt("Energy", energy);
        PlayerPrefs.SetInt("Lives", lives);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        coins = PlayerPrefs.GetInt("Coins", coins);
        gems = PlayerPrefs.GetInt("Gems", gems);
        energy = PlayerPrefs.GetInt("Energy", energy);
        lives = PlayerPrefs.GetInt("Lives", lives);
    }
    #endregion

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveData();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            SaveData();
    }
}
