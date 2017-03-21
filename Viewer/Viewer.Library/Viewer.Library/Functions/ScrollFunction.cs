using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.Library.ViewModels;

namespace Viewer.Library.Functions
{
    public class ScrollFunction : FunctionBase
    {
        public override bool ApplyMove(FolderViewModel folder, double deltaX, double deltaY)
        {
            folder.CurrentImageIndex += (int)Math.Round(deltaY * 5);

            return true;
        }
    }
}
