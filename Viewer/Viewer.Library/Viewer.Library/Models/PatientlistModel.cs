using Dicom.Imaging;
using System.Collections.ObjectModel;

namespace Viewer.Library.Models
{
    /// <summary>
    /// Represents the Collection of Patients and Studies and Series
    /// </summary>
    public class PatientlistModel : ObservableCollection<PatientModel>
    {

        /// <summary>
        /// Insert the DicomFile into the Collection and Create the PatientModel, StudyModel or SeriesModel if not already in the collection
        /// </summary>
        /// <param name="dicomFile"></param>
        public void InsertDicomFile(DicomImage dicomFile)
        {
            string patientKey = PatientModel.GetKeyString(dicomFile.Dataset);

            // search if the patient is already in the collection
            foreach (PatientModel p in this)
            {
                if (p.PatientKey == patientKey)
                {
                    p.InsertDicomFile(dicomFile);
                    return;
                }
            }

            PatientModel patient = new PatientModel(dicomFile.Dataset);

            this.Add(patient);
            patient.InsertDicomFile(dicomFile);
        }

        /// <summary>
        /// Find a Serie within the Model-Tree
        /// </summary>
        /// <param name="seriesUID"></param>
        /// <returns>Returns the SeriesModel if there is one with this SeriesUID, or null otherwise</returns>
        public SeriesModel FindSerie(string seriesUID)
        {
            SeriesModel serie = null;
            foreach (PatientModel p in this)
            {
                serie = p.FindSerie(seriesUID);
                if (serie != null)
                    return serie;
            }
            return null;
        }

    }

}
