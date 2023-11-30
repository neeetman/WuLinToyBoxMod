using System.Collections;
using UnityEngine.EventSystems;

namespace HaxxToyBox.GUI;

public static class Utils
{
    public static IEnumerator FadeIn(CanvasGroup group, float alpha, float duration)
    {
        var time = 0.0f;
        var originalAlpha = group.alpha;
        while (time < duration) {
            time += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
            yield return new WaitForEndOfFrame();
        }

        group.alpha = alpha;
    }

    public static IEnumerator FadeOut(CanvasGroup group, float alpha, float duration)
    {
        var time = 0.0f;
        var originalAlpha = group.alpha;
        while (time < duration) {
            time += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
            yield return new WaitForEndOfFrame();
        }

        group.alpha = alpha;
    }

    public static void RemoveAllChildren(this Transform transform, bool immediate)
    {
        if (transform == null) return;

        for (int i = transform.childCount - 1; i >= 0; i--) {
            if (immediate) {
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            } else {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    public static void SetSelected(GameObject go, bool triggerOnReselect = true)
    {
        if (EventSystem.current != null && go != null && !EventSystem.current.alreadySelecting) {
            bool wasSelected = EventSystem.current.currentSelectedGameObject == go;
            EventSystem.current.SetSelectedGameObject(go);
            // Ensure that even for reselection the select handler is being fired.
            if (wasSelected && triggerOnReselect) {
                ExecuteEvents.ExecuteHierarchy(go, new BaseEventData(EventSystem.current), ExecuteEvents.selectHandler);
            }
        }
    }
}
