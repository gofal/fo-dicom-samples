using Dicom;
using Dicom.Imaging;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Viewer.Library.Models
{

    /// <summary>
    /// Represents the main values of a Series
    /// </summary>
    public class SeriesModel : INotifyPropertyChanged
    {

        #region Fields

        private ObservableCollection<DicomImage> _images = new ObservableCollection<DicomImage>();

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public SeriesModel()
        { }

        public SeriesModel(DicomDataset dcmFile)
        {
            SeriesInstanceUID = dcmFile.Get<string>(DicomTag.SeriesInstanceUID);
            Modality = dcmFile.Get<string>(DicomTag.Modality);
            SeriesDescription = dcmFile.Get<string>(DicomTag.SeriesDescription, string.Empty);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The unique Series Instance UID
        /// </summary>
        public string SeriesInstanceUID { get; set; }

        /// <summary>
        /// Modality-Code for this Serie
        /// </summary>
        public string Modality { get; set; }

        /// <summary>
        /// Description of the Serie
        /// </summary>
        public string SeriesDescription { get; set; }

        /// <summary>
        /// List of Images loaded in this Serie
        /// </summary>
        public ObservableCollection<DicomImage> Images => _images;

        /// <summary>
        /// Number of currently loaded Images of this serie. There may be more Images in PACS or in the harddrive, but they have not been loaded at the moment
        /// </summary>
        public int ImagesCount => Images?.Count ?? 0;

        #endregion

        #region Methods

        /// <summary>
        /// Insert the DicomFile into the Collection
        /// </summary>
        /// <param name="dicomFile"></param>
        public void InsertDicomFile(DicomImage dicomFile)
        {
            // insert sorted;
            int i = 0;
            int instanceNumber = dicomFile.Dataset.Get<int>(DicomTag.InstanceNumber, 0);
            while (i < _images.Count && _images[i].Dataset.Get<int>(DicomTag.InstanceNumber, 0) < instanceNumber)
                i++;
            _images.Insert(i, dicomFile);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImagesCount)));
        }

        #endregion
    }

}
