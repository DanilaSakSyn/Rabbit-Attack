using System;
using UnityEngine;

namespace Project.Scripts
{
    public class GameTools : MonoBehaviour
    {
        [SerializeField] private Transform toolParent;
        [SerializeField] private ToolItem toolPrefab;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private RabbitController rabbitController;
        public void Start()
        {
            var items = SkinManager.Instance.GetOwnedSkins();

            if (items == null || items.Count == 0)
            {
                gameObject.SetActive(false);
                return;
            }
            
            foreach (var item in items)
            {
                var tool = Instantiate(toolPrefab, toolParent);
                tool.SetItem(item);
                tool.gameManager = gameManager;
                tool.rabbitController = rabbitController;
            }
        }


        
    }
}