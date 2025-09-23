using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class ToolItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Image fieldImage;
        [SerializeField] private SkinItem item;
        [SerializeField] private Button button;
        public GameManager gameManager;
        public RabbitController rabbitController;
        public float cooldown;
        public float time;
        public bool inActive;

        public void SetItem(SkinItem skinItem)
        {
            item = skinItem;
            if (item?.skinData != null && image != null)
            {
                image.sprite = item.skinData.skinIcon;
            }

            cooldown = skinItem.skinData.cooldown;
            StartCoroutine(CoolingDown());
        }

        IEnumerator CoolingDown()
        {
            inActive = false;
            time = cooldown;
            while (0 < time)
            {
                time -= Time.deltaTime;
                if (fieldImage != null)
                    fieldImage.fillAmount = (time / cooldown);
                yield return null;
            }

            inActive = true;
        }

        public void Use()
        {
            if (!inActive) return;

            item.skinData.Use(gameManager, rabbitController);
            StartCoroutine(CoolingDown());
        }
    }
}