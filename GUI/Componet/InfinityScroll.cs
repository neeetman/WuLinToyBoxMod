namespace HaxxToyBox.GUI;

//public interface IInfinityScrollEntry
//{
//    void SetData(object data);
//}

//internal class InfinityScroll<TData> : MonoBehaviour where TData : class
//{
//    public GameObject itemPrefab;
//    public int columns = 1;

//    private ScrollRect scrollRect;
//    private List<GameObject> items = new List<GameObject>();
//    private float itemHeight;
//    private float itemWidth;
//    private float spacex = 0;
//    private float spacey = 0;
//    private int rowsVisibleInView;

//    public List<TData> data = new();
//    public int totalItems => data.Count;

//    private void Awake()
//    {
//        scrollRect = GetComponent<ScrollRect>();
//    }

//    private void Start()
//    {
//        GameObject tempItem = Instantiate(itemPrefab, scrollRect.content);
//        itemHeight = tempItem.GetComponent<RectTransform>().sizeDelta.y + spacey;
//        itemWidth = tempItem.GetComponent<RectTransform>().sizeDelta.x + spacex;
//        Destroy(tempItem);

//        rowsVisibleInView = Mathf.CeilToInt(scrollRect.viewport.sizeDelta.y / itemHeight) + 2;

//        for (int i = 0; i < rowsVisibleInView * columns; i++)
//        {
//            GameObject item = Instantiate(itemPrefab, scrollRect.content);
//            items.Add(item);
//        }

//        UpdateItems(0);
//    }


//    private void LateUpdate()
//    {
//        int startIndex = Mathf.FloorToInt(scrollRect.content.anchoredPosition.y / itemHeight) * columns;

//        if (startIndex <= totalItems - columns)
//        {
//            UpdateItems(startIndex);
//        }
//    }

//    private void UpdateItems(int startIndex)
//    {
//        for (int i = 0; i < items.Count; i++)
//        {
//            int realIndex = i + startIndex;

//            if (realIndex < 0 || realIndex >= totalItems) {
//                items[i].SetActive(false);
//                continue;
//            }

//            items[i].SetActive(true);

//            RectTransform itemRect = items[i].GetComponent<RectTransform>();
//            int row = i / columns;
//            int column = i % columns;

//            itemRect.anchoredPosition = new Vector2(itemWidth * column, -(row * itemHeight) - startIndex / columns * itemHeight);
//            IInfinityScrollEntry entry = items[i].GetComponent<IInfinityScrollEntry>();
//            if (entry != null) {
//                entry.SetData(data[realIndex]);
//            }

//            //items[i].GetComponent<TraitEntry>().SetData(TraitPanel.Instance.traits[realIndex]);
//            //items[i].GetComponent<ItemEntry>().SetItem(ItemPanel.Instance.itemList[realIndex]);
//        }
//    }

//    public void SetTotalItems(int num)
//    {
//        //totalItems = num;

//        //RectTransform contentRect = scrollRect.content;
//        //contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalItems / columns * itemHeight);
//    }
//    public void UpdateData(ref List<TData> newdata)
//    {
//        data = newdata;

//        RectTransform contentRect = scrollRect.content;
//        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalItems / columns * itemHeight);

//        scrollRect.SetVerticalNormalizedPosition(1);
//    }
//}
