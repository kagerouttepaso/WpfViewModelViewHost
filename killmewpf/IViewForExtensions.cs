using killmewpf.View;
using System.Windows;
using System.Windows.Data;

namespace killmewpf
{
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
