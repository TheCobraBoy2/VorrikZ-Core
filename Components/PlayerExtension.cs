using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Storage;

namespace VorrikZ_Core.Components
{
    public static class PlayerExtension
    {
        public static StorageEntity GetBackpackStorage(this Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            var backpackStorage = player.gameObject.GetComponent<StorageEntity>();
            if (backpackStorage == null)
                throw new InvalidOperationException("Player does not have a BackpackStorage component");

            return backpackStorage;
        }
    }
}
