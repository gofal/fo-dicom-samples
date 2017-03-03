using System;
using Viewer.Library.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Viewer.Universal.Controls
{
    public class FolderPanel : Panel
    {

        // Measure each children and give as much room as they want 
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement elem in Children)
            {
                try
                {
                    if (elem is ContentPresenter)
                    {
                        var content = elem as ContentPresenter;
                        if (content.Content is FolderViewModel)
                        {
                            FolderViewModel folderElem = content.Content as FolderViewModel;
                            var newSize = new Size(folderElem.RelativeWidth * availableSize.Width, folderElem.RelativeHeigh * availableSize.Height);
                            elem.Measure(newSize);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                }
            }
            return base.MeasureOverride(availableSize);
        }

        //Arrange all children based on the property for location
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0)
                return finalSize;

            foreach (UIElement elem in Children)
            {
                try
                {
                    if (elem is ContentPresenter)
                    {
                        var content = elem as ContentPresenter;
                        if (content.Content is FolderViewModel)
                        {
                            // if the control is bound to a FolderViewModel, then the properties for the Position are read and used here
                            FolderViewModel folderElem = content.Content as FolderViewModel;
                            var newRect = new Rect(folderElem.RelativePositionX * finalSize.Width, folderElem.RelativePositionY * finalSize.Height, folderElem.RelativeWidth * finalSize.Width, folderElem.RelativeHeigh * finalSize.Height);
                            elem.Arrange(newRect);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                }
            }

            return finalSize;
        }

    }
}
