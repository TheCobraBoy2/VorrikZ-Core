using MelonLoader;

[assembly: MelonInfo(typeof(VorrikZ_Core.Core), "VorrikZ-Core", "1.5.12", "VorrikZ", null)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace VorrikZ_Core
{
    using VorrikZ_Core.Tools;
    public class Core : MelonMod
    {
        private Logging logger = new Logging();

        public override void OnInitializeMelon()
        {
            Config.Init();

            logger.Msg(Logging.Prefix.CORE, "Initialized");
        }
    }
}