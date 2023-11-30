using System;
using UnityEngine;

namespace HaxxToyBox.Config;

public class ConfigElement
{
    public string Name { get; }
    public string Description { get; }

    public Action<KeyCode> OnValueChanged;
    public Action OnValueChangedNotify { get; set; }

    public object DefaultValue { get; }

    public ConfigHandler Handler => ConfigManager.Handler;

    public KeyCode Value {
        get => m_value;
        set => SetValue(value);
    }
    private KeyCode m_value;

    public ConfigElement(string name, string description, KeyCode defaultValue)
    {
        Name = name;
        Description = description;

        m_value = defaultValue;
        DefaultValue = defaultValue;

        ConfigManager.RegisterConfigElement(this);
    }

    private void SetValue(KeyCode value)
    {
        if (m_value == value)
            return;

        m_value = value;

        Handler.SetConfigValue(this, value);

        OnValueChanged?.Invoke(value);
        OnValueChangedNotify?.Invoke();

        Handler.SaveConfig();
    }

    public KeyCode GetLoaderConfigValue()
    {
        return Handler.GetConfigValue(this);
    }

    public void RevertToDefaultValue()
    {
        Value = (KeyCode)DefaultValue;
    }
}
