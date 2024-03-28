namespace Bars.Gkh1468.ViewModel
{
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class BaseProviderPassportViewModel<T> : BaseViewModel<T> where T : class, IEntity
    {
        private IFileManager _fileManager;

        public BaseProviderPassportViewModel(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        protected string GetSigny(FileInfo certificate)
        {
            if (certificate == null)
            {
                return string.Empty;
            }

            using (var certStream = new MemoryStream())
            {
                try
                {
                    _fileManager.GetFile(certificate).CopyTo(certStream);
                }
                catch (FileNotFoundException)
                {
                    return string.Empty;
                }

                var certX509 = new X509Certificate2(certStream.ToArray());

                return certX509.GetNameInfo(X509NameType.SimpleName, false);
            }
        }
    }
}