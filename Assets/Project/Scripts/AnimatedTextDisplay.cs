using System.Collections;
using UnityEngine;
using TMPro;

public class AnimatedTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textElement;
    public float animationDuration = 1f;
    public float maxScale = 1.5f;

    private Vector3 originalScale;

    private void Awake()
    {
        if (textElement == null)
        {
            Debug.LogError("Text element is not assigned.");
            return;
        }

        originalScale = textElement.transform.localScale;
            // DisplayText("Level 1");
    }

    public void DisplayText(string message)
    {
        if (textElement == null) return;

        textElement.text = message;
        StopAllCoroutines();
        StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText()
    {
        float halfDuration = animationDuration / 2f;
        float timer = 0f;

        // Scale up
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(1f, maxScale, timer / halfDuration);
            textElement.transform.localScale = originalScale * scale;
            yield return null;
        }

        timer = 0f;
        yield return new WaitForSeconds(0.5f);
        // Scale down
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(maxScale, 1f, timer / halfDuration);
            textElement.transform.localScale = originalScale * scale;
            yield return null;
        }

        textElement.transform.localScale = originalScale;
        gameObject.SetActive(false);
    }
}