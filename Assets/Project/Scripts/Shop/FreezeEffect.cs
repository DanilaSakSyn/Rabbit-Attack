using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.Shop
{
    public class FreezeEffect: MonoBehaviour
    {
        private GameManager gameManager;
        private RabbitController rabbitController;
        private FreezeTool freezeTool;
        public void Initialize(GameManager gameManager, RabbitController rabbitController, FreezeTool freezeTool)
        {
            this.gameManager = gameManager;
            this.rabbitController = rabbitController;
            this.freezeTool = freezeTool;
            StartCoroutine(FreezeRoutine());
            
        }

        private IEnumerator FreezeRoutine()
        {
          
            Debug.Log(rabbitController);
            Debug.Log(rabbitController.rabbits);
            List<IFreeze> freezables = rabbitController.rabbits.Where(n=>n!=null&& n is IFreeze).Select(n=>n as IFreeze).ToList();
            freezables.Add(rabbitController);
            
            freezables.ForEach(n=>n.Freeze());
            
            yield return new WaitForSeconds(freezeTool.freezeTime);
            freezables.ForEach(n=>n.UnFreeze());
            Destroy(gameObject);
            
        }
    }
}