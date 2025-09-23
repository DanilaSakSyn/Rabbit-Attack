using System.Linq;
using UnityEngine;

namespace Project.Scripts.Shop
{
[CreateAssetMenu(fileName = "New Skin", menuName = "Shop/Kill All Tool")]
    
    public class KillAllTool : SkinData
    {
        public override void Use(GameManager gameManager, RabbitController rabbitController)
        {
            var rabbits = rabbitController.rabbits.Where(n=>n!=null).ToList();
            rabbitController.rabbits = rabbits;
            foreach (var rabbit in rabbits)
            {
                rabbit.OnPointerClick(null);
            }
        }
    }
}