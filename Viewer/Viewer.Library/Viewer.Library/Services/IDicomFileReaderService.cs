using System.Collections.Generic;
using System.Threading.Tasks;

using Dicom;
using System;

namespace Viewer.Library.Services
{
    public delegate void ProgressDelegate(object state);

    public interface IDicomFileReaderService
    {

        Task<IList<DicomFile>> GetFilesAsync();

        Task<IList<DicomFile>> GetFilesFromDirectoryAsync(IProgress<DicomFile> progress);

    }
}
