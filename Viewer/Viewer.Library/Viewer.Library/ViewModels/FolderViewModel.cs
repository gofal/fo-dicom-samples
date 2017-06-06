using Dicom;
using Dicom.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Viewer.Library.Dicom;
using Viewer.Library.Models;

namespace Viewer.Library.ViewModels
{

    public class FolderViewModel : INotifyPropertyChanged
    {

        #region FIELDS

        ShellViewModel _mainModel;

        private int _currentImageIndex;

        private string _currentSeriesUID;

        private List<DisplayImage> _currentImages = new List<DisplayImage>();

        private SeriesModel _currentSerie;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors

        public FolderViewModel(ShellViewModel mainModel)
        {
            _mainModel = mainModel;
        }

        #endregion

        #region Properties

        public double RelativePositionX { get; set; }
        public double RelativePositionY { get; set; }
        public double RelativeWidth { get; set; }
        public double RelativeHeigh { get; set; }

        public double AbsoluteWidth { get; set; }
        public double AbsoluteHeight { get; set; }

        /// <summary>
        /// the currently selected DicomFile
        /// </summary>
        public DicomDataset File
            => this.NumberOfImages > 0 ? _currentImages[Math.Max(CurrentImageIndex - 1, 0)].Dataset : null;

        /// <summary>
        /// The Number of Images in the currently selected Series
        /// </summary>
        public int NumberOfImages => _currentImages?.Count ?? 0;

        /// <summary>
        /// The SeriesDescription of the currently selected series
        /// </summary>
        public string CurrentSeriesDescription => _currentSerie?.SeriesDescription ?? string.Empty;

        /// <summary>
        /// the index of the current image within the currently selected series
        /// </summary>
        public int CurrentImageIndex
        {
            get
            {
                return _currentImageIndex;
            }
            set
            {
                _currentImageIndex = Math.Max(Math.Min(value, NumberOfImages), 0);
                RaisePropertyChanged(nameof(CurrentImageIndex));
                RaisePropertyChanged(nameof(CurrentImage));
                RaisePropertyChanged(nameof(File));
            }
        }

        /// <summary>
        /// The UID of the currently selected series
        /// </summary>
        public string CurrentSeriesInstanceUID
        {
            get
            {
                return _currentSeriesUID;
            }
            set
            {
                _currentSeriesUID = value;
                var newSerie = _mainModel.Patients.FindSerie(_currentSeriesUID);
                SetNewCurrentSerie(newSerie);
                _currentImageIndex = 0;
                RaisePropertyChanged(nameof(NumberOfImages));
                RaisePropertyChanged(nameof(_currentImageIndex));
                RaisePropertyChanged(nameof(CurrentSeriesInstanceUID));
                RaisePropertyChanged(nameof(CurrentImage));
                RaisePropertyChanged(nameof(CurrentSeriesDescription));
                RaisePropertyChanged(nameof(File));
            }
        }

        /// <summary>
        /// the ImageSource of the current image
        /// </summary>
        public DisplayImage CurrentImage
        {
            get
            {
                if (NumberOfImages > 0)
                {
                    // the ImageSource is rendered each time, because here on this point the user-interactions like
                    // windowing, zooming, panning, rotation etc should be evaluated. 
                    // maybe there will be some sort of caching necessary, but currently performance is fine
                    DisplayImage dcmFile = _currentImages[Math.Max(CurrentImageIndex - 1, 0)];
                    return dcmFile;
                }
                else
                    return null;
            }
        }

        public double CurrentWindowWidth
        {
            get
            {
                if (NumberOfImages > 0)
                {
                    DisplayImage dcmFile = _currentImages[Math.Max(CurrentImageIndex - 1, 0)];
                    return dcmFile.WindowWidth;
                }
                else
                    return 0;
            }
            set
            {
                if (NumberOfImages > 0)
                {
                    foreach (DisplayImage image in _currentImages)
                    {
                        image.WindowWidth = value;
                    }
                    RaisePropertyChanged(nameof(CurrentImage));
                }
            }
        }

        public double CurrentWindowCenter
        {
            get
            {
                if (NumberOfImages <= 0) return 0;
                DisplayImage dcmFile = _currentImages[Math.Max(CurrentImageIndex - 1, 0)];
                return dcmFile.WindowCenter;
            }
            set
            {
                if (NumberOfImages <= 0) return;
                foreach (DisplayImage image in _currentImages)
                    image.WindowCenter = value;
                RaisePropertyChanged(nameof(CurrentImage));
            }
        }

        #endregion

        #region public Methods

        public void ApplyFunctionMove(double relativeDeltaX, double relativeDeltaY)
        {
            bool redraw = _mainModel.DefaultFunction?.ApplyMove(this, relativeDeltaX, relativeDeltaY) ?? false;
            if (redraw) RaisePropertyChanged(nameof(CurrentImage));
        }

        public void ApplyZoomMove(double relativeDeltaX, double relativeDeltaY)
        {
            if (CurrentImage == null) return;
            CurrentImage.Scale *= Math.Pow(1.1, relativeDeltaY / AbsoluteHeight);
            RaisePropertyChanged(nameof(CurrentImage));
        }

        #endregion

        #region private Methods

        private void SetNewCurrentSerie(SeriesModel newSerie)
        {
            _currentImages.Clear();
            _currentSerie = newSerie;
            // if the new current serie is null then there is no initialization of new DisplayImages
            if (_currentSerie == null) return;
            foreach (DicomImage image in _currentSerie.Images)
            {
                for (int i = 0; i < image.NumberOfFrames; i++)
                {
                    // There is a new DisplayImage for every displayable image, that means for every frame
                    _currentImages.Add(new DisplayImage(image, i));
                }
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }

}
