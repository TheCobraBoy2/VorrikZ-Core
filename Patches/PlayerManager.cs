using HarmonyLib;
using Il2CppScheduleOne.Persistence.Datas;
using Il2CppScheduleOne.Persistence.Loaders;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;

namespace VorrikZ_Core.Patches
{
    [HarmonyPatch(typeof(PlayerManager))]
    public static class PlayerManagerPatch
    {
        [HarmonyPatch("TryGetPlayerData")]
        [HarmonyPostfix]
        public static void TryGetPlayerData(PlayerManager __instance, PlayerData data, ref string inventoryString)
        {
            if (data == null)
                return;

            Il2CppSystem.String dataPath = (Il2CppSystem.String)__instance.loadedPlayerDataPaths[new Index(__instance.loadedPlayerData.IndexOf(data))];
            PlayerLoader loader = new();
            if (!loader.TryLoadFile(dataPath, "Backpack", out var backpackString))
            {
                Melon<Core>.Logger.Warning("Failed to load player backpack under " + dataPath);
                return;
            }

            inventoryString += "|||" + backpackString;
        }
    }
}
