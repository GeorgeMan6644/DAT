using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;

namespace DeepAssTranslator
{
    public static class Configuration
    {
        public const string NEW_LINE = "\\N";
        public const string TRANSLATED_FILE_SUFFIX = "_translated.ass";

        public static readonly string AuthenticationKey;
        public static readonly bool IDeepLApiPro;
        public static readonly string TargetLang;
        public static readonly string SourceLang;

        static Configuration()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration config = builder.SetBasePath(GetBasePath()).AddJsonFile("appsettings.json", false).Build();
            
            AuthenticationKey = config["authentication_key"];
            IDeepLApiPro = bool.Parse(config["is_DeepL_API_Pro"]);
            TargetLang = config["target_lang"];
            SourceLang = config["source_lang"];
        }

        public static string GetApiEndpoint()
        {
            if (IDeepLApiPro)
            {
                return "https://api.deepl.com/v2/translate";
            }

            return "https://api-free.deepl.com/v2/translate";
        }

        public static string GetBasePath()
        {
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }
    }
}
