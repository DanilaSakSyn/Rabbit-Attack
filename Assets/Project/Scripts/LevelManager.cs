using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private const string LevelKey = "CurrentLevel";
    public  int currentLevel;

    public AnimatedTextDisplay animatedTextDisplay;
public RabbitController rabbitController;
    private void Start()
    {
        LoadLevel();
        DisplayLevel();
        rabbitController.SetLevel(currentLevel);
    }

    public void CompleteLevel()
    {
        currentLevel++;
        SaveLevel();
        DisplayLevel();
    }

    private void DisplayLevel()
    {
        if (animatedTextDisplay != null)
        {
            animatedTextDisplay.gameObject.SetActive(true);
            animatedTextDisplay.DisplayText($"Level {currentLevel}");
        }
    }

    private void SaveLevel()
    {
        PlayerPrefs.SetInt(LevelKey, currentLevel);
        PlayerPrefs.Save();
    }

    private void LoadLevel()
    {
        currentLevel = PlayerPrefs.GetInt(LevelKey, 1); // Default to level 1 if no data is saved
    }
}
