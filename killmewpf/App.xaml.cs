using killmewpf.View;
using killmewpf.ViewModel;
using System;
using System.Windows;
using System.Windows.Data;

namespace killmewpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 直接コードで登録する方法もある
            this.Resources.RegistorDataTemplate<SubView1, SubViewModel1>();

            this.MainWindow = new MainWindow();
            this.MainWindow.Show();
        }
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
}
