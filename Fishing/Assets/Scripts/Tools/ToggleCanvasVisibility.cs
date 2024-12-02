using UnityEngine;

public class ToggleCanvasVisibility : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public void ToggleVisibility()
    {
        if (canvasGroup != null)
        {
            bool isVisible = canvasGroup.alpha > 0;
            canvasGroup.alpha = isVisible ? 0 : 1;
            canvasGroup.interactable = !isVisible;
            canvasGroup.blocksRaycasts = !isVisible;
        }
    }
}
