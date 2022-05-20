using System.Threading.Tasks;
using XProject.Core.Configs;
using Invedia.Web.HttpUtils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace XProject.WebApi
{
    public static class StartupSystemSetting
    {
        public static IServiceCollection AddSystemSetting(this IServiceCollection services, SystemSettingModel systemSettingModel)
        {
            SystemSettingModel.Instance = systemSettingModel ?? new SystemSettingModel();

            return services;
        }

        public static IServiceCollection AddCloudPhoneSetting(this IServiceCollection services, CloudPhone cloudPhoneModel)
        {
            SystemSettingModel.CloudPhone = cloudPhoneModel ?? new CloudPhone();

            return services;
        }

        public static IServiceCollection AddQueueSetting(this IServiceCollection services, QueueSetting QueueSettingModel)
        {
            QueueSetting.Instance = QueueSettingModel ?? new QueueSetting();

            return services;
        }

        public static IApplicationBuilder UseSystemSetting(this IApplicationBuilder app)
        {
            app.UseMiddleware<SystemSettingMiddleware>();

            return app;
        }

        public class SystemSettingMiddleware
        {
            private readonly RequestDelegate _next;

            public SystemSettingMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public Task Invoke(HttpContext context)
            {
                SystemSettingModel.Instance.Domain = context.Request.GetDomain();

                return _next(context);
            }
        }
    }
}