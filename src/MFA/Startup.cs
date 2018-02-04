using MFA.Entities.Configurations;
using MFA.IInfrastructure;
using MFA.Infrastructure;
using MFA.IService;
using MFA.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MFA
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddOptions();

            // Register the IConfiguration instance which MyOptions binds against.
            services.Configure<AzureConfiguration>(Configuration.GetSection(nameof(AzureConfiguration)));

            // Add Infrastructure services
            services.AddSingleton<IHttpClientsFactory, HttpClientsFactory>();
            services.AddSingleton<IWaitCall, WaitCall>();

            // Add Application services
            services.AddTransient<IImageStorageService, ImageStorageService>();
            services.AddTransient<IImageAnalysisService, ImageAnalysisService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "OnlyAction",
                    "{action}",
                    new { controller = "Home", action = "Index" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
