using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage.Pickers;

using Dicom;
using Windows.Storage;
using Viewer.Library.Services;

namespace Viewer.Universal.Services
{
    public class DicomFileReaderService : IDicomFileReaderService
    {

        public async Task<IList<DicomFile>> GetFilesAsync()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".dcm");
            picker.FileTypeFilter.Add(".dic");

            var picks = await picker.PickMultipleFilesAsync().AsTask().ConfigureAwait(false);
            if (picks == null)
            {
                return null;
            }

            var streams = await Task.WhenAll(picks.Select(pick => pick.OpenStreamForReadAsync())).ConfigureAwait(false);
            var files = await Task.WhenAll(streams.Select(DicomFile.OpenAsync)).ConfigureAwait(false);

            return files.Where(file => file != null).ToList();
        }

        /// <summary>
        /// After showing a FolderPicker, all dicomfiles in the selected Folder and all its subdirectories are loaded
        /// </summary>
        /// <returns></returns>
        public async Task<IList<DicomFile>> GetFilesFromDirectoryAsync(IProgress<DicomFile> progress)
        {
            var picker = new FolderPicker();
            picker.ViewMode = PickerViewMode.List;
            picker.FileTypeFilter.Add("*");
            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.SettingsIdentifier = "FolderPicker";

            var pick = await picker.PickSingleFolderAsync().AsTask().ConfigureAwait(false);
            if (pick == null)
            {
                return null;
            }

            IEnumerable<StorageFile> files = await GetAllFilesAsync(pick);
            var x = files.Select(async file =>
            {
                try
                {
                    var s = await file.OpenStreamForReadAsync();
                    var f = DicomFile.Open(s);
                    // report the progress as soon as the file is ready
                    if (progress != null)
                    {
                        progress.Report(f);
                    }

                    return f;
                }
                catch (Exception e)
                {
                    var es = e.ToString();
                    return null;
                }
            });
            var dicomFiles = await Task.WhenAll(x);

            return dicomFiles.Where(dicomFile => dicomFile != null).ToList();
        }

        /// <summary>
        /// Findes all files from a Directory and all its subdirectories with a recursive search
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<StorageFile>> GetAllFilesAsync(StorageFolder folder)
        {
            IEnumerable<StorageFile> files = await folder.GetFilesAsync();
            IEnumerable<StorageFolder> folders = await folder.GetFoldersAsync();
            foreach (StorageFolder subfolder in folders)
                files = files.Concat(await GetAllFilesAsync(subfolder));
            return files;
        }


    }

}
