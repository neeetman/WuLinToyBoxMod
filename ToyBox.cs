global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using UnityEngine;
global using UnityEngine.UI;
global using UniverseLib;
global using Il2CppInterop.Runtime.Injection;
global using HaxxToyBox.Utilities;
global using HaxxToyBox.Utilities.Extensions;
global using HaxxToyBox.Utilities.Attributes;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using HaxxToyBox.Patches;
using HaxxToyBox.Config;
using HaxxToyBox.GUI;

namespace HaxxToyBox;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class ToyBox : BasePlugin
{
    public static ToyBox Instance { get; private set; }

    private static readonly Harmony _harmonyIns = new(MyPluginInfo.PLUGIN_GUID);
    public Harmony HarmonyInstance => _harmonyIns;

    public ConfigHandler ConfigHandler { get; private set; }

    public ToyBox()
    {
        Instance = this;
        RegisterInIl2CppAttribute.Initialize();
    }

    public override void Load()
    {
        Instance = this;
        ConfigHandler = new ConfigHandler();
        ConfigManager.Init(ConfigHandler);

        LogMessage($"{MyPluginInfo.PLUGIN_NAME} initializing...");

        Universe.Init(1f, LateInit, LogInternal, new() {
            Disable_EventSystem_Override = true,
            Unhollowed_Modules_Folder = Path.Combine(Paths.BepInExRootPath, "interop")
        });

        try {
            HarmonyInstance.PatchAll(typeof(MiscPatch));
            //HarmonyInstance.PatchAll(typeof(TestPatch));
        }
        catch {
            LogError("Failed to ");
        }
    }

    static void LateInit()
    {
        LogMessage($"Creating UI...");

        ToyBoxBehaviour.Setup();

        LogMessage($"{MyPluginInfo.PLUGIN_NAME} initialized.");
    }

    #region LOGGING

    public static void LogMessage(object message) => LogInternal(message, LogType.Log);
    public static void LogWarning(object message) => LogInternal(message, LogType.Warning);
    public static void LogError(object message) => LogInternal(message, LogType.Error);
    private static void LogInternal(object message, LogType logType)
    {
        string log = message?.ToString() ?? "";

        switch (logType) {
            case LogType.Assert:
            case LogType.Log:
                Instance.Log.LogMessage(log);
                break;

            case LogType.Warning:
                Instance.Log.LogWarning(log);
                break;

            case LogType.Error:
            case LogType.Exception:
                Instance.Log.LogError(log);
                break;
        }
    }

    #endregion
}
