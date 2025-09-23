using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class Rabbit : MonoBehaviour, IPointerClickHandler, IFreeze
    {
        public CarrotCell targetCell;
        public SpriteRenderer rabbitSpriteRenderer;
        public Sprite rabbitNormalSprite;
        public Sprite rabbitEatSprite;
        public Animator animator;
        public float lifeTime = 1f;
        public Vector3 offcet;
        private bool isClicked = false;
        public RabbitController controller;

        public GameObject timerIndicatorPrefab;
        public Canvas uiCanvas;
        private Image timerImage;
        private GameObject timerInstance;
        private float timerDuration = 10f;
        public float timerElapsed = 0f;
        public bool isFrozen = false;

        void Start()
        {
            SpawnTimerIndicator();
            StartCoroutine(RabbitLifeCycle());
        }

        void SpawnTimerIndicator()
        {
            if (timerIndicatorPrefab != null && uiCanvas != null)
            {
                timerInstance = Instantiate(timerIndicatorPrefab, uiCanvas.transform);
                timerImage = timerInstance.GetComponentInChildren<Image>();
                UpdateTimerIndicatorPosition();
            }
        }

        void UpdateTimerIndicatorPosition()
        {
            if (timerInstance != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + offcet);
                timerInstance.transform.position = screenPos;
            }
        }

        void Update()
        {
            if (timerInstance != null)
                UpdateTimerIndicatorPosition();
        }

        IEnumerator RabbitLifeCycle()
        {
            timerElapsed = 0f;
            while (timerElapsed < timerDuration && !isClicked)
            {
                if (isFrozen)
                {
                    yield return null;
                    continue;
                }

                timerElapsed += Time.deltaTime;
                if (timerImage != null)
                    timerImage.fillAmount = 1f - (timerElapsed / timerDuration);
                yield return null;
            }

            if (isClicked) yield break;
            isClicked = true;
            if (targetCell != null)
                targetCell.ShowCarrot(false);
            if (rabbitSpriteRenderer != null && rabbitEatSprite != null)
                rabbitSpriteRenderer.sprite = rabbitEatSprite;
            yield return new WaitForSeconds(lifeTime);
            controller.OnRabbitRemoved();
            targetCell.hasRabbit = false;
            Destroy(timerInstance);
            Destroy(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isClicked)
            {
                StartCoroutine(KickedCoroutine());
            }
        }

        private IEnumerator KickedCoroutine()
        {
            isClicked = true;
            if (animator != null)
                animator.SetTrigger("Clicked");
            controller.OnRabbitRemoved();
            controller.AddScore();
            Destroy(timerInstance);
            yield return new WaitForSeconds(1.3f);
            targetCell.hasRabbit = false;
            Destroy(gameObject);
        }

        public void Freeze()
        {
            Debug.Log($"{name} is Freeze");
            isFrozen = true;
        }

        public void UnFreeze()
        {
            isFrozen = false;
        }
    }
}