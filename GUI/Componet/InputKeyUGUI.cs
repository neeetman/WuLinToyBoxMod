using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace HaxxToyBox.GUI;

[RegisterInIl2Cpp]
public class InputKeyUGUI : MonoBehaviour
{
    public InputKeyUGUI(IntPtr ptr) : base(ptr) { }

    public delegate void OnChangedDelegate(KeyCode key, KeyCode modifierKey);

    private KeyCode _key = KeyCode.None;
    public KeyCode Key {
        get => _key;
        set
        {
            if (value == _key)
                return;

            _key = value;
            UpdateKeyName();
        }
    }

    protected KeyCode _modifierKey = KeyCode.None;
    public KeyCode ModifierKey {
        get => _modifierKey;
        set
        {
            if (value == _modifierKey)
                return;

            _modifierKey = value;
            UpdateKeyName();
        }
    }

    public bool AllowKeyCombinations = false;
    public bool AllowAbortWithCancelButton = false;

    // The first key code is the normal key (like A, SPACE, ENTER, ...). The second key code is the modifier key (CTRL, SHIFT, COMMAND or TAB).
    public UnityEvent<KeyCode, KeyCode> OnChangedEvent;
    public OnChangedDelegate OnChanged;

    public Button Button;
    public GameObject Normal;
    public GameObject Active;
    public TextMeshProUGUI TextTf;
    public TextMeshProUGUI KeyNameTf;
    public TextMeshProUGUI ActiveTextTf;

    public bool IsActive => Active.activeSelf;

    public string Text {
        get => TextTf.text;
        set
        {
            if (value == Text)
                return;

            TextTf.text = value;
        }
    }

    public string KeyName {
        get => KeyNameTf.text;
        set
        {
            if (value == KeyName)
                return;

            KeyNameTf.text = value;
        }
    }

    public string ActiveText {
        get => ActiveTextTf.text;
        set
        {
            if (value == ActiveText)
                return;

            ActiveTextTf.text = value;
        }
    }

    public void Awake()
    {
        Button = GetComponent<Button>();
        Normal = transform.Find("KeyWithTextNormal").gameObject;
        Active = transform.Find("KeyWithTextActive").gameObject;
        TextTf = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        KeyNameTf = transform.Find("KeyWithTextNormal/KeyNameTf").GetComponent<TextMeshProUGUI>();
        ActiveTextTf = transform.Find("KeyWithTextActive/ActiveTextTf").GetComponent<TextMeshProUGUI>();

        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(() => { SetActive(true); });
    }

    protected bool waitForKeyRelease;

    public void SetActive(bool active)
    {
        bool willBeActivated = IsActive != active && active;
        bool willBeDeactivated = IsActive != active && !active;

        // Submit is triggered in onKeyDOWN, thus it is possible that
        // a key is still being held down when this happends.
        // To avoid this key to trigger the key selection (which happens
        // onKeyUP) we have to remember to ignore this key.
        if (willBeActivated && InputKeyUtils.AnyKey()) {
            waitForKeyRelease = true;
        }

        // Reselect Button after input was finished.
        if (willBeDeactivated && EventSystem.current != null) {
            Utils.SetSelected(Button.gameObject);
        }

        Normal.SetActive(!active);
        Active.SetActive(active);
        Button.interactable = !active;

        if (active) {
            _modifierKeyWhileActive = KeyCode.None;
            _keyWhileActive = KeyCode.None;
            _aKeyWasPressedWhileActive = false;
        }
    }

    public void UpdateKeyName()
    {
        if (ModifierKey != KeyCode.None) {
            KeyName = InputKeyUtils.UniversalKeyName(ModifierKey) + " + " + InputKeyUtils.UniversalKeyName(Key);
        }
        else {
            KeyName = InputKeyUtils.UniversalKeyName(Key);
        }
    }

    public bool IsCancelKeyPressed()
    {
        return Input.GetButtonDown("Cancel");
    }

    public void OnEnable()
    {
        Refresh();
    }

    public void OnDisable()
    {
        if (IsActive) {
            waitForKeyRelease = false;
            SetActive(false);
        }
    }

    public void Refresh()
    {
        UpdateKeyName();
    }

    protected KeyCode _modifierKeyWhileActive;
    protected KeyCode _keyWhileActive;
    protected bool _aKeyWasPressedWhileActive;

    public void Update()
    {
        if (!InputKeyUtils.AnyKey()) {
            waitForKeyRelease = false;
        }

        if (IsActive && !waitForKeyRelease) {
            if (AllowAbortWithCancelButton && IsCancelKeyPressed()) {
                SetActive(false);
            }

            // If no more keys are pressed then analyze the results and end editing this key.
            bool keyPressStopped = InputKeyUtils.GetUniversalKeyUp(excludeModifierKeys: false, excludeMouseButtons: true) != KeyCode.None;
            bool mouseClicked = InputKeyUtils.MouseUp();
            if (_aKeyWasPressedWhileActive && (keyPressStopped || mouseClicked)) {
                SetActive(false);

                // Don't set key if mouse was pressed yet mouse is ignored.
                if (!mouseClicked) {
                    // analyze pressed keys
                    if (_modifierKeyWhileActive != KeyCode.None && _keyWhileActive == KeyCode.None) {
                        ModifierKey = KeyCode.None;
                        Key = _modifierKeyWhileActive;
                    }
                    else {
                        if (AllowKeyCombinations) {
                            ModifierKey = _modifierKeyWhileActive;
                        }
                        else {
                            ModifierKey = KeyCode.None;
                        }
                        Key = _keyWhileActive;
                    }

                    OnChanged?.Invoke(Key, ModifierKey);
                    OnChangedEvent?.Invoke(Key, ModifierKey);
                }
            }

            if (InputKeyUtils.AnyKeyDown()) {
                // record pressed keys only if they are not NONE.
                _aKeyWasPressedWhileActive = true;
                var mKey = InputKeyUtils.GetModifierKeyDown();
                if (mKey != KeyCode.None)
                    _modifierKeyWhileActive = mKey;

                var key = InputKeyUtils.GetUniversalKeyDown(excludeModifierKeys: true, excludeMouseButtons: true);
                if (key != KeyCode.None)
                    _keyWhileActive = key;
            }
        }
    }
}
