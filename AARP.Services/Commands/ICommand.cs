using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services.Commands
{
    public interface ICommand
    {
       bool CanExecute();

       void Execute();
    }
}
