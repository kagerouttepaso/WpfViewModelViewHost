using killmewpf.View;
using killmewpf.ViewModel;
using System.Reactive.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Data;
using ZLogger;
using Reactive.Bindings.Extensions;

namespace killmewpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly CompositeDisposable disposables = new();
        private IHost host;
        private ILogger<App> logger;


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            host = BootStrap(e.Args);
            host.AddTo(disposables);

            logger = host.Services.GetRequiredService<ILogger<App>>();
            logger.ZLogInformation("app boot");
            // base.OnStartup(e);

            host.Start();

            // 直接コードで登録する方法もある
            this.Resources.RegistorDataTemplate<SubView1, SubViewModel1>();

            this.MainWindow = host.Services.GetRequiredService<MainWindow>();
            this.MainWindow.Show();
        }

        private IHost BootStrap(string[] args)
        {
            var appConst = new AppConst();
            var logDir = new DirectoryInfo(appConst.LogDirName);
            logDir.Create();

            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, configure) =>
                {
                    configure.SetBasePath(context.HostingEnvironment.ContentRootPath);
                    configure.AddJsonFile(appConst.ConfigFileName, true, true);
                    configure.AddCommandLine(args);
                })
                .ConfigureServices((context, service) =>
                {
                    service.AddSingleton(appConst);
                    service.Configure<AppConfig>(context.Configuration);

                    service.AddSingleton<MainWindow>();
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Debug);

                    logging.AddZLoggerRollingFile(
                        (dt, x) => $"{appConst.LogDirName}/{dt.ToLocalTime():yyyy-MM-dd}_{x:000}.log",
                        x => x.ToLocalTime().Date,
                        1024 * 1024);

                    logging.AddDebug();
                })
                .Build();


            return host;
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            logger.ZLogInformation("app exit");
            await host.StopAsync(TimeSpan.FromSeconds(5));
            disposables.Dispose();
            // base.OnExit(e);
        }
    }
    internal class AppConfig
    {
    }

    public static class IViewForExtensions
    {
        public static void RegistorDataTemplate<TView, TViewModel>(this ResourceDictionary resourceDictionary)
            where TView : class
            where TViewModel : class
        {
            var dt = new DataTemplate()
            {
                DataType = typeof(TViewModel),
            };

            var vt = new FrameworkElementFactory(typeof(TView));
            vt.SetBinding(SubView1.DataContextProperty, new Binding());
            dt.VisualTree = vt;

            resourceDictionary.Add(dt.DataTemplateKey, dt);
        }
    }


    public class AppConst
    {
        public string LogDirName => "Log";

        public string ConfigFileName => "hostConfig.json";
    }
}
