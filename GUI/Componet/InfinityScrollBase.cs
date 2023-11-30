namespace HaxxToyBox.GUI;

// Currently, Il2CppInterop does not support registering
// template/generic classes to Il2Cpp. 
internal abstract class InfinityScrollBase : MonoBehaviour
{
    protected ScrollRect _scrollRect;
    protected List<GameObject> _items = new List<GameObject>();
    protected float _itemHeight;
    protected float _itemWidth;
    protected int _rowsVisibleInView;

    public float SpaceX = 0;
    public float SpaceY = 0;
    public GameObject ItemPrefab;
    public int Columns = 1;

    public virtual int TotalItems { get; }

    //public InfinityScrollBase() 
    //{
    //    ClassInjector.RegisterTypeInIl2Cpp<InfinityScrollBase>();
    //}

    public InfinityScrollBase(IntPtr ptr) : base(ptr) { }

    protected virtual void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    protected void UpdateContentSize()
    {
        //ToyBox.LogMessage($"UpdateContentSize called with TotalItems {TotalItems}");
        RectTransform contentRect = _scrollRect.content;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, (TotalItems + Columns - 1) / Columns * _itemHeight);
        _scrollRect.SetVerticalNormalizedPosition(1);
    }

    protected virtual void Start()
    {
        InitializeItemDimensions();
        InitializeVisibleRows();
        InitializeItems();
        UpdateContentSize();
    }

    protected void InitializeItemDimensions()
    {
        GameObject tempItem = Instantiate(ItemPrefab, _scrollRect.content);
        _itemHeight = tempItem.GetComponent<RectTransform>().sizeDelta.y + SpaceX;
        _itemWidth = tempItem.GetComponent<RectTransform>().sizeDelta.x + SpaceY;
        Destroy(tempItem);
    }

    protected void InitializeVisibleRows()
    {
        _rowsVisibleInView = Mathf.CeilToInt(_scrollRect.viewport.sizeDelta.y / _itemHeight) + 2;
    }

    protected void InitializeItems()
    {
        for (int i = 0; i < _rowsVisibleInView * Columns; i++) {
            GameObject item = Instantiate(ItemPrefab, _scrollRect.content);
            _items.Add(item);
        }

        UpdateItems(0);
    }

    protected virtual void LateUpdate()
    {
        int startIndex = Mathf.FloorToInt(_scrollRect.content.anchoredPosition.y / _itemHeight) * Columns;
        if (startIndex <= TotalItems - Columns) {
            UpdateItems(startIndex);
        }
    }


    // The method should ideally be defined as abstract.
    // However, due to limitations with Il2cppInterop, we can't declare it as such.
    protected virtual void UpdateItems(int startIndex) 
    {
        // DO NOTHING
        return;
    }
}
