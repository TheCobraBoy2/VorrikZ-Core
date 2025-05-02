using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Storage;
using Il2CppScheduleOne.Tools;
using Il2CppScheduleOne.UI;
using MelonLoader;
using UnityEngine;
using VorrikZ_Core.Tools;

namespace VorrikZ_Core.Components
{
    [RegisterTypeInIl2Cpp]
    public class PlayerBackpack(IntPtr ptr) : MonoBehaviour(ptr)
    {
        private bool _backpackEnabled = true;
        private StorageEntity _instance;
        private Logging _logger = new Logging(Logging.Prefix.COMPONENTS + "~BACKPACK");

        public static PlayerBackpack Instance { get; private set; }

        public bool IsOpen => Singleton<StorageMenu>.instance.IsOpen && Singleton<StorageMenu>.instance.TitleLabel.text == Config.StorageName.Value;

        private void Awake()
        {
            _instance = gameObject.GetComponentInParent<StorageEntity>();
            if (_instance == null)
            {
                _logger.Err("Player does not have a BackpackStorage component!");
                return;
            }

            if (_instance.SlotCount != Config.SlotCount.Value)
            {
                _logger.Warn("Backpack storage not initialized. Reinitializing.");
                _instance.SlotCount = Config.SlotCount.Value;
                _instance.DisplayRowCount = Config.RowCount.Value;
                _instance.StorageEntityName = Config.StorageName.Value;
                _instance.StorageEntitySubtitle = string.Empty;
                _instance.MaxAccessDistance = float.PositiveInfinity;
                for (var i = _instance.ItemSlots.Count; i < _instance.SlotCount; i++)
                {
                    var itemSlot = new ItemSlot();
                    itemSlot.onItemDataChanged.CombineImpl((Il2CppSystem.Action)_instance.ContentsChanged);
                    _instance.ItemSlots.Add(itemSlot);
                }
            }

            OnStartClient(true);
        }
        private void Update()
        {
            if (!Input.GetKeyDown(Config.ToggleKey.Value) || !_backpackEnabled)
                return;

            try
            {
                if (IsOpen)
                    Close();
                else
                    Open();
            }
            catch (Exception e)
            {
                Melon<Core>.Logger.Error("Error toggling backpack: " + e.Message);
            }
        }


        public void SetBackpackEnabled(bool enabled)
        {
            if (!enabled)
                Close();

            _backpackEnabled = enabled;
        }

        public void Open()
        {
            if (!_backpackEnabled || Singleton<ManagementClipboard>.Instance.IsEquipped || Singleton<StorageMenu>.Instance.IsOpen)
                return;

            var storageMenu = Singleton<StorageMenu>.Instance;
            storageMenu.SlotGridLayout.constraintCount = Config.RowCount.Value;
            storageMenu.Open(Config.StorageName.Value, string.Empty, _instance.Cast<IItemSlotOwner>());
            _instance.SendAccessor(Player.Local.NetworkObject);
        }

        public void Close()
        {
            if (!_backpackEnabled || !IsOpen)
                return;

            Singleton<StorageMenu>.Instance.CloseMenu();
            _instance.SendAccessor(null);
        }

        public void OnStartClient(bool isOwner)
        {
            if (!isOwner)
            {
                _logger.Msg("Desotrying non-local player singleton: " + name);
                Destroy(this);
                return;
            }

            if (Instance != null)
            {
                _logger.Warn($"Multiple instances of {name} exist. Keeping prior instance refrence.");
                return;
            }

            Instance = this;
        }

        public void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
