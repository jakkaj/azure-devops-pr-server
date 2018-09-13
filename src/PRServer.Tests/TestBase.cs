using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using PR.Helpers.Models;
 using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Options;
using PR.Helpers;
using PR.Helpers.AzRest;
using PR.Helpers.Contract;
using PR.Helpers.Telemetry.Xamling.Azure.Logger;
using PR.Helpers.Validators;
using PR.Helpers.VSTS;
using PR.Helpers.Workers;

namespace PRServer.Tests
{
    public class TestBase
    {
        public static IOptions<Secrets> SecretOptions;
        public static Settings AppSettings;
        public static ServiceProvider Services { get; set; }
        static TestBase()
        {
            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                devEnvironmentVariable.ToLower() == "development";
            //Determines the working environment as IHostingEnvironment is unavailable in a console app

            var builder = new ConfigurationBuilder();
            // tell the builder to look for the appsettings.json file
            builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //only add secrets in development
            if (isDevelopment)
            {
                builder.AddUserSecrets<TestBase>();
            }

            var Configuration = builder.Build();

            var services = new ServiceCollection();
            
            //Map the implementations of your classes here ready for DI
            var serviceProvider = services
                .Configure<Secrets>(Configuration.GetSection(nameof(Secrets)))
                .Configure<Settings>(Configuration.GetSection(nameof(Settings)))
                .Configure<AppInsightsSettings>(Configuration.GetSection("ApplicationInsights"))
                .AddSingleton<ISecurityHelpers, SecurityHelpers>()
                .AddTransient<IRestValidateCheck, RestValidateCheck>()
                .AddSingleton<IPRProcessor, PRProcessor>()
                .AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>()
                .AddSingleton<IValidatorRunner, ValidatorRunner>()
                .AddSingleton<IBasicJsonValidator, BasicJsonValidator>()
                .AddSingleton<IARMValidator, ARMValidator>()
                .AddSingleton<QueuedHostedService>()
                .AddSingleton<ILogService, LogService>()
                .AddSingleton<ITemplateBuilder, TemplateBuilder>()
                .AddSingleton<IVstsHelper, VstsHelper>()
                .AddOptions()
                .BuildServiceProvider();

            Services = serviceProvider;
            
            var opts = serviceProvider.GetService<IOptions<Secrets>>();

            SecretOptions = opts;

            AppSettings = serviceProvider.GetService<IOptions<Settings>>().Value;
        }

        public static T Resolve<T>()
        {
            return Services.GetService<T>();
        }
    }
}
