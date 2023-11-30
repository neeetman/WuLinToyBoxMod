using GameData;
using TMPro;
using WuLin;

namespace HaxxToyBox.GUI;

[RegisterInIl2Cpp]
public class ItemPanel : MonoBehaviour
{
    private TMP_InputField _numberInput;
    private TMP_InputField _searchInput;
    private ToggleGroup _typeGroup;
    private ToggleGroup _subtypeGroup;

    private InfinityScrollItemData _infinityScroll;

    private ItemType[][] _typeList = { 
        new ItemType[] { ItemType.Equip,
            ItemType.Equip_Weapon, ItemType.Equip_Armor, ItemType.Equip_Amulet },
        new ItemType[] { ItemType.KungfuBook,
            ItemType.KungfuBook_Outer,
            ItemType.KungfuBook_Inner },
        new ItemType[] { ItemType.Consumeable_Recipe|ItemType.Consumeable_Edible,
            ItemType.Consumeable_Edible_Meal|ItemType.Consumeable_Edible_Fruit,
            ItemType.Consumeable_Edible_Elixir, ItemType.Consumeable_Edible_Medicine, ItemType.Consumeable_Recipe},
        new ItemType[] { ItemType.Consumeable_Material },
        new ItemType[] { ItemType.Misc_Map },
        new ItemType[] { ItemType.Misc^ItemType.Misc_Map },
    };
    private Dictionary<ItemType, List<ItemData>> _classifiedItems = new ();

    private int _selectedType = 0;

    public List<ItemData> ItemList = new ();
    public int Number = 1;

    public static ItemPanel Instance { get; private set; }

    public ItemPanel(IntPtr ptr) : base(ptr) { }

    private void Awake()
    {
        Instance = this;

        _typeGroup = transform.Find("TypeGroup/Toggles").GetComponent<ToggleGroup>();
        _subtypeGroup = transform.Find("SubtypeGroup").GetComponent<ToggleGroup>();
        for (int i = 0; i < _typeGroup.transform.childCount; i++) {
            var toggle = _typeGroup.transform.GetChild(i).GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            var type = i;
            toggle.onValueChanged.AddListener((bool value) => {
                if (value) {
                    UpdateItemList(type);
                    UpdateSubToggles(type);
                }
            });
        }

        for (int i = 0; i < _subtypeGroup.transform.childCount; i++) {
            var toggle = _subtypeGroup.transform.GetChild(i).GetComponent<Toggle>();
            int subtype = i;
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((bool value) => {
                if (value) {
                    UpdateItemList(_selectedType, subtype);
                }
            });
        }
        
        _numberInput = transform.Find("NumInput").GetComponent<TMP_InputField>();
        _numberInput.onValueChanged.RemoveAllListeners();
        _numberInput.onValueChanged.AddListener((string input) => {
            int.TryParse(input, out Number);
            Number = Mathf.Clamp(Number, 1, 9999);
        });

        _searchInput = transform.Find("SearchInput").GetComponent<TMP_InputField>();
        _searchInput.onValueChanged.RemoveAllListeners();
        _searchInput.onValueChanged.AddListener((string input) => {

        });

        var scrollView = transform.Find("ScrollView").gameObject;
        var entryPrefab = transform.Find("ScrollView/Viewport/EntryPrefab").gameObject;
        entryPrefab.AddComponent<ItemEntry>();
        _infinityScroll = scrollView.AddComponent<InfinityScrollItemData>();
        _infinityScroll.ItemPrefab = entryPrefab;
        _infinityScroll.Columns = 11;
        _infinityScroll.SpaceX = 25;
        _infinityScroll.SpaceY = 25;
        
        LoadItemData();
    }

    private void LoadItemData()
    {
#if DEBUGMODE
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
#endif

        foreach (var group in _typeList) {
            ItemType mainType = group[0];
            if (!_classifiedItems.ContainsKey(mainType)) {
                _classifiedItems[mainType] = new List<ItemData>();
            }
        }

        var itemsConfig = GameConfig.Instance.ItemDataScriptObject.ItemData;
        foreach (var itemData in itemsConfig) {
            foreach (var group in _typeList) {
                ItemType mainType = group[0];
                if ((itemData.Type & mainType) == itemData.Type) {
                    _classifiedItems[mainType].Add(itemData);
                    break; 
                }
            }
        }

        foreach (var list in _classifiedItems.Values) {
            list.Sort((a, b) => a.Piror.CompareTo(b.Piror));
        }

#if DEBUGMODE
        stopwatch.Stop();
        ToyBox.LogMessage("LoadItemData Execution time: " + stopwatch.ElapsedMilliseconds + "ms");
#endif
    }

    private void UpdateSubToggles(int type)
    {

#if DEBUGMODE
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
#endif

        var toggles = _subtypeGroup.transform;

        int togglenum = _typeList[type].Length;
        switch(type) {
            case 0:
                toggles.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "武器";
                toggles.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "内甲";
                toggles.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = "配饰";
                break;
            case 1:
                toggles.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "外功";
                toggles.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "内功";
                break;
            case 2:
                toggles.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "食物";
                toggles.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "丹药";
                toggles.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = "药品";
                toggles.GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = "配方";
                break;
        }

        for (int i = 0; i < toggles.childCount; i++) {
            toggles.GetChild(i).gameObject.SetActive(i < togglenum);
        }

        toggles.GetChild(0).GetComponent<Toggle>().isOn = true;

#if DEBUGMODE
        stopwatch.Stop();
        ToyBox.LogMessage("UpdateSubToggles Execution time: " + stopwatch.ElapsedMilliseconds + "ms");
#endif
    }


    private void UpdateItemList(int maintype, int subType = 0)
    {

        if (maintype < 0 || maintype >= _typeList.Length) return;

#if DEBUGMODE
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
#endif

        _selectedType = maintype;
        var type = _typeList[maintype][0];
        ItemList = _classifiedItems[type].Where(x =>
            (x.Type & _typeList[maintype][subType]) == x.Type)
            .ToList();

        _infinityScroll.Data = ItemList;

#if DEBUGMODE
        stopwatch.Stop();
        ToyBox.LogMessage("UpdateItemList Execution time: " + stopwatch.ElapsedMilliseconds + "ms");
#endif
    }

}
