using System.Collections;
using UnityEngine;

namespace Project.Scripts.Shop
{
    [CreateAssetMenu (fileName = "New Skin", menuName = "Shop/Freeze Tool")]
    public class FreezeTool : SkinData
    {
        public float freezeTime;

        public override void Use(GameManager gameManager, RabbitController rabbitController)
        {
            //base.Use(gameManager, rabbitController);
            
            GameObject freezeObject = new GameObject("FreezeEffect");
            
          freezeObject.AddComponent<FreezeEffect>().Initialize(gameManager, rabbitController, this);
       
        }
    }
}