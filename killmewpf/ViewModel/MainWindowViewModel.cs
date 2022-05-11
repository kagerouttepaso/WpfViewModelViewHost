using killmewpf.Ioc;
using killmewpf.Message;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace killmewpf.ViewModel
{
    public class MainWindowViewModel : ObservableObject, ITransientService
    {
        private ObservableCollection<ISubViewModel> _subViewModels = new ObservableCollection<ISubViewModel>()
        {
            new SubViewModel1(),
            new SubViewModel2(),
            new SubViewModel1(),
            new SubViewModel1(),
        };


        public ObservableCollection<ISubViewModel> SubViewModels
        {
            get => _subViewModels;
            set => _subViewModels = value;
        }

        public string Title => "Sample";

        public AsyncReactiveCommand TestCommand { get; }

        private async Task<string> TestAsync()
        {
            await Task.Delay(1000);
            _subViewModels.Add(new SubViewModel1());
            await messageBoxHandler.InvokeAsync("テスト");
            await Task.Delay(1000);
            _subViewModels.Add(new SubViewModel2());

            return "Comp";
        }

        private readonly IMessageBoxHandler messageBoxHandler;

        public MainWindowViewModel(IMessageBoxHandler messageBoxHandler)
        {
            this.messageBoxHandler = messageBoxHandler;
            TestCommand = new AsyncReactiveCommand()
                .WithSubscribe(TestAsync);
        }
    }
}
