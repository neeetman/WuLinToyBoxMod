namespace HaxxToyBox.GUI;

[RegisterInIl2Cpp]
public class PopupPanel : MonoBehaviour
{
    private Color _backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);

    private GameObject _background;

    public PopupPanel(IntPtr ptr) : base(ptr) { }

    public void Open()
    {
        AddBackground();
    }

    public void Close()
    {
        RemoveBackground();
    }

    private void AddBackground()
    {
        var bgTex = new Texture2D(1, 1);
        bgTex.SetPixel(0, 0, _backgroundColor);
        bgTex.Apply();

        _background = new GameObject("PopupBackground");
        var image = _background.AddComponent<Image>();
        var rect = new Rect(0, 0, bgTex.width, bgTex.height);
        var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
        image.material.mainTexture = bgTex;
        image.sprite = sprite;

        var canvas = GameObject.Find("ToyBoxCanvas");
        _background.transform.localScale = new Vector3(1, 1, 1);
        _background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
        _background.transform.SetParent(canvas.transform, false);
        _background.transform.SetSiblingIndex(transform.GetSiblingIndex());
    }

    private void RemoveBackground()
    {
        DestroyImmediate(_background);
    }
}
