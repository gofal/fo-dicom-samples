using Dicom;
using Dicom.Imaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Viewer.Library.Models;
using Viewer.Library.Services;

namespace Viewer.Library.ViewModels
{

    public class ShellViewModel : INotifyPropertyChanged
    {

        #region FIELDS

        private readonly IDicomFileReaderService _readerService;

        private PatientlistModel _patients = new PatientlistModel();

        private ObservableCollection<FolderViewModel> _folers = new ObservableCollection<FolderViewModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand _openDirectoryCommand;

        #endregion

        #region CONSTRUCTORS

        public ShellViewModel(IDicomFileReaderService readerService)
        {
            this._readerService = readerService;
            var folder1 = new FolderViewModel(_patients) { RelativePositionX = 0, RelativePositionY = 0, RelativeWidth = 1, RelativeHeigh = 0.5 };
            _folers.Add(folder1);
            var folder2 = new FolderViewModel(_patients) { RelativePositionX = 0, RelativePositionY = 0.5, RelativeWidth = 0.5, RelativeHeigh = 0.5 };
            _folers.Add(folder2);
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// A Model containing a hierachial structure of Patients, Studies, Series and Images
        /// </summary>
        public PatientlistModel Patients => _patients;

        public ObservableCollection<FolderViewModel> Folders => _folers;

        public ICommand OpenDirectoryCommand
        {
            get
            {
                if (_openDirectoryCommand == null) _openDirectoryCommand = new ModelCommand<object>(this.OpenDirectory);
                return _openDirectoryCommand;
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Loads all the dicom-files from a directory und its subdirectories
        /// </summary>
        public async void OpenDirectory(object parameter)
        {
            // clear the current view
            ClearFoldersAndPatientmodel();

            var progressProcessor = new Progress<DicomFile>(_readerService_Progress);
            // load the files
            var files = await _readerService.GetFilesFromDirectoryAsync(progressProcessor);
            if (files == null || files.Count == 0)
            {
                return;
            }

            RaisePropertyChanged(nameof(Patients));
        }

        /// <summary>
        /// Loads specific user-selected files
        /// </summary>
        public async void OpenFiles()
        {
            // clear the current view
            ClearFoldersAndPatientmodel();

            // Todo: make a label or animated gif "loading" visible

            // load the files
            var files = await _readerService.GetFilesAsync();
            if (files == null || files.Count == 0)
            {
                return;
            }

            RaisePropertyChanged(nameof(Patients));
            // todo: hide a label or animated gif "loading"
        }

        private void ClearFoldersAndPatientmodel()
        {
            foreach (var fvm in _folers)
                fvm.CurrentSeriesInstanceUID = string.Empty;
            _patients.Clear();
        }

        private void _readerService_Progress(DicomFile file)
        {
            if (file != null)
            {
                DicomImage image = new DicomImage(file.Dataset);

                // await d.RunAsync(CoreDispatcherPriority.Normal, () =>
                // {
                _patients.InsertDicomFile(image);
                // RaisePropertyChanged("AsyncPatients");
                //});
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

}
