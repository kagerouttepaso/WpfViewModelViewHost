using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace killmewpf.ViewModel
{
    public class MainWindowViewModel : ObservableObject
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
            await Task.Delay(1000);
            _subViewModels.Add(new SubViewModel2());

            return "Comp";
        }


        public MainWindowViewModel()
        {
            TestCommand = new AsyncReactiveCommand()
                .WithSubscribe(TestAsync);
        }
    }

}
