using Viewer.Library.ViewModels;

namespace Viewer.Library.Functions
{
    public class WindowFunction : FunctionBase
    {
        public override bool ApplyMove(FolderViewModel folder, double deltaX, double deltaY)
        {
            // TODO: make it relative to folder-size (depending on resolution)?
            folder.CurrentWindowWidth += deltaX;
            folder.CurrentWindowCenter += deltaY;

            return true;
        }
    }
}
