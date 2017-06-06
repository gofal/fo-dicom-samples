using Dicom;
using Dicom.Imaging;
using System;
using Viewer.Library.Tools;

namespace Viewer.Library.Dicom
{
    public class DisplayImage : IDisposable
    {

        private DicomImage _dicomImage;
        private bool _disposed;
        private IImage _cachedImage;
        private int _frame;
        private ImageSizer _sizer;


        static DisplayImage()
        {
        }


        public DisplayImage(DicomDataset dataset, int frame = 0)
        {
            _dicomImage = new DicomImage(dataset, frame);
            _frame = frame;
            InitValues();
        }

        public DisplayImage(string fileName, int frame = 0)
        {
            _dicomImage = new DicomImage(fileName, frame);
            _frame = frame;
            InitValues();
        }

        public DisplayImage(DicomImage image, int frame = 0)
        {
            _dicomImage = image;
            _frame = frame;
            InitValues();
        }

        ~DisplayImage()
        {
            Dispose(false);
        }

        private double _windowCenter;
        private double _windowWidth;
        private Color32[] _grayscaleColorMap;
        private double _scale;
        private bool _showOverlay;
        private int _overlayColor;

        public DicomDataset Dataset => _dicomImage.Dataset;


        /// <summary>
        /// Clears and disposes the cached IImage
        /// </summary>
        public void ClearCachedImage()
        {
            if (_cachedImage != null)
                _cachedImage.Dispose();
            _cachedImage = null;
        }

        private void InitValues()
        {
            _windowCenter = _dicomImage.WindowCenter;
            _windowWidth = _dicomImage.WindowWidth;
            _grayscaleColorMap = _dicomImage.GrayscaleColorMap;
            _scale = _dicomImage.Scale;
            _showOverlay = _dicomImage.ShowOverlays;
            _overlayColor = _dicomImage.OverlayColor;
            _sizer = new ImageSizer() { SourceWith = _dicomImage.Width, SourceHeight = _dicomImage.Height, DestinationWidth = _dicomImage.Width, DestinationHeight = _dicomImage.Height }; // todo: cache this ImageSizer
        }

        private void SetValues()
        {
            _dicomImage.WindowCenter = _windowCenter;
            _dicomImage.WindowWidth = _windowWidth;
            _dicomImage.GrayscaleColorMap = _grayscaleColorMap;
            _dicomImage.Scale = _scale;
            _dicomImage.ShowOverlays = _showOverlay;
            _dicomImage.OverlayColor = _overlayColor;
        }

        public double WindowCenter
        {
            get { return _windowCenter; }
            set
            {
                if (_windowCenter == value) return;
                _windowCenter = value;
                ClearCachedImage();
            }
        }

        public double WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                if (_windowWidth == value) return;
                _windowWidth = value;
                ClearCachedImage();
            }
        }

        public Color32[] GrayscaleColorMap
        {
            get { return _grayscaleColorMap; }
            set
            {
                _grayscaleColorMap = value;
                ClearCachedImage();
            }
        }

        public int OverlayColor
        {
            get { return _overlayColor; }
            set
            {
                if (_overlayColor == value) return;
                _overlayColor = value;
                ClearCachedImage();
            }
        }

        public double Scale
        {
            get { return _scale; }
            set
            {
                if (_scale == value) return;
                _scale = value;
                ClearCachedImage();
            }
        }

        public bool ShowOverlays
        {
            get { return _showOverlay; }
            set
            {
                if (_showOverlay == value) return;
                _showOverlay = value;
                ClearCachedImage();
            }
        }

        public IImage RenderImage()
        {
            lock (this)
            {

                if (_cachedImage == null)
                {
                    SetValues();
                    _cachedImage = _dicomImage.RenderImage(_frame);
                }

                return _cachedImage;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                ClearCachedImage();
                _disposed = true;
            }

        }

        public void Dispose()
        {
            Dispose(true);
        }

    }

}
