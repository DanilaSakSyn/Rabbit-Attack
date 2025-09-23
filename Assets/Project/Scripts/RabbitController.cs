using System.Collections.Generic;
using System.Linq;
using Project.Scripts;
using UnityEngine;

public class RabbitController : MonoBehaviour, IFreeze
{
    public GameManager gameManager;
    public GameObject rabbitPrefab;
    public Canvas indicatorCanvas;
    public CarrotCell[] carrotCells;
    public List<Rabbit> rabbits;

    public int maxRabbits = 5;
    public float spawnInterval = 3f;

    private int currentRabbits = 0;
    private float timer = 0f;
    public bool isFrozen = false;

    private void Update()
    {
        if (isFrozen)
            return;
        timer += Time.deltaTime;
        if (timer >= spawnInterval && currentRabbits < maxRabbits)
        {
            SpawnRabbit();
            timer = 0f;
        }
    }

    public void SetLevel(int level)
    {
        maxRabbits = (int)Mathf.Min(5 + level * 0.3f, 20);
        spawnInterval = Mathf.Max(3f - level * 0.1f, .4f);
    }

    public void SpawnRabbit()
    {
        if (carrotCells.Length == 0 || rabbitPrefab == null || currentRabbits >= maxRabbits)
            return;


        var cells = carrotCells.Where(n => n.hasCarrot && !n.hasRabbit).ToList();
        if (cells.Count <= 0) return;
        int index = Random.Range(0, cells.Count);
        CarrotCell cell = cells[index];
        GameObject rabbit = Instantiate(rabbitPrefab, cell.transform.position, Quaternion.identity);
        Rabbit rabbitScript = rabbit.GetComponent<Rabbit>();
        if (rabbitScript != null)
        {
            rabbitScript.targetCell = cell;
            cell.hasCarrot = true;
            cell.hasRabbit = true;
            rabbitScript.controller = this;
            rabbitScript.uiCanvas = indicatorCanvas;
        }

        rabbits.Add(rabbitScript);
        currentRabbits++;
    }

    public void AddScore()
    {
        gameManager.AddScore();
    }

    public void OnRabbitRemoved()
    {
        currentRabbits = Mathf.Max(0, currentRabbits - 1);
    }

    public void Freeze()
    {
        isFrozen = true;
    }

    public void UnFreeze()
    {
        isFrozen = false;
    }
}