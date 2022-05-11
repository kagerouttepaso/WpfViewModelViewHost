using DynamicData;
using killmewpf.Ioc;
using killmewpf.ViewModel;

namespace killmewpf.Model
{
    public class DataModel : ISingletonService
    {
        public SourceList<ISubViewModel> SubViewModels { get; } = new();
        public DataModel()
        {
            SubViewModels.Add(new SubViewModel1());
            SubViewModels.Add(new SubViewModel2(0));
        }
    }
}
