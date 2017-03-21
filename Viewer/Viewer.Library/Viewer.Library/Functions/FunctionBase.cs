using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.Library.ViewModels;

namespace Viewer.Library.Functions
{
    public abstract class FunctionBase
    {

        public abstract bool ApplyMove(FolderViewModel folder, double deltaX, double deltaY);
    }
}
