using Il2CppMono.Btls;
using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;
using VorrikZ_Core.ConfigUtils;

namespace VorrikZ_Core
{
    namespace ConfigUtils
    {
        public class Entry<T>
        {
            private readonly MelonPreferences_Entry<T> _entry;
            public MelonPreferences_Entry<T> Raw => _entry;

            public Entry(MelonPreferences_Entry<T> entry)
            {
                _entry = entry;
            }

            public T Value
            {
                get => _entry.Value;
                set => _entry.Value = value;
            }

            public void Save()
            {
                _entry.Save();
            }
        }

        public class Category
        {
            private readonly MelonPreferences_Category _category;

            public string Name => _category.Identifier;

            public Category(string name, string filename = null)
            {
                _category = MelonPreferences.CreateCategory(name);
                string _directory = Path.Combine(MelonEnvironment.UserDataDirectory, "Vorrikz", "Core");
                string _path = Path.Combine(_directory, (filename ?? name) + ".cfg");
                Directory.CreateDirectory(_directory);
                _category.SetFilePath(_path);
            }

            public Entry<T> CreateEntry<T>(string key, T defaultValue, string description = "")
            {
                var entry = _category.CreateEntry(key, defaultValue, description);
                return new Entry<T>(entry);
            }

            public void Save(bool printmsg=true) => _category.SaveToFile(printmsg);
        }
    }

    public class Config
    {
        // Categories
        public static Category TestCategory { get; private set; }
        public static Category StackSizeCategory { get; private set; }
        public static Category MixerCategory { get; private set; }
        public static Category DryingRack { get; private set; }
        public static Category Backpack { get; private set; }

        // Entries
        public static Entry<bool> TestEntry { get; private set; }
        public static Entry<int> MiscStackSize { get; private set; }
        public static Entry<int> ProductStackSize { get; private set; }
        public static Entry<int> IngredientStackSize { get; private set; }
        public static Entry<int> GrowingStackSize { get; private set; }
        public static Entry<int> PackagingStackSize { get; private set; }
        public static Entry<int> ToolsStackSize { get; private set; }
        public static Entry<int> LightingStackSize { get; private set; }
        public static Entry<int> EquipmentStackSize { get; private set; }
        public static Entry<int> FurnitureStackSize { get; private set; }
        public static Entry<int> MixerStackSize { get; private set; }
        public static Entry<int> MixerTimePerItem { get; private set; }
        public static Entry<int> DryingRackSize { get; private set; }
        public static Entry<int> SlotCount { get; private set; }
        public static Entry<int> RowCount { get; private set; }
        public static Entry<string> StorageName { get; private set; }
        public static Entry<bool> MixerEnable { get; private set; }
        public static Entry<bool> DryingRackEnable { get; private set; }

        public static void Init()
        {
            // Categories
            TestCategory = new Category("Test Category", "Testing");
            StackSizeCategory = new Category("Stack Sizes", "StackSize");
            MixerCategory = new Category("Mixer", "StackSize");
            DryingRack = new Category("Drying Rack", "StackSize");
            Backpack = new Category("Backpack");

            // Entries
            TestEntry = TestCategory.CreateEntry<bool>("Test1", true, "Test Entry");
            MiscStackSize = StackSizeCategory.CreateEntry<int>("Stack Size of all other un-included items like future categories", 100);
            ProductStackSize = StackSizeCategory.CreateEntry<int>("Product Stack Size (bagged and unbagged products, jarred products, etc...)", 100);
            IngredientStackSize = StackSizeCategory.CreateEntry<int>("Ingredient Stack Size (horse semen, mouth wash, all Consumables like Cuke/Energy Drink, etc)", 100);
            GrowingStackSize = StackSizeCategory.CreateEntry<int>("Growing Stack Size (Fertilizer, seeds, soil, etc)", 100);
            PackagingStackSize = StackSizeCategory.CreateEntry<int>("Packaging Stack Size (Jars/Baggies)", 100);
            ToolsStackSize = StackSizeCategory.CreateEntry<int>("Tools Stack Size (trash bags, all other tools are disallowed from stacking)", 100);
            LightingStackSize = StackSizeCategory.CreateEntry<int>("Lighting Stack Size (Grow lights like LED lights)", 100);
            EquipmentStackSize = StackSizeCategory.CreateEntry<int>("Equipment Stack Size (packaging station, mixing station, etc)", 100);
            FurnitureStackSize = StackSizeCategory.CreateEntry<int>("Furniture Stack Size (table and other various forms of non-table furnitures)", 100);
            MixerStackSize = MixerCategory.CreateEntry<int>("OutputSize", 100);
            MixerTimePerItem = MixerCategory.CreateEntry<int>("MixerTimePerItem", 1);
            MixerEnable = MixerCategory.CreateEntry<bool>("Enable Mixer stack size change", true);
            DryingRackSize = DryingRack.CreateEntry<int>("OutputSize", 100);
            DryingRackEnable = DryingRack.CreateEntry<bool>("Enable Drying Rack stack size change", true);
            SlotCount = Backpack.CreateEntry<int>("Slots", 12);
            RowCount = Backpack.CreateEntry<int>("Rows", 3);
            StorageName = Backpack.CreateEntry<string>("Name", "Backpack");

            // Saving
            TestCategory.Save();
            StackSizeCategory.Save();
            MixerCategory.Save();
            DryingRack.Save();
            Backpack.Save();
        }

        public static void Save(Category _category)
        {
            _category.Save();
        }
    }
}
