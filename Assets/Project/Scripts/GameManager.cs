using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public RabbitController rabbitController;
    public CarrotCell[] carrotCells;
    public float gameTime = 60f;

    public int rabbitScore;
    public GameObject loseScreen;
    [Header("UI Elements")] public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreText2;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI moneyText2;
public LevelManager levelManager;

    private float timer;
    private bool isGameOver = false;

    public int score;
    public int money;

    private void Start()
    {
        timer = gameTime;
        loseScreen.SetActive(false);
        UpdateTimerDisplay();
        UpdateScoreText();
        UpdateMoneyText();
    }

    private void Update()
    {
        if (isGameOver) return;

        timer -= Time.deltaTime;
        UpdateTimerDisplay();

        if (timer <= 0f)
        {
            Debug.Log("timer <= 0f");
            levelManager.CompleteLevel();
            EndGame();
        }
        else if (AllCarrotsGone())
        {
            Debug.Log("AllCarrotsGone");
            EndGame();
        }
    }

    public void AddScore()
    {
        // Implement score addition logic here
        Debug.Log("Score added: " + rabbitScore);
        score += rabbitScore;
        money += 1;
        UpdateScoreText();
        UpdateMoneyText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
        scoreText2.text = "Score: " + score;
    }

    public void UpdateMoneyText()
    {
        moneyText.text = money + "<sprite index=0>";
        moneyText2.text = money + "<sprite index=0>";
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private bool AllCarrotsGone()
    {
        foreach (var cell in carrotCells)
        {
            if (cell.hasCarrot)
                return false;
        }

        return true;
    }

    private void EndGame()
    {
        Debug.Log("Game Over");
        isGameOver = true;
        rabbitController.enabled = false;
        Wallet.Instance.AddCoins(money);
        loseScreen.SetActive(true);
    }
    
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void GotoMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}