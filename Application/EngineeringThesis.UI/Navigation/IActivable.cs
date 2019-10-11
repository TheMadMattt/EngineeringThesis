using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringThesis.UI.Navigation
{
    public interface IActivable
    {
        Task ActivateAsync(object parameter);
    }
}
