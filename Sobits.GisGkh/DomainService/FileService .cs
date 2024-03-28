
using Bars.B4.Modules.FileStorage;
using Castle.Windsor;
using GisGkhLibrary.Enums;
using Sobits.GisGkh.File;

namespace Sobits.GisGkh.DomainService
{
    public class FileService : IFileService
    {
        #region Constants



        #endregion

        #region Properties     

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private IFileManager _fileManager;

        #endregion

        #region Constructors

        public FileService(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        #endregion

        #region Public methods

        public FileUploadResult UploadFile(GisFileRepository repo, FileInfo File, string OrgPPAGUID)
        {
            //5 МБ = 5242880 Б 
            var uploaderName = File.Size <= 5242880 ? "SimpleFileUploader" : "FileUploader";

            var uploader = Container.Resolve<IFileUploader>(uploaderName);

            return uploader.UploadFile(repo, File, OrgPPAGUID);
        }

        public FileDownloadResult DownloadFile(string AttachmentGuid, string OrgPPAGUID)
        {
            var downloader = Container.Resolve<IFileDownloader>();
            return downloader.DownloadFile(AttachmentGuid, OrgPPAGUID);
        }

        #endregion
    }
}
