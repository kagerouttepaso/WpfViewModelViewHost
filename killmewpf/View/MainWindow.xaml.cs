using killmewpf.Ioc;
using killmewpf.Message;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace killmewpf.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMessageBoxHandler, ISingletonService
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public async ValueTask<MessageBoxResult?> InvokeAsync(string request, CancellationToken cancellationToken = default)
        {
            MessageBoxResult? ret = null;
            await this.Dispatcher.InvokeAsync(() =>
            {
                ret = MessageBox.Show(request);
            });

            return ret;
        }
    }
}
