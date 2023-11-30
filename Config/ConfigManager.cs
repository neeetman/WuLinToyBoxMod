using System.Collections.Generic;
using UnityEngine;

namespace HaxxToyBox.Config;

public static class ConfigManager
{
    internal static readonly Dictionary<string, ConfigElement> ConfigElements = new();

    public static ConfigHandler Handler { get; private set; }

    public static ConfigElement Canvas_Toggle;
    public static ConfigElement SpeedUp_Toggle;
    public static ConfigElement SpeedDown_Toggle;
    public static ConfigElement Recover_Toggle;

    public static void Init(ConfigHandler configHandler)
    {
        Handler = configHandler;

        CreateConfigElements();

        Handler.LoadConfig();
    }

    internal static void RegisterConfigElement(ConfigElement configElement)
    {
        Handler.RegisterConfigElement(configElement);
        ConfigElements.Add(configElement.Name, configElement);
    }

    private static void CreateConfigElements()
    {
        Canvas_Toggle = new("HaxxToyBox Toggle",
            "The key to show and hide ToyBox panel.",
            KeyCode.Tab);

        SpeedUp_Toggle = new("SpeedUp Toggle",
            "The key to increase game speed.",
            KeyCode.Equals);
        SpeedDown_Toggle = new("SpeedDown Toggle",
            "The key to decrease game speed.",
            KeyCode.Minus);

        Recover_Toggle = new("Recover Toggle",
            "The key to recover all statuses of the entire team.",
            KeyCode.F1);
    }
}
