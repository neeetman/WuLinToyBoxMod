using BepInEx.Configuration;

namespace HaxxToyBox.Config;

public class ConfigHandler
{
    private ConfigFile Config => ToyBox.Instance.Config;

    private const string SEC_NAME = "HaxxToyBox";

    public void RegisterConfigElement(ConfigElement config)
    {
        ConfigEntry<KeyCode> entry = Config.Bind(SEC_NAME, config.Name, config.Value, config.Description);

        entry.SettingChanged += (object o, EventArgs e) => {
            config.Value = entry.Value;
        };
    }

    public KeyCode GetConfigValue(ConfigElement element)
    {
        if (Config.TryGetEntry(SEC_NAME, element.Name, out ConfigEntry<KeyCode> configEntry))
            return configEntry.Value;
        else
            throw new Exception("Could not get config entry '" + element.Name + "'");
    }

    public void SetConfigValue(ConfigElement element, KeyCode value)
    {
        if (Config.TryGetEntry(SEC_NAME, element.Name, out ConfigEntry<KeyCode> configEntry))
            configEntry.Value = value;
        else
            ToyBox.LogMessage("Could not get config entry '" + element.Name + "'");
    }

    public void LoadConfig()
    {
        foreach (KeyValuePair<string, ConfigElement> entry in ConfigManager.ConfigElements) {
            string key = entry.Key;
            ConfigDefinition def = new(SEC_NAME, key);
            if (Config.ContainsKey(def) && Config[def] is ConfigEntry<KeyCode> configEntry) {
                ConfigElement config = entry.Value;
                config.Value = configEntry.Value;
            }
        }
    }

    public void SaveConfig()
    {
        Config.Save();
    }
}
