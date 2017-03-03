using Dicom;
using Dicom.Imaging;
using System;
using System.Collections.ObjectModel;

namespace Viewer.Library.Models
{

    /// <summary>
    /// Represents the main values of a Study
    /// </summary>
    public class StudyModel
    {

        #region Fields

        private ObservableCollection<SeriesModel> _series = new ObservableCollection<SeriesModel>();

        #endregion

        #region Constructor

        public StudyModel()
        { }

        public StudyModel(DicomDataset dcmFile)
        {
            StudyInstanceUID = dcmFile.Get<string>(DicomTag.StudyInstanceUID);
            StudyDate = dcmFile.GetDateTime(DicomTag.StudyDate, DicomTag.StudyTime);
            StudyDescription = dcmFile.Get<string>(DicomTag.StudyDescription, string.Empty);
        }

        #endregion

        #region Properties

        public string StudyInstanceUID { get; set; }

        public DateTime StudyDate { get; set; }

        public string StudyDescription { get; set; }

        public string StudyDateString => StudyDate.ToString("dd.MM.yyyy");

        public ObservableCollection<SeriesModel> Series => _series;

        #endregion

        #region Methods

        /// <summary>
        /// Insert the DicomFile into the Collection and Create the SeriesModel if not already in the collection
        /// </summary>
        /// <param name="dicomFile"></param>
        public void InsertDicomFile(DicomImage dicomFile)
        {
            string seriesUID = dicomFile.Dataset.Get<string>(DicomTag.SeriesInstanceUID);

            // search if the patient is already in the collection
            foreach (SeriesModel s in _series)
            {
                if (s.SeriesInstanceUID == seriesUID)
                {
                    s.InsertDicomFile(dicomFile);
                    return;
                }
            }

            SeriesModel series = new SeriesModel(dicomFile.Dataset);

            _series.Add(series);
            series.InsertDicomFile(dicomFile);
        }

        /// <summary>
        /// Searches the Model for the Serie.
        /// </summary>
        /// <param name="seriesUID">The unique Series Instance UID</param>
        /// <returns>Returns the SeriesModel if found and null otherwise</returns>
        public SeriesModel FindSerie(string seriesUID)
        {
            foreach (SeriesModel s in Series)
            {
                if (s.SeriesInstanceUID == seriesUID)
                    return s;
            }
            // if not found, return null
            return null;
        }

        #endregion

    }

}
