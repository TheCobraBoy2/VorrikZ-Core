using System.Net.NetworkInformation;
using MelonLoader;

namespace VorrikZ_Core.Tools
{
    public class Logging(string Prefix = "MODULE~UNKNOWN")
    {
        private readonly MelonLogger.Instance _logger = Melon<Core>.Logger;
        private string defaultLogPrefix = Prefix;

        private string GetPrefix(string logPrefix)
        {
            if (logPrefix == null)
            {
                return defaultLogPrefix;
            }
            else
            {
                return logPrefix;
            }
        }

        public class Prefix
        {
            public static readonly string PATCH = "MODULE~PATCHER";
            public static readonly string CORE = "MODULE~CORE";
            public static readonly string COMPONENTS = "MODULE~COMPONENTS";
        }
        
        public bool Msg(string Message, string logPrefix=null)
        {
            try
            {
                _logger.Msg($"[{GetPrefix(logPrefix)}]: \"{Message}\"");
                return true;
            }
            catch (Exception e)
            {
                _logger.Warning(e);
                return false;
            }
        }

        public bool MsgNoQuotes(string Message, string logPrefix=null)
        {
            try
            {
                _logger.Msg($"{GetPrefix(logPrefix)}: {Message}");
                return true;
            }
            catch (Exception e)
            {
                _logger.Warning(e);
                return false;
            }
        }

        public bool Err(string Message, string logPrefix = null)
        {
            try
            {
                _logger.Error($"[{GetPrefix(logPrefix)} | ERROR]: \"{Message}\"");
                return true;
            }
            catch (Exception e)
            {
                _logger.Warning(e);
                return false;
            }
        }

        public bool ErrNoQuotes(string Message, string logPrefix=null)
        {
            try
            {
                _logger.Error($"[{GetPrefix(logPrefix)} | ERROR]: {Message}");
                return true;
            }
            catch (Exception e)
            {
                _logger.Warning(e);
                return false;
            }
        }

        public bool Warn(string Message, string logPrefix=null)
        {
            try
            {
                _logger.Warning($"[{GetPrefix(logPrefix)} | WARNING]: \"{Message}\"");
                return true;
            }
            catch (Exception e)
            {
                _logger.Warning(e);
                return false;
            }
        }

        public bool WarnNoQuotes(string Message, string logPrefix=null)
        {
            try
            {
                _logger.Warning($"[{GetPrefix(logPrefix)} | WARNING]: {Message}");
                return true;
            }
            catch (Exception e)
            {
                _logger.Warning(e);
                return false;
            }
        }
    }
}
