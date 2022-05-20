using Autofac;
using Flurl.Http;
using Flurl.Http.Configuration;
using Hangfire;
using Hangfire.SqlServer;
using Invedia.Core.ConfigUtils;
using Invedia.DI;
using Invedia.Web.Middlewares.HttpContextMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.IO;
using XProject.Contract.Repository.Infrastructure;
using XProject.Core.Configs;
using XProject.Core.Constants;
using XProject.Core.Utils;
using XProject.Repository.Infrastructure;
using XProject.WebApi.Extension;
using XProject.WebApi.Filters.Validation;
using XProject.WebApi.Modules;

namespace XProject.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "XProject.WebApi", Version = "v1" });
                //c.OperationFilter<>();
                c.OperationFilter<FileUploadOperation>();
            });

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddApplicationInsightsTelemetry();
            services.AddHealthChecks();
            services.AddSystemSetting(Configuration.GetSection<SystemSettingModel>("SystemSetting"));
            //services.AddQueueSetting(Configuration.GetSection<QueueSetting>("QueueSetting"));
            //services.AddCloudPhoneSetting(Configuration.GetSection<CloudPhone>("CloudPhone"));
            services.AddInvediaHttpContext();
            services.AddDataProtection();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(SystemHelper.AppDb));

            //Repository service

            //Service layer service

            //UnitOfWork service
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddControllers().AddNewtonsoftJson().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

            // 1----
            services.AddControllers();
            services.AddControllersWithViews();
            services.AddMvcCore();
            // 1----


            // Auto Register Dependency Injection
            services.AddDI();
            services.PrintServiceAddedToConsole();
            // Flurl Config
            FlurlHttp.Configure(config =>
            {
                config.JsonSerializer = new NewtonsoftJsonSerializer(Formattings.JsonSerializerSettings);
            });
            // Add Hangfire
            services.AddHangfire(configuration => configuration
              .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
              .UseSimpleAssemblyNameTypeSerializer()
              .UseRecommendedSerializerSettings()
              .UseSqlServerStorage(SystemHelper.AppDb,
                  new SqlServerStorageOptions
                  {
                      CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                      SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                      QueuePollInterval = TimeSpan.Zero,
                      UseRecommendedIsolationLevel = true,
                      DisableGlobalLocks = true
                  }));

            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            app.UseCors("MyPolicy");

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });








            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Files")),
                RequestPath = "/Files"
            });





            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var swagger = Configuration.GetValue("UseSwagger", false);
            if (swagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DefaultModelsExpandDepth(-1);
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "XProject v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSerilogRequestLogging();

            // System Setting
            app.UseSystemSetting();

            app.UseInvediaHttpContext();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("health");
                endpoints.MapControllerRoute(
                    name: "area", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[]
    {
                    new HangfireAuthorizationFilter
                    {
                        User = "xproject",
                        Pass = "login@2022"
                    }
                },

                // IsReadOnlyFunc = _ => true
            });
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }
    }
}