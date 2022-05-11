using killmewpf.Ioc;
using killmewpf.View;
using killmewpf.ViewModel;
using MessagePipe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reactive.Bindings.Extensions;
using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using ZLogger;

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
            this.MainWindow.DataContext = host.Services.GetRequiredService<MainWindowViewModel>();
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

                    service.Scan(scan => scan
                        .FromApplicationDependencies()
                            .AddClasses(c => c.AssignableTo<ISingletonService>()).AsSelfWithInterfaces().WithSingletonLifetime()
                            .AddClasses(c => c.AssignableTo<ITransientService>()).AsSelfWithInterfaces().WithTransientLifetime());

                    service.AddMessagePipe(o =>
                    {
                        o.EnableAutoRegistration = true;
                        o.EnableCaptureStackTrace = true;
                    });
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
        }
    }
}
