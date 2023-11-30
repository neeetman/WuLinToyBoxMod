namespace HaxxToyBox.GUI;

[RegisterInIl2Cpp]
public class Switch : MonoBehaviour
{
    private Button _button;
    private Animator _animator;

    private Image _bgEnabledImage;
    private Image _bgDisabledImage;

    private Image _handleEnabledImage;
    private Image _handleDisabledImage;

    private bool _switchEnabled;

    public Switch(IntPtr ptr) : base(ptr) { }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _animator = GetComponent<Animator>();

        _bgEnabledImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _bgDisabledImage = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        _handleEnabledImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        _handleDisabledImage = transform.GetChild(1).GetChild(1).GetComponent<Image>();

        _switchEnabled = true;
        Toggle();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(Toggle);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Toggle);
        _button.onClick.RemoveAllListeners();
    }

    public void Toggle()
    {
        _switchEnabled = !_switchEnabled;

        if (_switchEnabled) {
            _bgDisabledImage.gameObject.SetActive(false);
            _bgEnabledImage.gameObject.SetActive(true);
            _handleDisabledImage.gameObject.SetActive(false);
            _handleEnabledImage.gameObject.SetActive(true);
        }
        else {
            _bgEnabledImage.gameObject.SetActive(false);
            _bgDisabledImage.gameObject.SetActive(true);
            _handleEnabledImage.gameObject.SetActive(false);
            _handleDisabledImage.gameObject.SetActive(true);
        }
        _animator.SetTrigger(_switchEnabled ? "Enable" : "Disable");
    }

    public bool IsToggled()
    {
        return _switchEnabled;
    }

}
