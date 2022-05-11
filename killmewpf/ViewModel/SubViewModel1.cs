using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace killmewpf.ViewModel
{
    public class SubViewModel1 : ObservableObject, ISubViewModel
    {
        public string Name => nameof(SubViewModel1);
    }
}
