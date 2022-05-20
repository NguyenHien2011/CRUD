using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace XProject.Core.Configs
{
    public class SystemSettingModel
    {
        private string _assetsRootUrl;
        public static SystemSettingModel Instance { get; set; }
        public static CloudPhone CloudPhone { get; set; }

        public static IConfiguration Configs { get; set; }
        public string ApplicationName { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name;

        public string EncryptKey { get; set; }
        public string UrlConfig { get; set; }
        public string AppId { get; set; }
        public string Issuer { get; set; }
        public string DefaultPass { get; set; }

        public string Domain { get; set; }

        // public DateTimeOffset StartUpTime { get; set; } = CoreHelper.SystemTimeNow;

        public string VersionName { get; set; }

        public int AuthorizeCodeStorageSeconds { get; set; } = 600;

        public int AccessTokenJwtExpirationSeconds { get; set; } = 1800;

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string CookieAccessTokenKey { get; set; } = "AccessToken";

        public string CookieRefreshTokenKey { get; set; } = "RefreshToken";

        public string CookieEncryptedKey { get; set; } = "b1a4e68f03c54740bbd7b49d6b993d46";

        public string AssetsDomain
        {
            get => _assetsRootUrl;
            set => _assetsRootUrl = value?.Trim(' ', '/', '\\');
        }

        public bool IsEnforceHttps { get; set; }

        public string SecretKey { get; set; }
    }
    public class QueueSetting
    {
        public static QueueSetting Instance { get; set; }
        public QueueInfo TrackingStaff { get; set; }
    }

    public class QueueInfo
    {
        public string QueueName { get; set; }

        public string QueueTopic { get; set; }

        public int MaxWorker { get; set; }
    }

    public class CloudPhone
    {
        public string ServiceName { get; set; }
        public string AuthUser { get; set; }
        public string AuthKey { get; set; }
    }
}