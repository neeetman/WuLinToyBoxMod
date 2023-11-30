using BepInEx.Unity.IL2CPP.Utils.Collections;
using UnityEngine.EventSystems;


namespace HaxxToyBox.GUI;

[RegisterInIl2Cpp(typeof(IEventSystemHandler), typeof(IPointerEnterHandler), typeof(IPointerExitHandler), typeof(IPointerDownHandler), typeof(IPointerUpHandler))]
public class FadeButtonWrapper : MonoBehaviour 
{
    private float _fadeTime = 0.2f;
    private float _onHoverAlpha = 0.6f;
    private float _onClickAlpha = 0.7f;

    private Button _button;
    private CanvasGroup _canvasGroup;

    public FadeButtonWrapper(IntPtr ptr) : base(ptr) { }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(Utils.FadeOut(_canvasGroup, _onHoverAlpha, _fadeTime).WrapToIl2Cpp());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(Utils.FadeIn(_canvasGroup, 1.0f, _fadeTime).WrapToIl2Cpp());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _canvasGroup.alpha = _onClickAlpha;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1.0f;
    }
}
