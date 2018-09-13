using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PR.Helpers;
using PR.Helpers.AzRest;
using PR.Helpers.Contract;
using PR.Helpers.Models;
using PR.Helpers.Telemetry.Xamling.Azure.Logger;
using PR.Helpers.Validators;
using PR.Helpers.VSTS;
using PR.Helpers.Workers;
using TemplateBuilder = Microsoft.AspNetCore.Mvc.ViewFeatures.Internal.TemplateBuilder;

namespace PRServer.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<Secrets>(Configuration.GetSection(nameof(Secrets)));
            services.Configure<Settings>(Configuration.GetSection(nameof(Settings)));

            services.Configure<AppInsightsSettings>(Configuration.GetSection("ApplicationInsights"));
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<ISecurityHelpers, SecurityHelpers>();
            
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddTransient<IRestValidateCheck, RestValidateCheck>();
            services.AddSingleton<ITemplateBuilder, PR.Helpers.AzRest.TemplateBuilder>();
            services.AddSingleton<IValidatorRunner, ValidatorRunner>();
            services.AddSingleton<IVstsHelper, VstsHelper>();
            services.AddSingleton<IBasicJsonValidator, BasicJsonValidator>();
            services.AddSingleton<IARMValidator, ARMValidator>();
            services.AddSingleton<IPRProcessor, PRProcessor>();

            services.AddHostedService<QueuedHostedService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
