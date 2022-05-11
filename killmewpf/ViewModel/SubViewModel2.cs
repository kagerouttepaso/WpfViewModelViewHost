using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace killmewpf.ViewModel
{
    public class SubViewModel2 : ObservableObject, ISubViewModel
    {
        public string Title => nameof(SubViewModel2);
    }
}
