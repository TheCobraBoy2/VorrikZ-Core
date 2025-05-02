using HarmonyLib;
using Il2CppFishNet.Component.Spawning;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Storage;
using Il2CppVLB;
using MelonLoader;
using VorrikZ_Core.Components;


namespace VorrikZ_Core.Patches
{
    [HarmonyPatch(typeof(PlayerSpawner))]
    public static class PlayerSpawnerPatch
    {
        [HarmonyPatch("InitializeOnce")]
        [HarmonyPostfix]
        public static void InitializeOnce(PlayerSpawner __instance)
        {
            Il2CppFishNet.Object.NetworkObject playerPrefab = __instance._playerPrefab;
            if (!playerPrefab)
            {
                Melon<Core>.Logger.Error("Player prefab is null!");
                return;
            }

            var player = playerPrefab.GetComponent<Player>();
            if (player == null)
            {
                Melon<Core>.Logger.Error("Player prefab does not have a Player component!");
                return;
            }

            var storage = player.gameObject.GetOrAddComponent<StorageEntity>();
            storage.SlotCount = Config.SlotCount.Value;
            storage.DisplayRowCount = Config.RowCount.Value;
            storage.StorageEntityName = Config.StorageName.Value;
            storage.MaxAccessDistance = float.PositiveInfinity;
            player.LocalGameObject.GetOrAddComponent<PlayerBackpack>();
        }
    }
}
