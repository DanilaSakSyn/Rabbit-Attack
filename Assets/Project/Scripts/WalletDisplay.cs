using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WalletDisplay : MonoBehaviour
{
    [Header("Currency Text Components")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI gemsText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI livesText;

    [Header("Currency Icons (Optional)")]
    [SerializeField] private Image coinsIcon;
    [SerializeField] private Image gemsIcon;
    [SerializeField] private Image energyIcon;
    [SerializeField] private Image livesIcon;

    [Header("Animation Settings")]
    [SerializeField] private bool useAnimation = true;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private int currentDisplayCoins;
    private int currentDisplayGems;
    private int currentDisplayEnergy;
    private int currentDisplayLives;

    private void OnEnable()
    {
        // Подписываемся на события кошелька
        Wallet.OnCoinsChanged += UpdateCoinsDisplay;
        Wallet.OnGemsChanged += UpdateGemsDisplay;
        Wallet.OnEnergyChanged += UpdateEnergyDisplay;
        Wallet.OnLivesChanged += UpdateLivesDisplay;
    }

    private void OnDisable()
    {
        // Отписываемся от событий
        Wallet.OnCoinsChanged -= UpdateCoinsDisplay;
        Wallet.OnGemsChanged -= UpdateGemsDisplay;
        Wallet.OnEnergyChanged -= UpdateEnergyDisplay;
        Wallet.OnLivesChanged -= UpdateLivesDisplay;
    }

    private void Start()
    {
        // Инициализируем отображение текущими значениями из кошелька
        if (Wallet.Instance != null)
        {
            UpdateCoinsDisplay(Wallet.Instance.Coins);
            UpdateGemsDisplay(Wallet.Instance.Gems);
            UpdateEnergyDisplay(Wallet.Instance.Energy);
            UpdateLivesDisplay(Wallet.Instance.Lives);
        }
    }

    #region Update Display Methods
    private void UpdateCoinsDisplay(int newAmount)
    {
        if (coinsText != null)
        {
            if (useAnimation)
                StartCoroutine(AnimateValue(currentDisplayCoins, newAmount, coinsText, (value) => currentDisplayCoins = value));
            else
            {
                coinsText.text = FormatNumber(newAmount);
                currentDisplayCoins = newAmount;
            }
        }
    }

    private void UpdateGemsDisplay(int newAmount)
    {
        if (gemsText != null)
        {
            if (useAnimation)
                StartCoroutine(AnimateValue(currentDisplayGems, newAmount, gemsText, (value) => currentDisplayGems = value));
            else
            {
                gemsText.text = FormatNumber(newAmount);
                currentDisplayGems = newAmount;
            }
        }
    }

    private void UpdateEnergyDisplay(int newAmount)
    {
        if (energyText != null)
        {
            if (useAnimation)
                StartCoroutine(AnimateValue(currentDisplayEnergy, newAmount, energyText, (value) => currentDisplayEnergy = value));
            else
            {
                energyText.text = FormatNumber(newAmount);
                currentDisplayEnergy = newAmount;
            }
        }
    }

    private void UpdateLivesDisplay(int newAmount)
    {
        if (livesText != null)
        {
            if (useAnimation)
                StartCoroutine(AnimateValue(currentDisplayLives, newAmount, livesText, (value) => currentDisplayLives = value));
            else
            {
                livesText.text = FormatNumber(newAmount);
                currentDisplayLives = newAmount;
            }
        }
    }
    #endregion

    #region Animation
    private System.Collections.IEnumerator AnimateValue(int startValue, int targetValue, TextMeshProUGUI textComponent, System.Action<int> onValueUpdate)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;
            float curveValue = animationCurve.Evaluate(progress);
            
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, curveValue));
            textComponent.text = FormatNumber(currentValue);
            onValueUpdate?.Invoke(currentValue);
            
            yield return null;
        }
        
        // Убеждаемся, что финальное значение установлено точно
        textComponent.text = FormatNumber(targetValue);
        onValueUpdate?.Invoke(targetValue);
    }
    #endregion

    #region Utility Methods
    private string FormatNumber(int number)
    {
        // Форматируем большие числа (например, 1000 -> 1K, 1000000 -> 1M)
        if (number >= 1000000)
            return (number / 1000000f).ToString("0.#") + "M";
        else if (number >= 1000)
            return (number / 1000f).ToString("0.#") + "K";
        else
            return number.ToString();
    }

    // Методы для тестирования в редакторе
    [ContextMenu("Test Add Coins")]
    public void TestAddCoins()
    {
        if (Wallet.Instance != null)
            Wallet.Instance.AddCoins(100);
    }

    [ContextMenu("Test Spend Coins")]
    public void TestSpendCoins()
    {
        if (Wallet.Instance != null)
            Wallet.Instance.SpendCoins(50);
    }

    [ContextMenu("Test Add Gems")]
    public void TestAddGems()
    {
        if (Wallet.Instance != null)
            Wallet.Instance.AddGems(10);
    }
    #endregion

    #region Public Methods for External Use
    public void SetCoinsTextComponent(TextMeshProUGUI textComponent)
    {
        coinsText = textComponent;
        if (Wallet.Instance != null)
            UpdateCoinsDisplay(Wallet.Instance.Coins);
    }

    public void SetGemsTextComponent(TextMeshProUGUI textComponent)
    {
        gemsText = textComponent;
        if (Wallet.Instance != null)
            UpdateGemsDisplay(Wallet.Instance.Gems);
    }

    public void SetEnergyTextComponent(TextMeshProUGUI textComponent)
    {
        energyText = textComponent;
        if (Wallet.Instance != null)
            UpdateEnergyDisplay(Wallet.Instance.Energy);
    }

    public void SetLivesTextComponent(TextMeshProUGUI textComponent)
    {
        livesText = textComponent;
        if (Wallet.Instance != null)
            UpdateLivesDisplay(Wallet.Instance.Lives);
    }
    #endregion
}
