using GameData;

namespace HaxxToyBox.GUI;

[RegisterInIl2Cpp]
internal class InfinityScrollTraitData : InfinityScrollBase
{
    private List<TraitData> _data;

    public List<TraitData> Data {
        get => _data;
        set
        {
            if (value == _data) return;

            _data = value;
            UpdateContentSize();
        }
    }

    public override int TotalItems => _data?.Count ?? 0;

    public InfinityScrollTraitData(IntPtr ptr) : base(ptr) { }

    protected override void UpdateItems(int startIndex)
    {
        for (int i = 0; i < _items.Count; i++) {
            int realIndex = i + startIndex;

            if (realIndex < 0 || realIndex >= TotalItems) {
                _items[i].SetActive(false);
                continue;
            }

            _items[i].SetActive(true);

            RectTransform itemRect = _items[i].GetComponent<RectTransform>();
            int row = i / Columns;
            int column = i % Columns;

            itemRect.anchoredPosition = new Vector2(_itemWidth * column, -(row * _itemHeight) - startIndex / Columns * _itemHeight);
            var entry = _items[i].GetComponent<TraitEntry>();
            if (entry != null) {
                entry.Data = Data[realIndex];
            }
        }
    }
}
