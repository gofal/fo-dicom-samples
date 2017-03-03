using Dicom;
using Dicom.Imaging;
using System;
using System.Collections.ObjectModel;


namespace Viewer.Library.Models
{

    /// <summary>
    /// Represents the main values of a Patient
    /// </summary>
    public class PatientModel
    {

        #region Fields

        private ObservableCollection<StudyModel> _studies = new ObservableCollection<StudyModel>();

        #endregion

        #region Constructor

        public PatientModel()
        { }

        public PatientModel(DicomDataset dcmFile)
        {
            PatientID = dcmFile.Get<string>(DicomTag.PatientID);
            PatientName = dcmFile.Get<string>(DicomTag.PatientName);
            Born = dcmFile.Get<DateTime>(DicomTag.PatientBirthDate, new DateTime(1900, 1, 1));
            PatientKey = GetKeyString(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The full name of the patient as stored in DicomTag 0010,0010
        /// </summary>
        public string PatientName { get; set; }

        /// <summary>
        ///  the PatientID as stored in DicomTag 0010,0020
        /// </summary>
        public string PatientID { get; set; }

        /// <summary>
        /// A unique Key for the patient. Mostly the PatientID but maybe also including the patients name or birthdate
        /// </summary>
        public string PatientKey { get; set; }

        /// <summary>
        /// Date of Birth
        /// </summary>
        public DateTime Born { get; set; }

        /// <summary>
        /// Date of Birth formated as string
        /// </summary>
        public string BornString => Born.ToString("dd.MM.yyyy");

        /// <summary>
        /// List of currently loaded Study of this patient
        /// </summary>
        public ObservableCollection<StudyModel> Studies => _studies;

        #endregion

        #region Static methods

        /// <summary>
        /// Generates a Key-String that should be unique for this patient. Eg a combination of patient ID, patient name or date of birth
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        public static string GetKeyString(DicomDataset dataset)
        {
            string patid = dataset.Get<string>(DicomTag.PatientID, string.Empty);
            if (string.IsNullOrEmpty(patid))
            {
                string patname = dataset.Get<string>(DicomTag.PatientName, string.Empty);
                DateTime patborn = dataset.Get<DateTime>(DicomTag.PatientBirthDate, new DateTime(1900, 1, 1));
                return patname + ", " + patborn.ToString("dd.MM.yyyy");
            }
            else
                return patid;
        }

        /// <summary>
        /// Generates a Key-String that should be unique for this patient. Eg a combination of patient ID, patient name or date of birth
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public static string GetKeyString(PatientModel patient)
        {
            if (string.IsNullOrEmpty(patient.PatientID))
            {
                return patient.PatientName + ", " + patient.Born.ToString("dd.MM.yyyy");
            }
            else
                return patient.PatientID;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Insert the DicomFile into the Collection and Create the StudyModel or SeriesModel if not already in the collection
        /// </summary>
        /// <param name="dicomFile"></param>
        public void InsertDicomFile(DicomImage dicomFile)
        {
            string studyUID = dicomFile.Dataset.Get<string>(DicomTag.StudyInstanceUID);

            // search if the study is already in the collection
            foreach (StudyModel s in _studies)
            {
                if (s.StudyInstanceUID == studyUID)
                {
                    s.InsertDicomFile(dicomFile);
                    return;
                }
            }

            StudyModel study = new StudyModel(dicomFile.Dataset);

            _studies.Add(study);
            study.InsertDicomFile(dicomFile);
        }

        /// <summary>
        /// Searches a serie by SeriesInstanceUID.
        /// </summary>
        /// <param name="seriesUID"></param>
        /// <returns>Returns the SeriesModel if found, and null otherwise</returns>
        public SeriesModel FindSerie(string seriesUID)
        {
            SeriesModel serie = null;
            foreach (StudyModel s in Studies)
            {
                serie = s.FindSerie(seriesUID);
                if (serie != null)
                    return serie;
            }
            return null;
        }

        #endregion
    }
}
