using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public RabbitController rabbitController;
    public CarrotCell[] carrotCells;
    public float gameTime = 60f;
    public GameObject loseScreen;
    private float timer;
    private bool isGameOver = false;

    private void Start()
    {
        timer = gameTime;
        loseScreen.SetActive(false);
    }

    private void Update()
    {
        if (isGameOver) return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Debug.Log("timer <= 0f");

            EndGame();
        }
        else if (AllCarrotsGone())
        {
            Debug.Log("AllCarrotsGone");

            EndGame();
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
        loseScreen.SetActive(true);
    }
}