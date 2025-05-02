using HarmonyLib;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.Persistence.Datas;
using MelonLoader;
using VorrikZ_Core.Components;

namespace VorrikZ_Core.Patches
{
    [HarmonyPatch(typeof(Il2CppScheduleOne.PlayerScripts.Player))]
    public static class Player
    {
        public static void Awake(Il2CppScheduleOne.PlayerScripts.Player __instance)
        {
            var backpackStorage = __instance.GetBackpackStorage();
            if (backpackStorage)
            {
                backpackStorage.SlotCount = Config.SlotCount.Value;
                backpackStorage.DisplayRowCount = Config.RowCount.Value;
                backpackStorage.StorageEntityName = Config.StorageName.Value;
                backpackStorage.MaxAccessDistance = float.PositiveInfinity;
            }

            if (__instance.LocalExtraFiles.Contains("Backpack"))
                return;

            Melon<Core>.Logger.Msg("Registering backpack file for player.");
            __instance.LocalExtraFiles.Add("Backpack");
        }

        [HarmonyPatch("WriteData")]
        [HarmonyPostfix]
        public static void WriteData(Il2CppScheduleOne.PlayerScripts.Player __instance, string parentFolderPath)
        {
            var backpackStorage = __instance.GetBackpackStorage();
            var contents = new ItemSet(backpackStorage.ItemSlots).GetJSON();
            __instance.Cast<ISaveable>().WriteSubfile(parentFolderPath, "Backpack", contents);
        }

        [HarmonyPatch(typeof(Il2CppScheduleOne.PlayerScripts.Player), "Load")]
        [HarmonyPatch(new Type[] { typeof(PlayerData), typeof(string) })]
        [HarmonyPostfix]
        public static void Load(Il2CppScheduleOne.PlayerScripts.Player __instance, PlayerData data, string containerPath)
        {
            if (!__instance.Loader.TryLoadFile(containerPath, "Backpack", out var backpackData))
                return;

            Melon<Core>.Logger.Msg("Loading local backpack data.");
            try
            {
                var backpackStroage = __instance.GetBackpackStorage();
                var itemSet = ItemSet.Deserialize(backpackData);
                backpackStroage.LoadFromItemSet(itemSet);
            }
            catch (Exception e)
            {
                Melon<Core>.Logger.Error($"Error loading backpack data: {e.Message}");
            }
        }

        [HarmonyPatch("LoadInventory")]
        [HarmonyPostfix]
        public static void LoadInventory(Il2CppScheduleOne.PlayerScripts.Player __instance, ref string contentsString)
        {
            if (string.IsNullOrEmpty(contentsString))
                return;

            if (!__instance.IsOwner)
            {
                Melon<Core>.Logger.Msg("Not the owner, skipping backpack data load.");
                return;
            }

            var backpackString = contentsString.Split(["|||"], StringSplitOptions.None);
            if (backpackString.Length > 2)
                return;

            contentsString = backpackString[0];
            var backpackData = backpackString[1];
            Melon<Core>.Logger.Msg("Loading backpack data from network.");
            try
            {
                var backpackStorage = __instance.GetBackpackStorage();
                var itemSet = ItemSet.Deserialize(backpackData);
                backpackStorage.LoadFromItemSet(itemSet);
            }
            catch (Exception e)
            {
                Melon<Core>.Logger.Error($"Error loading backpack data: {e.Message}");
            }
        }

        [HarmonyPatch("Activate")]
        [HarmonyPrefix]
        public static void Activate()
        {
            Melon<Core>.Logger.Msg("Activating backpack");
            PlayerBackpack.Instance.SetBackpackEnabled(true);
        }

        [HarmonyPatch("Deactivate")]
        [HarmonyPrefix]
        public static void Deactivate()
        {
            Melon<Core>.Logger.Msg("Deactivating backpack");
            PlayerBackpack.Instance.SetBackpackEnabled(false);
        }

        [HarmonyPatch("ExitAll")]
        [HarmonyPrefix]
        public static void ExitAll()
        {
            Melon<Core>.Logger.Msg("Exiting all backpacks");
            PlayerBackpack.Instance.SetBackpackEnabled(false);
        }

        [HarmonyPatch("OnDied")]
        [HarmonyPrefix]
        public static void OnDied(Il2CppScheduleOne.PlayerScripts.Player __instance)
        {
            if (!__instance.Owner.IsLocalClient)
                return;

            Melon<Core>.Logger.Msg("Player died, disabling backpack");
            PlayerBackpack.Instance.SetBackpackEnabled(false);
        }
    }
}
