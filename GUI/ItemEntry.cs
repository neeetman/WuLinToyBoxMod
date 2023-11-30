using GameData;
using TMPro;
using WuLin;

namespace HaxxToyBox.GUI;

[RegisterInIl2Cpp]
internal class ItemEntry : MonoBehaviour
{
    private ItemData _data;
    public ItemData Data {
        get => _data;
        set
        {
            if (value == _data || value == null) return;

            _data = value;
            _nameText.text = _data.GetName(true);
            _icon.sprite = _data.GetIcon();
        }
    }

    private Button _button;
    private Image _icon;
    private TextMeshProUGUI _nameText;

    public ItemEntry(IntPtr ptr) : base(ptr) { }

    private void Awake()
    {
        _nameText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        _icon = transform.Find("Button/Icon").GetComponent<Image>();

        _button = transform.Find("Button").GetComponent<Button>();
        _button.gameObject.AddComponent<FadeButtonWrapper>();
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (_data == null) return;

        var pack = new GameItemPack();
        pack.AddItem(_data, ItemPanel.Instance.Number);
        PlayerTeamManager.Instance?.PickupPack(pack);
    }
}
