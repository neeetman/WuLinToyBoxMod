using HarmonyLib;
using TMPro;
using WuLin;
using HaxxToyBox.GUI;
using static WuLin.GameCharacterInstance;
using WuLin.GameFrameworks;

namespace HaxxToyBox.Patches;

public class MiscPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveObjectGameTime), "AddDeltaTime")]
    public static bool AddDeltaTime_PrePatch()
    {
        if (!MiscPanel.Instance) return true;

        return !MiscPanel.Instance.TimeFreezed;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BattleManager), "LeaveBattle")]
    public static void LeaveBattle_PrePatch()
    {
        if (MiscPanel.Instance && MiscPanel.Instance.RecoverEnabled) {
            PlayerTeamManager.Instance.PlayerDataInstance.FullyRecover();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(RoamingManager), "GetNpcBySightPoint")]
    public static void GetNpcBySightPoint_PostPatch(ref Il2CppSystem.Collections.Generic.List<Npc> __result)
    {
        if (MiscPanel.Instance && MiscPanel.Instance.NoCombat) {
            __result.Clear();
        }
    }
    [HarmonyPostfix]
    [HarmonyPatch(typeof(StealthManager), "GetPerceptionSpeed")]
    public static void GetPerceptionSpeed_PostPatch(ref float __result)
    {
        if (MiscPanel.Instance && MiscPanel.Instance.NoCombat) {
            __result = 0;
        }
    }
    [HarmonyPrefix]
    [HarmonyPatch(typeof(StealthPerceptComponent), "OnFound")]
    public static bool OnFound_PrePatch()
    {
        if (!MiscPanel.Instance) return true;

        return !MiscPanel.Instance.NoCombat;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameCharacterInstance), "ChangeAdditionProp")]
    public static bool ChangeAdditionProp_PrePatch(string key, ref Il2CppSystem.Decimal value)
    {
        if (MiscPanel.Instance && key.Contains("能力经验_")) {
            value *= MiscPanel.Instance.ExpMultiple;
        }
        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GiftingWithNpcUI), "Awake")]
    public static void GiftingWithNpcUIAwake_PostPatch(GiftingWithNpcUI __instance)
    {
        var buttonTemp = UiSingletonPrefab<EscUI>.Instance.main_Resume.gameObject;

        var button = UnityEngine.Object.Instantiate(buttonTemp, __instance.transform, false);
        button.name = "IncRelation";
        button.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 40);
        button.GetComponent<RectTransform>().localPosition = new Vector3(-10, 120, 0);
        button.GetComponentInChildren<TextMeshProUGUI>().text = "满好感";
        button.GetComponentInChildren<TextMeshProUGUI>().fontSize = 20;
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        button.GetComponent<Button>().onClick.AddListener(delegate {
            var source = GameCharacterInstance.RelationModifySource.Gift;
            GiftingWithNpcManager.npc?.ModifyRelationWithPlayer(100, source);
        });
    }
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GiftingWithNpcUI), "Update")]
    public static bool GiftingWithNpcUIUpdate_PrePatch(GiftingWithNpcUI __instance)
    {
        if (!MiscPanel.Instance) return true;

        var button = __instance.transform.Find("IncRelation");
        button?.gameObject.SetActive(MiscPanel.Instance.RelationEnabled);
        return true;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Role), "UpdateSpeed")]
    public static void RoleUpdateSpeed_PostPatch(Role __instance)
    {
        if (MiscPanel.Instance && __instance == RoamingManager.Instance.player) {
            __instance.speed *= MiscPanel.Instance.WalkSpeed;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManager), "IsAnyModActivedInHistroy", MethodType.Getter)]
    public static void IsAnyModActivedInHistroy_PostPatch(ref bool __result)
    {
        if (MiscPanel.Instance && MiscPanel.Instance.EnableAchieve) {
            __result = false;
        }
    }

    //[HarmonyPrefix]
    //[HarmonyPatch(typeof(BattleUI), "OnSpeedButtonClickHandler")]
    //public static void BattleUISwitchSpeed_PrePatch()
    //{
    //    if (MiscPanel.Instance) return;

    //    UIMiscPanel.battleSpeed = (UIMiscPanel.battleSpeed * 2) % 7;
    //}

    //[HarmonyPostfix]
    //[HarmonyPatch(typeof(BattleUI), "SwitchSpeed")]
    //public static void BattleUISwitchSpeed_PostPatch(BattleUI __instance)
    //{
    //    if (MiscPanel.Instance) return;

    //    __instance.speedText.text = $"x{UIMiscPanel.battleSpeed}";
    //    Time.timeScale = UIMiscPanel.battleSpeed;

    //}

}
