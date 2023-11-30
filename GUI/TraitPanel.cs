using GameData;

namespace HaxxToyBox.GUI;

[RegisterInIl2Cpp]
internal class TraitPanel : MonoBehaviour
{
    //private bool needUpdate = true;

    private InfinityScrollTraitData _infinityScroll;

    public PopupPanel Popup;

    public List<TraitData> Traits;

    public static TraitPanel Instance { get; private set; }

    public TraitPanel(IntPtr ptr) : base(ptr) { }

    private void Awake()
    {
        Instance = this;

        Popup = gameObject.AddComponent<PopupPanel>();

        var close = transform.Find("PopupBase/Top/CloseButton").GetComponent<Button>();
        close.gameObject.AddComponent<FadeButtonWrapper>();
        close.onClick.AddListener(() => {
            Hide();
        });

        var scrollView = transform.Find("ScrollView").gameObject;
        var entryPrefab = transform.Find("ScrollView/Viewport/EntryPrefab").gameObject;
        entryPrefab.AddComponent<TraitEntry>();
        _infinityScroll = scrollView.AddComponent<InfinityScrollTraitData>();
        _infinityScroll.ItemPrefab = entryPrefab;
        _infinityScroll.SpaceY = 10;
    }
   
    private void Start()
    {
        var traitDB = BaseDataClass.GetGameData<TraitDataScriptObject>().data;

        Traits = new List<TraitData>();
        foreach (var trait in traitDB.Values) {
            Traits.Add(trait);
        }
        Traits.Sort((a, b) => b.Rarity.CompareTo(a.Rarity));

        _infinityScroll.Data = Traits;
        // infinityScroll.SetTotalItems(traits.Count);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Popup.Open();

        //if (needUpdate) {
        //    UpdateTraitList();
        //}
    }

    public void Hide()
    {
        Popup.Close();
        gameObject.SetActive(false);

        RolePanel.Instance.UpdateRoleInfo();
    }

    private void UpdateTraitList()
    {
        //for (int i = 0; i < traits.Count; i++) {
        //    var trait = traits[i];
        //    GameObject entry = null;
        //    if (i >= scrollView.childCount) {
        //        entry = Instantiate(entryPrefab, scrollView);
        //    }
        //    else {
        //        entry = scrollView.GetChild(i).gameObject;
        //    }
        //    entry.SetActive(true);
        //    entry.GetComponent<TraitEntry>().SetTrait(trait);
        //}

        //for (int i = traits.Count; i < scrollView.childCount; i++) {
        //    scrollView.GetChild(i).gameObject.SetActive(false);
        //}
    }
}
