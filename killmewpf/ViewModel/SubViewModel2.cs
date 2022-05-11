using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace killmewpf.ViewModel
{
    public class SubViewModel2 : ObservableObject, ISubViewModel
    {
        private readonly int _id;

        public SubViewModel2(int id)
        {
            _id = id;
        }

        public string Title => nameof(SubViewModel2) + _id.ToString();
    }
}
