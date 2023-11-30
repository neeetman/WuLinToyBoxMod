using GameData;
using TMPro;
using WuLin;
using HaxxToyBox.Config;

namespace HaxxToyBox.GUI;

[RegisterInIl2Cpp]
internal class MiscPanel : MonoBehaviour
{
    public static MiscPanel Instance { get; private set; }
    
    private Switch _timeFreezeSwitch;
    private Switch _recoverSwitch;
    private Switch _noCombatSwitch;
    private Switch _relationSwitch;
    private Switch _enableAchieveSwitch;
    private Switch _ultimateMartialSwitch;

    private Slider _battleSpeedSlider;
    private TMP_InputField _coinInput;

    private InputKeyUGUI _toggleKeyUI;
    private InputKeyUGUI _speedUpKeyUI;
    private InputKeyUGUI _speedDownKeyUI;
    private InputKeyUGUI _recoverKeyUI;

    public int ExpMultiple = 1;
    public int WalkSpeed = 1;
    public int BattleSpeed = 1;

    public bool TimeFreezed => _timeFreezeSwitch.IsToggled();
    public bool RecoverEnabled => _recoverSwitch.IsToggled();
    public bool NoCombat => _noCombatSwitch.IsToggled();
    public bool RelationEnabled => _relationSwitch.IsToggled();
    public bool EnableAchieve => _enableAchieveSwitch.IsToggled();
    public bool UltimateMartial => _ultimateMartialSwitch.IsToggled();

    public MiscPanel(IntPtr ptr) : base(ptr) { }

    private void Awake()
    {
        Instance = this;

        _timeFreezeSwitch = transform.Find("Content/SwitchFunc/TimeFreeze/Switch").gameObject.AddComponent<Switch>();
        _recoverSwitch = transform.Find("Content/SwitchFunc/Recover/Switch").gameObject.AddComponent<Switch>();
        _noCombatSwitch = transform.Find("Content/SwitchFunc/NoCombat/Switch").gameObject.AddComponent<Switch>();
        _relationSwitch = transform.Find("Content/SwitchFunc/Friendship/Switch").gameObject.AddComponent<Switch>();
        _enableAchieveSwitch = transform.Find("Content/SwitchFunc/EnableAchievement/Switch").gameObject.AddComponent<Switch>();
        _ultimateMartialSwitch = transform.Find("Content/SwitchFunc/UltimateMartial/Switch").gameObject.AddComponent<Switch>();

        var expInput = transform.Find("Content/InputFunc/SkillExp/NumInput").GetComponent<TMP_InputField>();
        expInput.onValueChanged.RemoveAllListeners();
        expInput.onValueChanged.AddListener((string input) => {
            int.TryParse(input, out ExpMultiple);
            ExpMultiple = Mathf.Clamp(ExpMultiple, 1, 1000);
        });

        _coinInput = transform.Find("Content/InputFunc/Gold/NumInput").GetComponent<TMP_InputField>();
        _coinInput.onValueChanged.RemoveAllListeners();
        _coinInput.onValueChanged.AddListener((string input) => {
            if (!long.TryParse(input, out long value))
                _coinInput.text = _coinInput.m_OriginalText;
            else {
                var inventory = MonoSingleton<PlayerTeamManager>.Instance.TeamInventory;
                inventory.SetCurrency(CurrencyType.Coin, value * 1000);
            }
        });

        var walkspeedSlider = transform.Find("Content/SliderFunc/WalkSpeed/Slider");
        walkspeedSlider.Find("Text").gameObject.AddComponent<SliderAmountText>();
        walkspeedSlider.GetComponent<Slider>().onValueChanged.AddListener((float value) => {
            WalkSpeed = (int)value;
            //var player = RoamingManager.Instance?.player;
            //if (player == null) return;

            //if (!player.SpeedKey.ContainsKey("toybox")) {
            //    player.SpeedKey.Add("toybox", value);
            //}
            //else {
            //    player.SpeedKey["toybox"] = value;
            //}
        });

        _battleSpeedSlider = transform.Find("Content/SliderFunc/BattleSpeed/Slider").GetComponent<Slider>();
        _battleSpeedSlider.transform.Find("Text").gameObject.AddComponent<SliderAmountText>();
        _battleSpeedSlider.onValueChanged.AddListener((float value) => {
            GameTimer.Instance.AddOrSetTimeScale(this, value);
            BattleSpeed = (int)value;
        });

        var buttonAchievements = transform.Find("Content/ButtonFunc/Achievement").gameObject;
        buttonAchievements.AddComponent<FadeButtonWrapper>();
        buttonAchievements.GetComponent<Button>().onClick.AddListener(() => {
            var achievementDB = BaseDataClass.GetGameData<AchievementDataScriptObject>().data;
            foreach (var id in achievementDB.Keys) {
                MonoSingleton<AchievementManager>.Instance.Complate(id);
            }
        });

        var buttonRecover = transform.Find("Content/ButtonFunc/Recover").gameObject;
        buttonRecover.AddComponent<FadeButtonWrapper>();
        buttonRecover.GetComponent<Button>().onClick.AddListener(RecoverAll);

        _toggleKeyUI = transform.Find("Content/ConfigFunc/PanelToggle").gameObject.AddComponent<InputKeyUGUI>();
        _speedUpKeyUI = transform.Find("Content/ConfigFunc/SpeedupToggle").gameObject.AddComponent<InputKeyUGUI>();
        _speedDownKeyUI = transform.Find("Content/ConfigFunc/SpeeddownToggle").gameObject.AddComponent<InputKeyUGUI>();
        _recoverKeyUI = transform.Find("Content/ConfigFunc/Recover").gameObject.AddComponent<InputKeyUGUI>();

        BindInputKey(_toggleKeyUI, ConfigManager.Canvas_Toggle);
        BindInputKey(_speedUpKeyUI, ConfigManager.SpeedUp_Toggle);
        BindInputKey(_speedDownKeyUI, ConfigManager.SpeedDown_Toggle);
        BindInputKey(_recoverKeyUI, ConfigManager.Recover_Toggle);
    }

    private void BindInputKey(InputKeyUGUI obj, ConfigElement config)
    {
        obj.Key = config.Value;
        obj.AllowAbortWithCancelButton = true;
        obj.OnChanged += (KeyCode key, KeyCode modifierKey) => config.Value = key;
    }

    private void OnEnable()
    {
        var inventory = PlayerTeamManager.Instance?.TeamInventory;
        if (inventory != null) {
            _coinInput.SetTextWithoutNotify((inventory.GetCurrency(CurrencyType.Coin) / 1000).ToString());
        }

        _battleSpeedSlider.value = BattleSpeed;
    }

    public static void RecoverAll()
    {
        var teamManager = PlayerTeamManager.Instance;
        if (teamManager == null) return;
        teamManager.ModifyProp("队伍体力", 100);
        teamManager.ModifyProp("队伍心情", 100);
        for (int i = 0; i < teamManager.TeamSize; i++) {
            teamManager.GetTeamMemberByIndex(i).FullyRecover();
        }
    }

    public static void SpeedDown()
    {
        if (Instance == null) return;
        int min = (int)Instance._battleSpeedSlider.minValue;
        Instance.BattleSpeed = Math.Max(Instance.BattleSpeed-1, min);

        GameTimer.Instance.AddOrSetTimeScale(Instance, Instance.BattleSpeed);
    }

    public static void SpeedUp()
    {
        if (Instance == null) return;
        int max = (int)Instance._battleSpeedSlider.maxValue;
        Instance.BattleSpeed = Math.Min(Instance.BattleSpeed+1, max);

        GameTimer.Instance.AddOrSetTimeScale(Instance, Instance.BattleSpeed);
    }

}
