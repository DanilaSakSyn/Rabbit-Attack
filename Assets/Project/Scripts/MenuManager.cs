using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private string gameSceneName = "GameScene";

        public void StartGame()
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
}