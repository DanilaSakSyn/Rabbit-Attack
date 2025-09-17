using UnityEngine;

public class CarrotCell : MonoBehaviour
{
    public SpriteRenderer carrotSpriteRenderer;

    public bool hasCarrot;
    public bool hasRabbit;
    public void ShowCarrot(bool show)
    {
        if (carrotSpriteRenderer != null)
            carrotSpriteRenderer.enabled = show;
        hasCarrot = show;
    }
}

