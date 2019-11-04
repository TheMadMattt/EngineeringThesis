using System.Threading.Tasks;

namespace EngineeringThesis.UI.Navigation
{
    public interface IActivable
    {
        Task ActivateAsync(object parameter);
    }
}
