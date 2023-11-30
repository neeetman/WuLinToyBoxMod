using HarmonyLib;
using HaxxToyBox.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuLin;

namespace HaxxToyBox.Patches;

public class TestPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveObjectGameTime), "AddDeltaTime")]
    public static bool AddDeltaTime_PrePatch(float gameDeltaTime)
    {
        ToyBox.LogMessage($"AddDeltaTime called with gameDeltaTime {gameDeltaTime}");

        return true;
    }
}
