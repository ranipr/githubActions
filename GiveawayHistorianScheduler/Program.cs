using GiveawayHistorianScheduler.Interface;
using Fugu.AsciiArt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;

namespace GiveawayHistorianScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            args = new string[2] {

                                        "--SqlServerConstr abc",
                                        "--SPName xyz"
                        };
#endif

            using IHost host = CreateHostBuilder(args).Build();
            var config = host.Services.GetService<IConfig>();
            var appConfigService = host.Services.GetService<IAppConfig<IConfig>>();
            appConfigService.GetAppConfig(args, config);
            var historianScheduler = host.Services.GetService<IHistorianScheduler>();

            host.RunAsync();

            Console.WriteLine(new Figlet("Giveaway Historian Scheduler", FigletFont.Shadow).ToString());
            //Console.WriteLine(new Figlet("v" + Assembly.GetEntryAssembly().GetName().Version, FigletFont.Larry3D));
            Console.WriteLine(appConfigService.GetPropertyInfoString(config));
            Console.WriteLine("Execution Started");
            historianScheduler.call_HistorianSP();
            Console.WriteLine("Execution Ended");

        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureServices((_, services) => services.AddSingleton<IConfig, Config>())
              .ConfigureServices((_, services) => services.AddScoped<IAppConfig<IConfig>, AppConfigReader<IConfig>>())
             .ConfigureServices((_, services) => services.AddScoped<IHistorianScheduler, HistorianScheduler>())
          ;
    }
}
