using System.Collections.ObjectModel;

namespace killmewpf.ViewModel
{
    public class MainWindowViewModel
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
    }
}
