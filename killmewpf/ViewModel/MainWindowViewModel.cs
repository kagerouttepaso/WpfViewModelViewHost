using DynamicData;
using killmewpf.Ioc;
using killmewpf.Message;
using killmewpf.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace killmewpf.ViewModel
{
    public class MainWindowViewModel : ObservableObject, ITransientService, IDisposable
    {
        private readonly CompositeDisposable disposables = new();
        private readonly DataModel dataModel;


        public ReadOnlyObservableCollection<ISubViewModel> SubViewModels { get; }
        public void Dispose()
        {
            disposables.Dispose();
        }

        public string Title => "Sample";

        public AsyncReactiveCommand TestCommand { get; }

        private async Task<string> TestAsync()
        {
            await Task.Delay(1000);
            dataModel.SubViewModels.Add(new SubViewModel1());
            await messageBoxHandler.InvokeAsync("テスト");
            await Task.Delay(1000);
            await Task.Run(() =>
            {
                dataModel.SubViewModels
                    .AddRange(Enumerable
                        .Range(0, 1000)
                        .Select(x => new SubViewModel2(x)));
            });

            return "Comp";
        }

        private readonly IMessageBoxHandler messageBoxHandler;

        public MainWindowViewModel(DataModel dataModel, IMessageBoxHandler messageBoxHandler)
        {
            this.messageBoxHandler = messageBoxHandler;
            this.dataModel = dataModel;

            dataModel.SubViewModels
                .Connect()
                .ObserveOnUIDispatcher()
                .Bind(out var collection)
                .DisposeMany()
                .Subscribe()
                .AddTo(disposables);
            SubViewModels = collection;

            TestCommand = new AsyncReactiveCommand()
                .WithSubscribe(TestAsync);
        }
    }
}
