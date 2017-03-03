using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Library.Tools
{
    public class ImageSizer
    {

        private bool _dirty = true;

        private double _sourceWith;
        public double SourceWith
        {
            get { return _sourceWith; }
            set
            {
                if (value == _sourceWith) return;
                _sourceWith = value;
                _dirty = true;
            }
        }

        private double _sourceHeight;
        public double SourceHeight
        {
            get { return _sourceHeight; }
            set
            {
                if (_sourceHeight == value) return;
                _sourceHeight = value;
                _dirty = true;
            }
        }


        private double _destinationWith;
        public double DestinationWidth
        {
            get { return _destinationWith; }
            set
            {
                if (_destinationWith == value) return;
                _destinationWith = value;
                _dirty = true;
            }
        }

        private double _destinationHeight;
        public double DestinationHeight
        {
            get { return _destinationHeight; }
            set
            {
                if (_destinationHeight == value) return;
                _destinationHeight = value;
                _dirty = true;
            }
        }


        private double _scale;
        public double ResultingScale
        {
            get
            {
                if (_dirty) Calculate();
                return _scale;
            }
        }

        private double _resultingWidht;
        public double ResultingWidth
        {
            get
            {
                if (_dirty) Calculate();
                return _resultingWidht;
            }
        }

        private double _resultingHeight;
        public double ResultingHeight
        {
            get
            {
                if (_dirty) Calculate();
                return _resultingHeight;
            }
        }


        private double _offsetX;
        public double ResultingOffsetX
        {
            get
            {
                if (_dirty) Calculate();
                return _offsetX;
            }
        }

        private double _offsetY;
        public double ResultingOffsetY
        {
            get
            {
                if (_dirty) Calculate();
                return _offsetY;
            }
        }


        private void Calculate()
        {
            if (!_dirty) return;
            if (_sourceHeight / _sourceWith > _destinationHeight / _destinationWith)
            {
                // Portrait
                _scale = _destinationHeight / _sourceHeight;
                _resultingHeight = _destinationHeight;
                _resultingWidht = _sourceWith * _scale;
                _offsetY = 0;
                _offsetX = (_destinationWith - _resultingWidht) / 2;
            }
            else
            {
                // landscape
                _scale = _destinationWith / _sourceWith;
                _resultingWidht = _destinationWith;
                _resultingHeight = _sourceHeight * _scale;
                _offsetX = 0;
                _offsetY = (_destinationHeight - _resultingHeight) / 2;
            }
            _dirty = false;
        }

    }

}
