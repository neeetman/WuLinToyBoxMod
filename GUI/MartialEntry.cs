using GameData;
using TMPro;
using WuLin;
using WuLin.GameFrameworks;

namespace HaxxToyBox.GUI;

[RegisterInIl2Cpp]
internal class MartialEntry : MonoBehaviour
{
    private KungfuData _data;
    public KungfuData Data {
        get => _data;
        set
        {
            if (value == _data || value == null) return;

            _data = value;
            _nameText.text = GetRichText(_data.UName, _data.Rarity);
            _icon.sprite = GetIcon(_data.Icon);
        }
    }

    private Button _button;
    private Image _icon;
    private TextMeshProUGUI _nameText;

    public MartialEntry(IntPtr ptr) : base(ptr) { }

    private void Awake()
    {
        _nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        _icon = transform.Find("Icon").GetComponent<Image>();

        _button = transform.Find("Button").GetComponent<Button>();
        _button.gameObject.AddComponent<FadeButtonWrapper>();
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClickAdd);
    }

    public void OnClickAdd()
    {
        var character = MartialPanel.Instance?.Character;
        if (character == null) return;

        character.InstantLearnKungfu(Data, 1);
    }


    private string GetRichText(string text, int rarity)
    {
        string color;
        switch (rarity) {
            case 3:
                color = "orange";
                break;
            case 2:
                color = "purple";
                break;
            case 1:
                color = "#87CEEB";
                break;
            default:
                return text;
        }

        return $"<color={color}>{text}</color>";
    }

    private Sprite GetIcon(string path)
    {
        Sprite sprite = null;
        try {
            sprite = ResourceManager.Instance.GetSprite("UI/Icons/Kungfu/" + path + ".png");
        }
        catch { }

        if (sprite == null) {
            sprite = ResourceManager.Instance.GetSprite("UI/Icons/Kungfu/Default.png");
        }
        return sprite;
    }

}
