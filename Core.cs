using MelonLoader;

[assembly: MelonInfo(typeof(VorrikZ_Core.Core), "VorrikZ-Core", "1.5.13", "VorrikZ", null)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace VorrikZ_Core
{
    using VorrikZ_Core.Tools;
    public class Core : MelonMod
    {
        
        public override void OnInitializeMelon()
        {
            Logging _logger = new(Logging.Prefix.CORE);
            Config.Init();

            _logger.Msg("Initialized");
        }
    }
}