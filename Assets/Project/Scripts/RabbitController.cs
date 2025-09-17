using System.Linq;
using Project.Scripts;
using UnityEngine;

public class RabbitController : MonoBehaviour
{
    public GameObject rabbitPrefab;
    public Canvas indicatorCanvas;
    public CarrotCell[] carrotCells;

    public int maxRabbits = 5;
    public float spawnInterval = 3f;

    private int currentRabbits = 0;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval && currentRabbits < maxRabbits)
        {
            SpawnRabbit();
            timer = 0f;
        }
    }

    public void SpawnRabbit()
    {
        if (carrotCells.Length == 0 || rabbitPrefab == null || currentRabbits >= maxRabbits)
            return;


        var cells = carrotCells.Where(n => n.hasCarrot).ToList();
        int index = Random.Range(0, cells.Count);
        CarrotCell cell = cells[index];
        GameObject rabbit = Instantiate(rabbitPrefab, cell.transform.position, Quaternion.identity);
        Rabbit rabbitScript = rabbit.GetComponent<Rabbit>();
        if (rabbitScript != null)
        {
            rabbitScript.targetCell = cell;
            cell.hasCarrot = true;
            rabbitScript.controller = this;
            rabbitScript.uiCanvas = indicatorCanvas;
        }

        currentRabbits++;
    }

    public void OnRabbitRemoved()
    {
        currentRabbits = Mathf.Max(0, currentRabbits - 1);
    }
}